using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms;
using WinFormsLabel = System.Windows.Forms.Label;

namespace WorkTimeCounterWidget
{
    public partial class WidgetForm : Form
    {
        protected override bool ShowWithoutActivation => true;

        private readonly ProjectRepository projectRepository;
        //zaokrąglenie formularza
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );
        private const string FilePath = "projects.json";
        private List<Project> projects = new List<Project>();
        private int currentProjectIndex = -1; // Brak projektu na początku
        private TimeSpan projectTime = TimeSpan.Zero;
        private TimeSpan breakTime = TimeSpan.Zero;
        private TimeSpan infoLineTime = TimeSpan.Zero;
        private bool isProjectRunning = false;
        private bool isTimerRunning = false;
        private bool isBreakRunning = false;
        private bool isInfoLineRunning = false;
        private DetailsForm detailsForm;
        private FontManager fontManager;

        private int tickCounter = 0;

        private Color defaultBackColor = ColorTranslator.FromHtml("#EAEBEC");
        private Color resizeBackColor = ColorTranslator.FromHtml("#757575");

        private CustomButton button_ShowMainWindow;
        private CustomButton button_Infolinia;
        private CustomButton button_Break;
        private CustomButton button_StartStop;
        private CustomButton button_Up;
        private CustomButton button_Down;
        private PictureBox pictureBox_digitalScreen;
        private WinFormsLabel label_ProjectName;
        private WinFormsLabel label_ProjectTime;
        private WinFormsLabel label_Hours;
        private WinFormsLabel label_Mins;
        private WinFormsLabel label_Secs;

        private int margin = 3;
        private int buttonWidthHorizontal;
        private int buttonHeightHorizontal;

        private int buttonHeightVertical;
        private int buttonWidthVertical;

        private readonly int minFormWidth = 315;
        private readonly int maxFormWidth = 600;
        private readonly int minFormHeight = 30;
        private readonly int baseButtonWidthHorizontal = 35;
        private readonly int baseButtonHeightHorizontal = 24;
        private readonly int baseButtonMargin = 3;


