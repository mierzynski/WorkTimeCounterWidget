using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WorkTimeCounterWidget
{
    public partial class WidgetForm : Form
    {
        private List<Project> projects = new List<Project>();
        private int currentProjectIndex = -1; // Brak projektu na początku
        private TimeSpan projectTime = TimeSpan.Zero;
        private bool isProjectRunning = false;
        private bool isTimerRunning = false;
        private bool isBreakRunning = false;

        private Form1 detailsForm;

        public WidgetForm()
        {
            InitializeComponent();


            detailsForm = new Form1();

            detailsForm.ProjectsUpdated += projects =>
            {
                this.projects = projects;
                UpdateCurrentProject();
            };

            detailsForm.Hide();
            detailsForm.LoadProjectsFromFile();
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
                label_ProjectTime.Text = "00:00:00";
            }
        }





        private void button_StartStop_Click(object sender, EventArgs e)
        {
            if (isTimerRunning)
            {
                timer.Stop();
                isProjectRunning = false;
                button_StartStop.Text = "PAUSED";
                button_StartStop.BackColor = ColorTranslator.FromHtml("#FF0000");
            }
            else
            {
                timer.Start();
                isProjectRunning = true;
                button_StartStop.Text = "STARTED";
                button_StartStop.BackColor = ColorTranslator.FromHtml("#008000");
            }

            isTimerRunning = !isTimerRunning;
        }
        private void button_Break_Click(object sender, EventArgs e)
        {
            if (isBreakRunning)
            {
                // Wznawiamy stoper dla projektu
                timer.Start();
                isBreakRunning = false;

                // Zmieniamy tekst i kolor przycisku
                button_Break.BackColor = Color.FromArgb(64, 64, 64);

                // Zmieniamy stan przycisku Start/Stop
                button_StartStop.Text = "STARTED";
                button_StartStop.BackColor = ColorTranslator.FromHtml("#008000");
            }
            else
            {
                // Pauzujemy czas dla projektu i zaczynamy przerwę
                timer.Stop();
                isBreakRunning = true;

                // Zmieniamy tekst i kolor przycisku przerwy
                button_Break.BackColor = ColorTranslator.FromHtml("#008000");

                // Zmieniamy stan przycisku Start/Stop
                button_StartStop.Text = "PAUSED";
                button_StartStop.BackColor = ColorTranslator.FromHtml("#FF0000");
            }
        }
        private void button_Infolinia_Click(object sender, EventArgs e)
        {

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (isProjectRunning && currentProjectIndex >= 0 && currentProjectIndex < projects.Count)
            {
                var currentProject = projects[currentProjectIndex];
                currentProject.TimeSpent = currentProject.TimeSpent.Add(TimeSpan.FromSeconds(1));
                label_ProjectTime.Text = currentProject.TimeSpent.ToString(@"hh\:mm\:ss");
            }
        }

        private void button_Up_Click_Click(object sender, EventArgs e)
        {
            if (projects.Count == 0 || isTimerRunning) return;  // Sprawdzamy, czy czas jest aktywny

            currentProjectIndex = (currentProjectIndex - 1 + projects.Count) % projects.Count;
            UpdateCurrentProject();
        }

        private void button_Down_Click_Click(object sender, EventArgs e)
        {
            if (projects.Count == 0 || isTimerRunning) return;  // Sprawdzamy, czy czas jest aktywny

            currentProjectIndex = (currentProjectIndex + 1) % projects.Count;
            UpdateCurrentProject();
        }

        private void button_ShowMainWindow_Click_1(object sender, EventArgs e)
        {
            if (detailsForm == null || detailsForm.IsDisposed)
            {
                detailsForm = new Form1();
                detailsForm.ProjectsUpdated += projects =>
                {
                    this.projects = projects;
                    UpdateCurrentProject();
                };
                detailsForm.Show();
            }
            else
            {
                if (detailsForm.Visible)
                {
                    detailsForm.Hide();
                }
                else
                {
                    detailsForm.Show();
                }
            }
        }

    }
}
