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

        private WidgetForm widgetForm;
        public DetailsForm()
        {
            InitializeComponent();
            LoadProjectsFromFile();
            this.FormClosing += new FormClosingEventHandler(DetailsForm_FormClosing);
        }
        public event Action<List<Project>> ProjectsUpdated;
        private void OnProjectsUpdated()
        {
            // Wywo³anie zdarzenia, jeœli ma subskrybentów
            ProjectsUpdated?.Invoke(projects);
        }

        private void DetailsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void SaveProjectsToFile()
        {
            try
            {
                var projectNames = projects.Select(p => new ProjectNameOnly { Name = p.Name }).ToList();

                var json = JsonSerializer.Serialize(projectNames);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving projects: {ex.Message}");
            }
        }

        public void LoadProjectsFromFile()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    var projectNames = JsonSerializer.Deserialize<List<ProjectNameOnly>>(json) ?? new List<ProjectNameOnly>();

                    projects = projectNames.Select(p => new Project(p.Name)).ToList();
                }
                else
                {
                    projects = new List<Project>();
                    SaveProjectsToFile();
                }

                listBox_Projects.Items.Clear();
                foreach (var project in projects)
                {
                    listBox_Projects.Items.Add(project.Name);
                }
                OnProjectsUpdated();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading projects: {ex.Message}");
            }
        }





        private void button_AddProject_Click(object sender, EventArgs e)
        {
            string projectName = textBox_ProjectName.Text;

            if (string.IsNullOrEmpty(projectName))
            {
                MessageBox.Show("Please enter a project name.");
                return;
            }

            Project newProject = new Project(projectName);
            projects.Add(newProject);
            listBox_Projects.Items.Add(projectName);
            textBox_ProjectName.Clear();

            SaveProjectsToFile();
            OnProjectsUpdated();
        }

        private void button_DeleteProject_Click(object sender, EventArgs e)
        {
            if (listBox_Projects.SelectedItem == null)
            {
                MessageBox.Show("Please select a project to delete.");
                return;
            }

            string projectName = listBox_Projects.SelectedItem.ToString();
            Project projectToDelete = projects.FirstOrDefault(p => p.Name == projectName);

            if (projectToDelete != null)
            {
                projects.Remove(projectToDelete);
                listBox_Projects.Items.Remove(projectName);
                SaveProjectsToFile();
                OnProjectsUpdated();
            }
        }

        public void ReceiveProjectsData(List<Project> projects, TimeSpan breakTime, TimeSpan infoLineTime)
        {
            listBox_ProjectTimesList.Items.Clear();
            this.projects = projects;
            this.breakTime = breakTime;
            this.infoLineTime = infoLineTime;

            foreach (var project in projects)
            {
                listBox_ProjectTimesList.Items.Add($"{project.Name} - {project.TimeSpent.ToString(@"hh\:mm\:ss")}");
            }
            listBox_ProjectTimesList.Items.Add($"Przerwa: {breakTime.ToString(@"hh\:mm\:ss")}");
            listBox_ProjectTimesList.Items.Add($"Infolinia WB: {infoLineTime.ToString(@"hh\:mm\:ss")}");

            UpdateTimeSumLabel(projects, breakTime, infoLineTime);
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
