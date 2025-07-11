using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace WorkTimeCounterWidget
{
    public partial class DetailsForm : Form
    {
        private const string FilePath = "projects.json";
        private List<Project> projects = new List<Project>();
        private TimeSpan infoLineTime = TimeSpan.Zero;
        private TimeSpan breakTime = TimeSpan.Zero;
        private readonly ProjectRepository projectRepository;
        public DetailsForm(ProjectRepository projectRepository)
        {
            InitializeComponent();
            this.projectRepository = projectRepository;
            this.FormClosing += new FormClosingEventHandler(DetailsForm_FormClosing);
        }
        private void button_AddProject_Click(object sender, EventArgs e)
        {
            string projectName = textBox_ProjectName.Text;

            if (string.IsNullOrEmpty(projectName))
            {
                MessageBox.Show("Please enter a project name.");
                return;
            }

            projectRepository.AddProject(projectName);
            textBox_ProjectName.Clear();
            UpdateProjectsList();
            UpdateProjectTimesList();
        }

        private void button_DeleteProject_Click(object sender, EventArgs e)
        {
            if (listBox_Projects.SelectedItem == null)
            {
                MessageBox.Show("Please select a project to delete.");
                return;
            }

            string projectName = listBox_Projects.SelectedItem.ToString();
            Project projectToDelete = projectRepository.Projects.FirstOrDefault(p => p.Name == projectName);

            if (projectToDelete != null)
            {
                projectRepository.RemoveProject(projectToDelete);
                UpdateProjectsList();
                UpdateProjectTimesList();
            }
        }

        public void ReceiveProjectsData(List<Project> projects, TimeSpan breakTime, TimeSpan infoLineTime)
        {
            this.breakTime = breakTime;
            this.infoLineTime = infoLineTime;

            UpdateProjectsList();
            UpdateProjectTimesList();
        }

        private void UpdateProjectsList()
        {
            listBox_Projects.Items.Clear();
            foreach (var project in projectRepository.Projects)
            {
                listBox_Projects.Items.Add(project.Name);
            }
        }

        private void UpdateProjectTimesList()
        {
            listBox_ProjectTimesList.Items.Clear();
            foreach (var project in projectRepository.Projects)
            {
                listBox_ProjectTimesList.Items.Add($"{project.Name} - {project.TimeSpent.ToString(@"hh\:mm\:ss")}");
            }
            listBox_ProjectTimesList.Items.Add($"Przerwa: {breakTime.ToString(@"hh\:mm\:ss")}");
            listBox_ProjectTimesList.Items.Add($"Infolinia WB: {infoLineTime.ToString(@"hh\:mm\:ss")}");

            UpdateTimeSumLabel(projectRepository.Projects, breakTime, infoLineTime);
        }

        private void DetailsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void UpdateTimeSumLabel(List<Project> projects, TimeSpan breakTime, TimeSpan infoLineTime)
        {

            TimeSpan totalProjectsTime = projects.Aggregate(TimeSpan.Zero, (sum, project) => sum + project.TimeSpent) + infoLineTime;
            TimeSpan totalWorkTime = totalProjectsTime + breakTime;

            double totalWorkHours = totalProjectsTime.TotalHours;
            TimeSpan calculatedBreakTime = TimeSpan.FromMinutes(totalWorkHours * 7);

            TimeSpan totalTimeWithBreaks = totalProjectsTime + calculatedBreakTime;


            string formattedTotalWorkTime = FormatTime(totalWorkTime);
            string formattedTotalTimeWithBreaks = FormatTime(totalTimeWithBreaks);


            label_TimeSum.Text = $"Deklarowana suma: {formattedTotalWorkTime}\nWyliczona suma: {formattedTotalTimeWithBreaks}";
        }

        private string FormatTime(TimeSpan time)
        {
            int hours = (int)time.TotalHours;
            int minutes = time.Minutes;
            return $"{hours}h {minutes}min";
        }

        private void button_CountTheDay_Click(object sender, EventArgs e)
        {
            SaveDayDataToFile();

            Environment.Exit(0);
        }

        private void SaveDayDataToFile()
        {
            string fileName = $"WorkTimes_{DateTime.Now:ddMMyyyy}.txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine($"Data utworzenia pliku: {DateTime.Now:dd.MM.yyyy}");
                writer.WriteLine();

                writer.WriteLine($"Data: {DateTime.Now:dd.MM.yyyy}");
                foreach (var project in projects)
                {
                    string formattedTime = FormatTime(project.TimeSpent);
                    double roundedTime = Math.Round(project.TimeSpent.TotalHours * 100 / 15) * 0.15;
                    writer.WriteLine($"Nazwa projektu: {formattedTime} ({roundedTime:F2})");
                }

                string formattedInfoLineTimeMinutes = $"{(int)infoLineTime.TotalMinutes}min";
                double roundedInfoLineTime = Math.Ceiling(infoLineTime.TotalMinutes / 15) * 0.15;
                writer.WriteLine($"Infolinia WB: {formattedInfoLineTimeMinutes} ({roundedInfoLineTime:F2}h)");

            }

            MessageBox.Show($"Dane zapisano do pliku: {fileName}", "Zapis zakoñczony", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_ApplyCorrectTime_Click(object sender, EventArgs e)
        {
            if (listBox_ProjectTimesList.SelectedItem == null)
            {
                MessageBox.Show("Wybierz projekt z listy.");
                return;
            }

            string selectedItem = listBox_ProjectTimesList.SelectedItem.ToString();
            string[] parts = selectedItem.Split('-');
            if (parts.Length < 1) return;

            string itemLabel = parts[0].Trim(); // Mo¿e byæ "Projekt A", "Przerwa", "Infolinia WB"

            if (!TimeSpan.TryParse(textBox_CorrectTime.Text, out TimeSpan newTime))
            {
                MessageBox.Show("Nieprawid³owy format czasu. U¿yj formatu hh:mm:ss");
                return;
            }

            // Obs³uga przerwy
            if (itemLabel.StartsWith("Przerwa", StringComparison.OrdinalIgnoreCase))
            {
                breakTime = newTime;
                textBox_CorrectTime.Clear();
                UpdateProjectTimesList();
                return;
            }

            // Obs³uga infolinii
            if (itemLabel.StartsWith("Infolinia", StringComparison.OrdinalIgnoreCase))
            {
                infoLineTime = newTime;
                textBox_CorrectTime.Clear();
                UpdateProjectTimesList();
                return;
            }

            // Standardowy projekt
            var project = projectRepository.Projects.FirstOrDefault(p => p.Name == itemLabel);
            if (project != null)
            {
                project.TimeSpent = newTime;
                textBox_CorrectTime.Clear();
                UpdateProjectTimesList();

                // Aktualizuj widget tylko, jeœli widaæ ten projekt
                if (Application.OpenForms["WidgetForm"] is WidgetForm widgetForm)
                {
                    widgetForm.UpdateCurrentProject();
                }
            }
        }

        private void listBox_ProjectTimesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = listBox_ProjectTimesList.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedItem))
                return;

            if (selectedItem.StartsWith("Przerwa", StringComparison.OrdinalIgnoreCase))
            {
                textBox_CorrectTime.Text = breakTime.ToString(@"hh\:mm\:ss");
                return;
            }

            if (selectedItem.StartsWith("Infolinia", StringComparison.OrdinalIgnoreCase))
            {
                textBox_CorrectTime.Text = infoLineTime.ToString(@"hh\:mm\:ss");
                return;
            }

            // Domyœlne — projekt
            string[] parts = selectedItem.Split('-');
            if (parts.Length > 1)
            {
                string timePart = parts[1].Trim();
                textBox_CorrectTime.Text = timePart;
            }
            else
            {
                textBox_CorrectTime.Text = "";
            }
        }
    }
    public class ProjectNameOnly
    {
        public string Name { get; set; }
    }

    public class Project
    {
        public string Name { get; set; }
        public TimeSpan TimeSpent { get; set; }

        public Project(string name)
        {
            Name = name;
            TimeSpent = TimeSpan.Zero;
        }
    }
}