        public WidgetForm()
        {
            InitializeComponent();

            projectRepository = new ProjectRepository();
            projectRepository.ProjectsChanged += OnProjectsChanged;

            this.Resize += Form_Resize;
            this.MinimumSize = new Size(minFormWidth, minFormHeight);




            //zaokrąglenie formularza
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 5, 5));

            //PrivateFontCollection pfc = new PrivateFontCollection();
            //pfc.AddFontFile("C:\\Users\\user\\source\\repos\\WorkTimeCounterWidget\\WorkTimeCounterWidget\\Fonts\\Technology.ttf");
            //label_ProjectTime.Font = new Font(pfc.Families[0], 16, FontStyle.Regular);

            fontManager = new FontManager();
            try
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fontsFolder = Path.Combine(appDirectory, "Fonts");

                fontManager.LoadFontsFromFolder(fontsFolder);

                //label_ProjectTime.Font = fontManager.GetFont("Technology", 16, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania czcionek: {ex.Message}");
            }

            AddCustomButtons();
            AddDigitalScreen();
            AttachDoubleClickHandlers(this);
            projectRepository.LoadProjects();
            this.Location = projectRepository.WidgetLocation;
            this.Size = projectRepository.WidgetSize;
            UpdateButtonPositions();

            this.TopMost = true;
            this.ShowInTaskbar = false;
        }

        private void OnProjectsChanged()
        {
            projects = projectRepository.Projects;

            if (currentProjectIndex >= projects.Count)
            {
                currentProjectIndex = projects.Count - 1;
            }

            UpdateCurrentProject();
        }

        private void AttachDoubleClickHandlers(Control control)
        {
            // Dodaj tylko jeśli handler jeszcze nie jest przypisany
            if (!HasMouseDoubleClickHandler(control, ToggleResizeMode))
            {
                control.MouseDoubleClick += ToggleResizeMode;
            }

            foreach (Control child in control.Controls)
            {
                AttachDoubleClickHandlers(child);
            }
        }

        private bool HasMouseDoubleClickHandler(Control control, MouseEventHandler handlerToCheck)
        {
            var field = typeof(Control)
                .GetField("EventMouseDoubleClick", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            if (field == null) return false;

            object key = field.GetValue(null);
            if (key == null) return false;

            var events = (EventHandlerList)typeof(Component)
                .GetProperty("Events", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(control, null);

            Delegate d = events?[key];
            return d?.GetInvocationList().Contains(handlerToCheck) ?? false;
        }

        private void ToggleResizeMode(object sender, MouseEventArgs e)
        {
            isResizeModeEnabled = !isResizeModeEnabled;

            if (isResizeModeEnabled)
            {
                this.Cursor = Cursors.SizeNWSE;
                this.BackColor = resizeBackColor;
            }
            else
            {
                this.Cursor = Cursors.Default;
                this.BackColor = defaultBackColor;
                projectRepository.UpdateSize(this.Size);
            }

            SetButtonsEnabled(!isResizeModeEnabled);
        }

        private void SetButtonsEnabled(bool enabled)
        {
            button_ShowMainWindow.Enabled = enabled;
            button_Infolinia.Enabled = enabled;
            button_Break.Enabled = enabled;
            button_StartStop.Enabled = enabled;
            button_Up.Enabled = enabled;
            button_Down.Enabled = enabled;
        }

        private void AddDigitalScreen()
        {
            int pictureBoxWidth = CalculateDigitalScreenWidth();
            int pictureBoxHeight = button_Up.Height + button_Down.Height;

            pictureBox_digitalScreen = new PictureBox
            {
                Size = new Size(pictureBoxWidth, pictureBoxHeight),
                Location = new Point(
                    button_Up.Location.X + button_Up.Width + margin,
                    button_Up.Location.Y + (button_Up.Height / 2) - (pictureBoxHeight / 2)
                ),
                Image = Image.FromFile("Images/digitalScreenBG.png"),
                SizeMode = PictureBoxSizeMode.StretchImage, // lub Zoom, jeśli chcesz zachować proporcje
                BackColor = Color.Transparent
            };

            int halfWidth = pictureBoxWidth / 2;
            int upperHeight = (int)(pictureBoxHeight * 0.7);
            int lowerHeight = pictureBoxHeight - upperHeight;
            int thirdWidth = halfWidth / 3;

            // label_ProjectTime
            label_ProjectTime = new WinFormsLabel
            {
                Text = "00:00:00",
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 0),
                Size = new Size(halfWidth, upperHeight)
            };

            // label_Hours
            label_Hours = new WinFormsLabel
            {
                Name = "label_Hours",
                Text = "hours",
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(0, upperHeight),
                Size = new Size(thirdWidth, lowerHeight)
            };

            // label_Mins
            label_Mins = new WinFormsLabel
            {
                Name = "label_Mins",
                Text = "mins",
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(thirdWidth, upperHeight),
                Size = new Size(thirdWidth, lowerHeight)
            };

            // label_Secs
            label_Secs = new WinFormsLabel
            {
                Name = "label_Secs",
                Text = "secs",
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(thirdWidth * 2, upperHeight),
                Size = new Size(thirdWidth, lowerHeight)
            };

            // label_ProjectName (po prawej, wycentrowany w pionie)
            label_ProjectName = new WinFormsLabel
            {
                Text = "No project",
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(halfWidth, 0),
                Size = new Size(halfWidth, pictureBoxHeight)
            };

            AdjustFontsForDigitalScreen();

            pictureBox_digitalScreen.Controls.Add(label_ProjectTime);
            pictureBox_digitalScreen.Controls.Add(label_Hours);
            pictureBox_digitalScreen.Controls.Add(label_Mins);
            pictureBox_digitalScreen.Controls.Add(label_Secs);
            pictureBox_digitalScreen.Controls.Add(label_ProjectName);

            this.Controls.Add(pictureBox_digitalScreen);
        }


        private void Form_Resize(object sender, EventArgs e)
        {
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
        }

        private void AdjustFontsForDigitalScreen()
        {
            int screenHeight = pictureBox_digitalScreen.Height;

            // Wzorcowa wysokość digital screenu (dla której podałeś rozmiary czcionek)
            float baseHeight = 26f;

            // Obliczamy rozmiary czcionek w punktach
            float font_ProjectTime = 15f * screenHeight / baseHeight;
            float font_HoursMinsSecs = 3f * screenHeight / baseHeight;
            float font_ProjectName = 9f * screenHeight / baseHeight;

            label_ProjectTime.Font = fontManager.GetFont("Technology", font_ProjectTime, FontStyle.Bold);

            label_Hours.Font = new Font(label_Hours.Font.FontFamily, font_HoursMinsSecs, FontStyle.Regular, GraphicsUnit.Point);

            label_Mins.Font = new Font(label_Mins.Font.FontFamily, font_HoursMinsSecs, FontStyle.Regular, GraphicsUnit.Point);

            label_Secs.Font = new Font(label_Secs.Font.FontFamily, font_HoursMinsSecs, FontStyle.Regular, GraphicsUnit.Point);

            label_ProjectName.Font = fontManager.GetFont("Technology", font_ProjectName, FontStyle.Bold);
        }


        private void AddCustomButtons()
        {
            buttonWidthHorizontal = 35;
            buttonHeightHorizontal = 24;

            buttonHeightVertical = (this.ClientSize.Height / 2) - margin;
            buttonWidthVertical = GetScaledWidth("Images/down_normal.png", buttonHeightVertical);

            button_ShowMainWindow = new CustomButton
            {
                Size = new Size(buttonWidthHorizontal + 3, buttonHeightHorizontal),
                LocationState = CustomButton.ButtonLocation.Right,
                Orientation = CustomButton.ButtonOrientation.Horizontal
            };
            button_ShowMainWindow.LabelText = "DANE";
            button_ShowMainWindow.Click += button_ShowMainWindow_Click;
            this.Controls.Add(button_ShowMainWindow);

            button_Infolinia = new CustomButton
            {
                Size = new Size(buttonWidthHorizontal, buttonHeightHorizontal),
                LocationState = CustomButton.ButtonLocation.Mid,
                Orientation = CustomButton.ButtonOrientation.Horizontal
            };
            button_Infolinia.LabelText = "INFOLINIA";
            button_Infolinia.Click += button_Infolinia_Click;
            this.Controls.Add(button_Infolinia);

            button_Break = new CustomButton
            {
                Size = new Size(buttonWidthHorizontal, buttonHeightHorizontal),
                LocationState = CustomButton.ButtonLocation.Mid,
                Orientation = CustomButton.ButtonOrientation.Horizontal
            };
            button_Break.LabelText = "PRZERWA";
            button_Break.Click += button_Break_Click;
            this.Controls.Add(button_Break);

            button_StartStop = new CustomButton
            {
                Size = new Size(buttonWidthHorizontal, buttonHeightHorizontal),
                LocationState = CustomButton.ButtonLocation.Left,
                Orientation = CustomButton.ButtonOrientation.Horizontal,
            };
            button_StartStop.LabelText = "START";
            button_StartStop.Click += button_StartStop_Click;
            this.Controls.Add(button_StartStop);

            button_Up = new CustomButton
            {
                Size = new Size(buttonWidthVertical, buttonHeightVertical),
                LocationState = CustomButton.ButtonLocation.Up,
                Orientation = CustomButton.ButtonOrientation.Vertical,
            };
            button_Up.Click += button_Up_Click;
            this.Controls.Add(button_Up);

            button_Down = new CustomButton
            {
                Size = new Size(buttonWidthVertical, buttonHeightVertical),
                LocationState = CustomButton.ButtonLocation.Down,
                Orientation = CustomButton.ButtonOrientation.Vertical,
            };
            button_Down.Click += button_Down_Click;
            this.Controls.Add(button_Down);


            //this.Resize += (s, e) => UpdateButtonPositions();
        }

        // Funkcja do obliczania szerokości zachowując proporcje
        private int GetScaledWidth(string imagePath, int newHeight)
        {
            if (!File.Exists(imagePath)) return 50; // Domyślna szerokość, jeśli obraz nie istnieje

            using (Image img = Image.FromFile(imagePath))
            {
                float aspectRatio = (float)img.Width / img.Height;
                return (int)(newHeight * aspectRatio);
            }
        }

        //private int CalculateDigitalScreenWidth()
        //{
        //    int totalWidth = this.ClientSize.Width;
        //    int totalButtonsWidth = buttonWidthVertical + (4 * buttonWidthHorizontal);
        //    int totalMarginsWidth = 6 * margin; // margines lewy + prawy + między przyciskami

        //    return totalWidth - (totalButtonsWidth + totalMarginsWidth);
        //}

        //private int CalculateDigitalScreenWidth()
        //{
        //    Debug.WriteLine(pictureBox_digitalScreen.Width);
        //    int totalWidth = this.ClientSize.Width;

        //    // Szerokość przycisków
        //    int totalButtonsWidth = buttonWidthVertical;  // przycisk Up/Down
        //    totalButtonsWidth += (buttonWidthHorizontal * 4);  // przyciski poziome
        //    totalButtonsWidth += 3;  // dodatkowa szerokość dla button_ShowMainWindow

        //    // Marginesy
        //    int totalMarginsWidth = margin * 8;

        //    // Dodatkowe odsunięcie
        //    int additionalSpacing = 10;  // możesz dostosować tę wartość

        //    // Oblicz dostępną szerokość dla digitalScreen
        //    int availableWidth = totalWidth - (totalButtonsWidth + totalMarginsWidth + additionalSpacing);

        //    return availableWidth;
        //}

        private int CalculateDigitalScreenWidth(float _scale = 0)
        {
            if(_scale == 0)
            {
            //Debug.WriteLine(pictureBox_digitalScreen.Width);
            int totalWidth = this.ClientSize.Width;

            // Szerokość przycisków
            int totalButtonsWidth = button_Up.Width;  // przycisk Up/Down
            totalButtonsWidth += (button_Infolinia.Width * 4);  // przyciski poziome
            //totalButtonsWidth += 3;  // dodatkowa szerokość dla button_ShowMainWindow

            // Marginesy
            int totalMarginsWidth = margin * 8;

            // Dodatkowe odsunięcie
            int additionalSpacing = 0;  // możesz dostosować tę wartość

            // Oblicz dostępną szerokość dla digitalScreen
            int availableWidth = totalWidth - (totalButtonsWidth + totalMarginsWidth + additionalSpacing);

            return availableWidth;

            }
            else
            {
                //Debug.WriteLine(pictureBox_digitalScreen.Width);
                int totalWidth = this.ClientSize.Width;

                // Szerokość przycisków
                int totalButtonsWidth = button_Up.Width;  // przycisk Up/Down
                totalButtonsWidth += (button_Infolinia.Width * 4);  // przyciski poziome
                totalButtonsWidth += 3;  // dodatkowa szerokość dla button_ShowMainWindow

                // Marginesy
                int totalMarginsWidth = (int)(_scale * margin) * 4;

                // Dodatkowe odsunięcie
                int additionalSpacing = 0;  // możesz dostosować tę wartość

                // Oblicz dostępną szerokość dla digitalScreen
                int availableWidth = totalWidth - (totalButtonsWidth + totalMarginsWidth + additionalSpacing);

                return availableWidth;
            }
        }

        private void UpdateButtonPositions()
        {
            float scaleX = (float)this.ClientSize.Width / minFormWidth;
            float scaleY = (float)this.ClientSize.Height / minFormHeight;
            float scale = Math.Min(scaleX, scaleY);

            int margin = (int)(baseButtonMargin * scale);

            int buttonWidthHorizontal = (int)(baseButtonWidthHorizontal * scale);
            int buttonHeightHorizontal = (int)(baseButtonHeightHorizontal * scale);

            int buttonHeightVertical = (this.ClientSize.Height / 2) - margin;
            int buttonWidthVertical = GetScaledWidth("Images/down_normal.png", buttonHeightVertical);

            // Aktualizacja rozmiarów przycisków
            button_ShowMainWindow.Size = new Size(buttonWidthHorizontal + 3, buttonHeightHorizontal);
            button_ShowMainWindow.UpdateLabelFont(scale);
            button_ShowMainWindow.UpdateLabelPosition(scale);

            button_Infolinia.Size = new Size(buttonWidthHorizontal, buttonHeightHorizontal);
            button_Infolinia.UpdateLabelFont(scale);
            button_Infolinia.UpdateLabelPosition(scale);

            button_Break.Size = new Size(buttonWidthHorizontal, buttonHeightHorizontal);
            button_Break.UpdateLabelFont(scale);
            button_Break.UpdateLabelPosition(scale);

            button_StartStop.Size = new Size(buttonWidthHorizontal, buttonHeightHorizontal);
            button_StartStop.UpdateLabelFont(scale);
            button_StartStop.UpdateLabelPosition(scale);

            button_Up.Size = new Size(buttonWidthVertical, buttonHeightVertical);
            button_Down.Size = new Size(buttonWidthVertical, buttonHeightVertical);



            // Pozycje przycisków
            button_ShowMainWindow.Location = new Point(this.ClientSize.Width - button_ShowMainWindow.Width - margin, margin);
            button_Infolinia.Location = new Point(button_ShowMainWindow.Location.X - button_Infolinia.Width, margin);
            button_Break.Location = new Point(button_Infolinia.Location.X - button_Break.Width, margin);
            button_StartStop.Location = new Point(button_Break.Location.X - button_StartStop.Width, margin);
            button_Up.Location = new Point(margin, margin);
            button_Down.Location = new Point(margin, button_Up.Height + margin);


            int pictureBoxWidth = CalculateDigitalScreenWidth(scale);
            int pictureBoxHeight = button_Up.Height + button_Down.Height;

            pictureBox_digitalScreen.Location = new Point(button_Up.Right + margin, margin);
            pictureBox_digitalScreen.Size = new Size(pictureBoxWidth, pictureBoxHeight);

            int halfWidth = pictureBoxWidth / 2;
            int upperHeight = (int)(pictureBoxHeight * 0.8);
            int lowerHeight = pictureBoxHeight - upperHeight;
            int thirdWidth = halfWidth / 3;
            int hoursMinsSecsYLocation = (int)(upperHeight * 0.9);

            // label_ProjectTime
            label_ProjectTime.Location = new Point(0, 0);
            label_ProjectTime.Size = new Size(halfWidth, upperHeight);
            //AdjustFontToLabel(label_ProjectTime);

            // label_Hours
            pictureBox_digitalScreen.Controls["label_Hours"].Location = new Point(margin, hoursMinsSecsYLocation);
            pictureBox_digitalScreen.Controls["label_Hours"].Size = new Size((int)(thirdWidth * 0.8), lowerHeight);
            //AdjustFontToLabel((WinFormsLabel)pictureBox_digitalScreen.Controls["label_Hours"]);

            // label_Mins
            pictureBox_digitalScreen.Controls["label_Mins"].Location = new Point(thirdWidth + margin, hoursMinsSecsYLocation);
            pictureBox_digitalScreen.Controls["label_Mins"].Size = new Size((int)(thirdWidth * 0.7), lowerHeight);
            //AdjustFontToLabel((WinFormsLabel)pictureBox_digitalScreen.Controls["label_Mins"]);

            // label_Secs
            pictureBox_digitalScreen.Controls["label_Secs"].Location = new Point((int)((pictureBox_digitalScreen.Controls["label_Mins"].Width + pictureBox_digitalScreen.Controls["label_Hours"].Width)*1.3) + margin, hoursMinsSecsYLocation);
            pictureBox_digitalScreen.Controls["label_Secs"].Size = new Size((int)(thirdWidth * 0.7), lowerHeight);
            //AdjustFontToLabel((WinFormsLabel)pictureBox_digitalScreen.Controls["label_Secs"]);

            // label_ProjectName
            label_ProjectName.Size = new Size(halfWidth, pictureBoxHeight);
            label_ProjectName.Location = new Point(pictureBox_digitalScreen.Width - label_ProjectName.Width - (int)(margin * scale), 0);

            AdjustFontsForDigitalScreen();
        }


        public void UpdateCurrentProject()
        {
            if (currentProjectIndex >= 0 && currentProjectIndex < projects.Count)
            {
                var currentProject = projects[currentProjectIndex];
                label_ProjectName.Text = currentProject.Name;
                label_ProjectTime.Text = currentProject.TimeSpent.ToString(@"hh\:mm\:ss");
            }
            else
            {
                label_ProjectName.Text = "No project selected";
                label_ProjectTime.Text = "";
            }
        }





        private void button_StartStop_Click(object sender, EventArgs e)
        {
            if (isTimerRunning)
            {
                timer.Stop();
                isProjectRunning = false;
                button_StartStop.Text = "PAUSED";
                //button_StartStop.BackColor = ColorTranslator.FromHtml("#FF0000");
            }
            else
            {
                timer.Start();
                isProjectRunning = true;
                button_StartStop.Text = "STARTED";
                //button_StartStop.BackColor = ColorTranslator.FromHtml("#008000");
            }

            isTimerRunning = !isTimerRunning;
        }

        private void button_Break_Click(object sender, EventArgs e)
        {
            if (isBreakRunning)
            {
                // Kończymy przerwę, wznawiamy licznik projektu
                isBreakRunning = false;
                timer.Start();

                // Przywracamy informacje o bieżącym projekcie
                if (currentProjectIndex >= 0 && currentProjectIndex < projects.Count)
                {
                    var currentProject = projects[currentProjectIndex];
                    label_ProjectName.Text = currentProject.Name;
                    label_ProjectTime.Text = currentProject.TimeSpent.ToString(@"hh\:mm\:ss");
                }

                button_StartStop.Enabled = true;
                //button_StartStop.BackColor = ColorTranslator.FromHtml("#008000");
                button_Infolinia.Enabled = true;
                //button_Infolinia.BackColor = Color.FromArgb(64, 64, 64);
                //button_Break.BackColor = Color.FromArgb(64, 64, 64);
            }
            else
            {
                // Rozpoczynamy przerwę, zatrzymujemy licznik projektu
                isBreakRunning = true;
                timer.Start();

                // Resetujemy czas przerwy i zmieniamy informacje wyświetlane w UI
                label_ProjectName.Text = "Przerwa";
                label_ProjectTime.Text = breakTime.ToString(@"hh\:mm\:ss");

                button_StartStop.Enabled = false;
                //button_StartStop.BackColor = Color.Gray;
                button_Infolinia.Enabled = false;
                //button_Infolinia.BackColor = Color.Gray;
                //button_Break.BackColor = ColorTranslator.FromHtml("#008000");
            }
        }



        private void button_Infolinia_Click(object sender, EventArgs e)
        {
            if (isInfoLineRunning)
            {
                // Kończymy liczenie infolinii
                isInfoLineRunning = false;
                timer.Start();

                // Przywracamy informacje o bieżącym projekcie
                if (currentProjectIndex >= 0 && currentProjectIndex < projects.Count)
                {
                    var currentProject = projects[currentProjectIndex];
                    label_ProjectName.Text = currentProject.Name;
                    label_ProjectTime.Text = currentProject.TimeSpent.ToString(@"hh\:mm\:ss");
                }

                // Aktywujemy przycisk Start/Stop
                button_StartStop.Enabled = true;
                //button_StartStop.BackColor = ColorTranslator.FromHtml("#008000");
                button_Break.Enabled = true;
                //button_Break.BackColor = Color.FromArgb(64, 64, 64);

                // Zmieniamy kolor przycisku infolinii
                //button_Infolinia.BackColor = Color.FromArgb(64, 64, 64);
            }
            else
            {
                // Rozpoczynamy liczenie czasu infolinii
                isInfoLineRunning = true;
                timer.Start();

                // Resetujemy czas infolinii i zmieniamy informacje w UI
                label_ProjectName.Text = "Infolinia";
                label_ProjectTime.Text = infoLineTime.ToString(@"hh\:mm\:ss");

                // Dezaktywujemy przycisk Start/Stop
                button_StartStop.Enabled = false;
                //button_StartStop.BackColor = Color.Gray;
                button_Break.Enabled = false;
                //button_Break.BackColor = Color.Gray;

                // Zmieniamy kolor przycisku infolinii
                //button_Infolinia.BackColor = ColorTranslator.FromHtml("#008000");
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (isProjectRunning && !isBreakRunning && !isInfoLineRunning && currentProjectIndex >= 0 && currentProjectIndex < projects.Count)
            {
                // Liczymy czas dla bieżącego projektu
                var currentProject = projects[currentProjectIndex];
                currentProject.TimeSpent = currentProject.TimeSpent.Add(TimeSpan.FromSeconds(1));
                label_ProjectTime.Text = currentProject.TimeSpent.ToString(@"hh\:mm\:ss");
            }
            else if (isBreakRunning)
            {
                // Liczymy czas dla przerwy
                breakTime = breakTime.Add(TimeSpan.FromSeconds(1));
                label_ProjectTime.Text = breakTime.ToString(@"hh\:mm\:ss");
            }
            else if (isInfoLineRunning)
            {
                // Liczymy czas dla infolinii
                infoLineTime = infoLineTime.Add(TimeSpan.FromSeconds(1));
                label_ProjectTime.Text = infoLineTime.ToString(@"hh\:mm\:ss");
            }

            tickCounter++;

            if (tickCounter >= 30)
            {
                tickCounter = 0;
                this.TopMost = false;
                this.TopMost = true;
            }
        }





        private void button_Up_Click(object sender, EventArgs e)
        {
            if (projects.Count == 0 || isTimerRunning) return;  // Sprawdzamy, czy czas jest aktywny

            currentProjectIndex = (currentProjectIndex - 1 + projects.Count) % projects.Count;
            UpdateCurrentProject();
        }

        private void button_Down_Click(object sender, EventArgs e)
        {
            if (projects.Count == 0 || isTimerRunning) return;  // Sprawdzamy, czy czas jest aktywny

            currentProjectIndex = (currentProjectIndex + 1) % projects.Count;
            UpdateCurrentProject();
        }


        private void button_ShowMainWindow_Click(object sender, EventArgs e)
        {
            if (detailsForm == null || detailsForm.IsDisposed)
            {
                detailsForm = new DetailsForm(projectRepository);
            }

            if (detailsForm.Visible)
            {
                detailsForm.Hide();
            }
            else
            {
                detailsForm.ReceiveProjectsData(projectRepository.Projects, breakTime, infoLineTime);
                detailsForm.Show();
            }
        }

        bool mouseDown;
        private Point offset;
        private Point lastMousePosition;
        private const int resizeBorderThickness = 10;

        private bool isResizeModeEnabled = false;
        private bool isResizing = false;
        private Point resizeStartMouse;
        private Size resizeStartSize;

        private void mouseDown_Event(object sender, MouseEventArgs e)
        {
            if (isResizeModeEnabled)
            {
                isResizing = true;
                resizeStartMouse = Cursor.Position; // globalna pozycja kursora
                resizeStartSize = this.Size;
            }
            else
            {
                offset = new Point(e.X, e.Y);
                mouseDown = true;
            }
        }

        private void mouseMove_Event(object sender, MouseEventArgs e)
        {
            //if (isResizeModeEnabled && isResizing)
            //{
            //    Point currentMouse = Cursor.Position;
            //    int deltaX = currentMouse.X - resizeStartMouse.X;

            //    // Wyliczamy nową szerokość z uwzględnieniem minimum
            //    int newWidth = Math.Max(minFormWidth, resizeStartSize.Width + deltaX);

            //    // Oblicz proporcję na podstawie początkowych rozmiarów
            //    float aspectRatio = (float)resizeStartSize.Height / resizeStartSize.Width;

            //    // Ustal wysokość na podstawie nowej szerokości
            //    int newHeight = (int)(newWidth * aspectRatio);

            //    // Minimalna wysokość
            //    if (newHeight < minFormHeight)
            //    {
            //        newHeight = minFormHeight;
            //        newWidth = (int)(newHeight / aspectRatio); // Dopasuj szerokość, żeby zachować proporcje
            //    }

            //    // Sprawdź, czy nowy rozmiar nie wykracza poza ekran
            //    Rectangle screenBounds = Screen.FromControl(this).Bounds;
            //    if (this.Location.X + newWidth > screenBounds.Width)
            //    {
            //        newWidth = screenBounds.Width - this.Location.X;
            //        newHeight = (int)(newWidth * aspectRatio);
            //    }

            //    this.Size = new Size(newWidth, newHeight);

            //    // Przeskaluj przyciski po zmianie rozmiaru
            //    UpdateButtonPositions();
            //}
            //if (isResizeModeEnabled && isResizing)
            //{
            //    Point currentMouse = Cursor.Position;
            //    int deltaX = currentMouse.X - resizeStartMouse.X;

            //    // Wyliczamy nową szerokość z uwzględnieniem minimum
            //    int newWidth = Math.Max(minFormWidth, resizeStartSize.Width + deltaX);

            //    // Zachowujemy aktualną wysokość
            //    int newHeight = this.Height;

            //    // Sprawdź, czy nowy rozmiar nie wykracza poza ekran
            //    Rectangle screenBounds = Screen.FromControl(this).Bounds;
            //    if (this.Location.X + newWidth > screenBounds.Width)
            //    {
            //        newWidth = screenBounds.Width - this.Location.X;
            //    }

            //    this.Size = new Size(newWidth, newHeight);

            //    // Przeskaluj przyciski po zmianie rozmiaru
            //    UpdateButtonPositions();
            //}
            //if (isResizeModeEnabled && isResizing)
            //{
            //    Point currentMouse = Cursor.Position;
            //    int deltaX = currentMouse.X - resizeStartMouse.X;

            //    // Wyliczamy nową szerokość z uwzględnieniem minimum
            //    int newWidth = Math.Max(minFormWidth, resizeStartSize.Width + deltaX);

            //    // Oblicz proporcję na podstawie minimalnych wymiarów
            //    float aspectRatio = (float)minFormHeight / minFormWidth;

            //    // Ustal wysokość na podstawie nowej szerokości, zachowując proporcje względem wymiarów minimalnych
            //    int newHeight = (int)(newWidth * aspectRatio);

            //    // Sprawdź, czy nowy rozmiar nie wykracza poza ekran
            //    Rectangle screenBounds = Screen.FromControl(this).Bounds;
            //    if (this.Location.X + newWidth > screenBounds.Width)
            //    {
            //        newWidth = screenBounds.Width - this.Location.X;
            //        newHeight = (int)(newWidth * aspectRatio);
            //    }

            //    this.Size = new Size(newWidth, newHeight);

            //    // Przeskaluj przyciski po zmianie rozmiaru
            //    UpdateButtonPositions();
            //}
            if (isResizeModeEnabled && isResizing)
            {
                Point currentMouse = Cursor.Position;
                int deltaX = currentMouse.X - resizeStartMouse.X;

                // Oblicz nową szerokość z uwzględnieniem min i max
                int newWidth = resizeStartSize.Width + deltaX;
                newWidth = Math.Max(minFormWidth, newWidth);
                newWidth = Math.Min(maxFormWidth, newWidth);

                // Zachowaj proporcje względem minimalnych wymiarów
                float aspectRatio = (float)minFormHeight / minFormWidth;
                int newHeight = (int)(newWidth * aspectRatio);

                // Upewnij się, że nowy rozmiar mieści się na ekranie
                Rectangle screenBounds = Screen.FromControl(this).Bounds;
                if (this.Location.X + newWidth > screenBounds.Width)
                {
                    newWidth = screenBounds.Width - this.Location.X;
                    newHeight = (int)(newWidth * aspectRatio);
                }

                this.Size = new Size(newWidth, newHeight);

                // Przeskaluj przyciski po zmianie rozmiaru
                UpdateButtonPositions();
            }
            else if (!isResizeModeEnabled && mouseDown)
            {
                Point currentScreenPosition = PointToScreen(e.Location);
                Point newLocation = new Point(currentScreenPosition.X - offset.X, currentScreenPosition.Y - offset.Y);

                // Pobierz wymiary całego ekranu, włącznie z paskiem zadań
                Rectangle screenBounds = Screen.FromControl(this).Bounds;

                // Ogranicz położenie X - widget nie może wyjść poza ekran w poziomie
                newLocation.X = Math.Max(screenBounds.X, newLocation.X); // Lewa krawędź
                newLocation.X = Math.Min(screenBounds.Width - this.Width, newLocation.X); // Prawa krawędź

                // Ogranicz położenie Y - widget może być schowany, ale musi być widoczny na wysokość swojego rozmiaru
                newLocation.Y = Math.Max(screenBounds.Y - this.Height + this.Height, newLocation.Y); // Górna krawędź
                newLocation.Y = Math.Min(screenBounds.Height - this.Height, newLocation.Y); // Dolna krawędź

                Location = newLocation;
            }
        }

        private void mouseUp_Event(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            isResizing = false;
            projectRepository.UpdateLocation(this.Location);
        }



    }

    public class FontManager
    {
        private PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        public void LoadFontsFromFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Folder czcionek nie istnieje: {folderPath}");
            }

            foreach (var fontFile in Directory.GetFiles(folderPath, "*.ttf"))
            {
                privateFontCollection.AddFontFile(fontFile);
            }
        }

        public Font GetFont(string fontFamilyName, float size, FontStyle style = FontStyle.Regular)
        {
            var family = Array.Find(privateFontCollection.Families, f => f.Name.Equals(fontFamilyName, StringComparison.OrdinalIgnoreCase));
            if (family == null)
            {
                throw new Exception($"Nie znaleziono czcionki: {fontFamilyName}");
            }

            return new Font(family, size, style);
        }
    }
}
