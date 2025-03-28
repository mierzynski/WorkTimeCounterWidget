﻿using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WorkTimeCounterWidget
{
    public partial class WidgetForm : Form
    {
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

        private CustomButton button_ShowMainWindow;
        private CustomButton button_Infolinia;
        private CustomButton button_Break;
        private CustomButton button_StartStop;
        private CustomButton button_Up;
        private CustomButton button_Down;
        public WidgetForm()
        {
            InitializeComponent();

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

                label_ProjectTime.Font = fontManager.GetFont("Technology", 16, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania czcionek: {ex.Message}");
            }


            detailsForm = new DetailsForm();

            detailsForm.ProjectsUpdated += projects =>
            {
                this.projects = projects;
                UpdateCurrentProject();
            };

            detailsForm.Hide();
            detailsForm.LoadProjectsFromFile();

            AddCustomButtons();
        }

        private void AddCustomButtons()
        {
            int buttonWidthHorizontal = 35;
            int buttonHeightHorizontal = 24;

            int buttonHeightVertical = this.ClientSize.Height / 2;
            int buttonWidthVertical = GetScaledWidth("Images/down_normal.png", buttonHeightVertical);
            int margin = 3;

            button_ShowMainWindow = new CustomButton
            {
                Size = new Size(buttonWidthHorizontal + 3, buttonHeightHorizontal),
                LocationState = CustomButton.ButtonLocation.Right,
                Orientation = CustomButton.ButtonOrientation.Horizontal
            };
            button_ShowMainWindow.Click += button_ShowMainWindow_Click;
            this.Controls.Add(button_ShowMainWindow);

            button_Infolinia = new CustomButton
            {
                Size = new Size(buttonWidthHorizontal, buttonHeightHorizontal),
                LocationState = CustomButton.ButtonLocation.Mid,
                Orientation = CustomButton.ButtonOrientation.Horizontal
            };
            button_Infolinia.Click += button_Infolinia_Click;
            this.Controls.Add(button_Infolinia);

            button_Break = new CustomButton
            {
                Size = new Size(buttonWidthHorizontal, buttonHeightHorizontal),
                LocationState = CustomButton.ButtonLocation.Mid,
                Orientation = CustomButton.ButtonOrientation.Horizontal
            };
            button_Break.Click += button_Break_Click;
            this.Controls.Add(button_Break);

            button_StartStop = new CustomButton
            {
                Size = new Size(buttonWidthHorizontal, buttonHeightHorizontal),
                LocationState = CustomButton.ButtonLocation.Left,
                Orientation = CustomButton.ButtonOrientation.Horizontal
            };
            button_StartStop.Click += button_StartStop_Click;
            this.Controls.Add(button_StartStop);

            // Przycisk UP
            button_Up = new CustomButton
            {
                Size = new Size(buttonWidthVertical, buttonHeightVertical),
                LocationState = CustomButton.ButtonLocation.Up,
                Orientation = CustomButton.ButtonOrientation.Vertical,
            };
            button_Up.Click += button_Up_Click;
            this.Controls.Add(button_Up);

            // Przycisk DOWN
            button_Down = new CustomButton
            {
                Size = new Size(buttonWidthVertical, buttonHeightVertical),
                LocationState = CustomButton.ButtonLocation.Down,
                Orientation = CustomButton.ButtonOrientation.Vertical,
            };
            button_Down.Click += button_Down_Click;
            this.Controls.Add(button_Down);

            UpdateButtonPositions();

            this.Resize += (s, e) => UpdateButtonPositions();
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

        private void UpdateButtonPositions()
        {
            int margin = 3;

            button_ShowMainWindow.Location = new Point(
                this.ClientSize.Width - button_ShowMainWindow.Width - margin,
                margin
            );

            button_Infolinia.Location = new Point(
                button_ShowMainWindow.Location.X - button_Infolinia.Width,
                button_ShowMainWindow.Location.Y
            );

            button_Break.Location = new Point(
                button_Infolinia.Location.X - button_Break.Width,
                button_Infolinia.Location.Y
            );

            button_StartStop.Location = new Point(
                button_Break.Location.X - button_StartStop.Width,
                button_Break.Location.Y
            );

            button_Down.Location = new Point(
                margin,
                button_Up.Location.Y + button_Up.Height
            );

            button_Up.Location = new Point(
                margin,
                0
            );
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

            if (detailsForm.Visible)
            {
                detailsForm.Hide();
            }
            else
            {
                detailsForm = new DetailsForm();

                // Przesyłanie listy projektów, czasu przerwy i infolinii
                detailsForm.ReceiveProjectsData(projects, breakTime, infoLineTime);

                detailsForm.ProjectsUpdated += projects =>
                {
                    this.projects = projects;
                    UpdateCurrentProject();
                };
                detailsForm.Show();
            }
        }

        bool mouseDown;
        private Point offset;
        private void mouseDown_Event(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;
            mouseDown = true;
        }

        private void mouseMove_Event(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPosition = PointToScreen(e.Location);
                Location = new Point(currentScreenPosition.X - offset.X, currentScreenPosition.Y - offset.Y);
            }
        }

        private void mouseUp_Event(object sender, MouseEventArgs e)
        {
            mouseDown = false;

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
