using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace WorkTimeCounterWidget
{
    public partial class Form1 : Form
    {
        private const string FilePath = "projects.json";
        private List<Project> projects = new List<Project>();
        private WidgetForm widgetForm;

        public Form1()
        {
            InitializeComponent();
            LoadProjectsFromFile();
            widgetForm = new WidgetForm();
            widgetForm.Show();
        }

        private void SaveProjectsToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(projects);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving projects: {ex.Message}");
            }
        }

        private void LoadProjectsFromFile()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    projects = JsonSerializer.Deserialize<List<Project>>(json) ?? new List<Project>();
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
            }
        }
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
