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

        private WidgetForm widgetForm;
        public DetailsForm()
        {
            InitializeComponent();
            LoadProjectsFromFile();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }
        public event Action<List<Project>> ProjectsUpdated;
        private void OnProjectsUpdated()
        {
            // Wywo³anie zdarzenia, jeœli ma subskrybentów
            ProjectsUpdated?.Invoke(projects);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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
            // Wyczyœæ poprzednie dane
            listBox_ProjectTimesList.Items.Clear();

            // Dodaj projekty
            foreach (var project in projects)
            {
                listBox_ProjectTimesList.Items.Add($"{project.Name} - {project.TimeSpent.ToString(@"hh\:mm\:ss")}");
            }

            // Dodaj czas przerwy
            listBox_ProjectTimesList.Items.Add($"Przerwa: {breakTime.ToString(@"hh\:mm\:ss")}");

            // Dodaj czas infolinii
            listBox_ProjectTimesList.Items.Add($"Infolinia WB: {infoLineTime.ToString(@"hh\:mm\:ss")}");
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
