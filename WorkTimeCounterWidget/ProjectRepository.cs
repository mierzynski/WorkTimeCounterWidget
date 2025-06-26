using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkTimeCounterWidget
{
    public class ProjectRepository
    {
        private readonly string filePath = "projects.json";

        public List<Project> Projects { get; private set; } = new();

        public event Action? ProjectsChanged;

        public void LoadProjects()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var projectNames = JsonSerializer.Deserialize<List<ProjectNameOnly>>(json) ?? new();
                Projects = projectNames.Select(p => new Project(p.Name)).ToList();
            }
            else
            {
                Projects = new();
                SaveProjects(); // utworzenie pustego pliku
            }

            ProjectsChanged?.Invoke();
        }

        public void SaveProjects()
        {
            var projectNames = Projects.Select(p => new ProjectNameOnly { Name = p.Name }).ToList();
            var json = JsonSerializer.Serialize(projectNames);
            File.WriteAllText(filePath, json);
            ProjectsChanged?.Invoke();
        }

        public void AddProject(string name)
        {
            Projects.Add(new Project(name));
            SaveProjects();
        }

        public void RemoveProject(Project project)
        {
            Projects.Remove(project);
            SaveProjects();
        }
    }

    //public class ProjectNameOnly
    //{
    //    public string Name { get; set; }
    //}

}
