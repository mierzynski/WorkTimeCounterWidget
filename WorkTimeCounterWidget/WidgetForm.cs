//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace WorkTimeCounterWidget
//{
//    public partial class WidgetForm : Form
//    {
//        private TimeSpan projectTime = TimeSpan.Zero;  // Czas dla projektu
//        private TimeSpan breakTime = TimeSpan.Zero;    // Czas dla przerwy
//        private bool isProjectRunning = false;          // Status czy stoper dla projektu jest uruchomiony
//        private bool isBreakRunning = false;            // Status czy przerwa jest uruchomiona
//        private bool isTimerRunning = false;            // Status ogólny timera (czy zlicza czas)

//        public WidgetForm()
//        {
//            InitializeComponent();
//        }

//        private void button_StartStop_Click(object sender, EventArgs e)
//        {
//            if (isTimerRunning)
//            {
//                // Zatrzymujemy stoper dla projektu
//                timer.Stop();
//                isProjectRunning = false;

//                // Zmieniamy tekst i kolor przycisku
//                button_StartStop.Text = "Paused";
//                button_StartStop.BackColor = Color.Red;
//            }
//            else
//            {
//                // Rozpoczynamy stoper dla projektu
//                timer.Start();
//                isProjectRunning = true;

//                // Zmieniamy tekst i kolor przycisku
//                button_StartStop.Text = "Started";
//                button_StartStop.BackColor = Color.Green;
//            }
//            isTimerRunning = !isTimerRunning;  // Przełączamy stan timera
//        }


//        private void button_Break_Click(object sender, EventArgs e)
//        {
//            if (isBreakRunning)
//            {
//                // Wznawiamy stoper dla projektu
//                timer.Start();
//                isBreakRunning = false;

//                // Zmieniamy tekst i kolor przycisku
//                button_Break.Text = "Break";
//                button_Break.BackColor = Color.Red;

//                // Zmieniamy stan przycisku Start/Stop
//                button_StartStop.Text = "Started";
//                button_StartStop.BackColor = Color.Green;
//            }
//            else
//            {
//                // Pauzujemy czas dla projektu i zaczynamy przerwę
//                timer.Stop();
//                isBreakRunning = true;

//                // Zmieniamy tekst i kolor przycisku przerwy
//                button_Break.Text = "Resume";
//                button_Break.BackColor = Color.Green;

//                // Zmieniamy stan przycisku Start/Stop
//                button_StartStop.Text = "Paused";
//                button_StartStop.BackColor = Color.Red;
//            }
//        }


//        private void button_Infolinia_Click(object sender, EventArgs e)
//        {

//        }

//        private void timer_Tick(object sender, EventArgs e)
//        {
//            if (isProjectRunning)
//            {
//                // Zliczamy czas dla projektu
//                projectTime = projectTime.Add(TimeSpan.FromSeconds(1));
//                label_ProjectTime.Text = projectTime.ToString(@"hh\:mm\:ss");
//            }

//            if (isBreakRunning)
//            {
//                // Zliczamy czas na przerwie
//                breakTime = breakTime.Add(TimeSpan.FromSeconds(1));
//                label_ProjectTime.Text = breakTime.ToString(@"hh\:mm\:ss");  // Przerwa wyświetlana w tej samej etykiecie
//            }
//        }
//    }
//}


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

        public WidgetForm()
        {
            InitializeComponent();
        }

        public void UpdateProjectList(List<Project> projectList)
        {
            projects = projectList;
            if (projects.Count > 0)
            {
                currentProjectIndex = 0; // Ustaw pierwszy projekt
                UpdateCurrentProject();
            }
            else
            {
                currentProjectIndex = -1;
                label_ProjectName.Text = "No project selected";
                label_ProjectTime.Text = "00:00:00";
            }
        }


        private void UpdateCurrentProject()
        {
            if (currentProjectIndex >= 0 && currentProjectIndex < projects.Count)
            {
                var currentProject = projects[currentProjectIndex];
                label_ProjectName.Text = currentProject.Name;
                label_ProjectTime.Text = currentProject.TimeSpent.ToString(@"hh\:mm\:ss");
            }
        }


        private void button_StartStop_Click(object sender, EventArgs e)
        {
            if (isTimerRunning)
            {
                timer.Stop();
                isProjectRunning = false;
                button_StartStop.Text = "Paused";
                button_StartStop.BackColor = Color.Red;
            }
            else
            {
                timer.Start();
                isProjectRunning = true;
                button_StartStop.Text = "Started";
                button_StartStop.BackColor = Color.Green;
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
                button_Break.Text = "Break";
                button_Break.BackColor = Color.Red;

                // Zmieniamy stan przycisku Start/Stop
                button_StartStop.Text = "Started";
                button_StartStop.BackColor = Color.Green;
            }
            else
            {
                // Pauzujemy czas dla projektu i zaczynamy przerwę
                timer.Stop();
                isBreakRunning = true;

                // Zmieniamy tekst i kolor przycisku przerwy
                button_Break.Text = "Resume";
                button_Break.BackColor = Color.Green;

                // Zmieniamy stan przycisku Start/Stop
                button_StartStop.Text = "Paused";
                button_StartStop.BackColor = Color.Red;
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
            if (projects.Count == 0) return;

            currentProjectIndex = (currentProjectIndex - 1 + projects.Count) % projects.Count;
            UpdateCurrentProject();
        }

        private void button_Down_Click_Click(object sender, EventArgs e)
        {
            if (projects.Count == 0) return;

            currentProjectIndex = (currentProjectIndex + 1) % projects.Count;
            UpdateCurrentProject();
        }
    }
}
