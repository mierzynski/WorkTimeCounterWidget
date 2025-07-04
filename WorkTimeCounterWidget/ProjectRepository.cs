using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkTimeCounterWidget
{
    public class ApplicationState
    {
        public List<ProjectNameOnly> Projects { get; set; } = new();
        public Point WidgetLocation { get; set; }
        public Size WidgetSize { get; set; }
    }
    public class ProjectRepository
    {
        private readonly string filePath = "projects.json";

        public List<Project> Projects { get; private set; } = new();
        public Point WidgetLocation { get; private set; }
        public Size WidgetSize { get; private set; }

        public event Action? ProjectsChanged;
        public event Action<Point>? LocationChanged;
        public event Action<Size>? SizeChanged;

        public void LoadProjects()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);

                    try
                    {
                        // Próba wczytania nowego formatu
                        var state = JsonSerializer.Deserialize<ApplicationState>(json);
                        if (state != null)
                        {
                            Projects = state.Projects.Select(p => new Project(p.Name)).ToList();
                            WidgetLocation = state.WidgetLocation;
                            WidgetSize = state.WidgetSize;
                        }
                    }
                    catch
                    {
                        // Jeśli nie udało się wczytać nowego formatu, próbujemy stary
                        var projectNames = JsonSerializer.Deserialize<List<ProjectNameOnly>>(json);
                        if (projectNames != null)
                        {
                            Projects = projectNames.Select(p => new Project(p.Name)).ToList();
                            WidgetLocation = new Point(100, 100); // domyślna pozycja
                            WidgetSize = new Size(310, 30); // domyślny rozmiar
                        }
                    }
                }
                else
                {
                    Projects = new();
                    WidgetLocation = new Point(100, 100);
                    WidgetSize = new Size(310, 30);
                    SaveProjects(); // utworzenie pustego pliku
                }

                ProjectsChanged?.Invoke();
                LocationChanged?.Invoke(WidgetLocation);
                SizeChanged?.Invoke(WidgetSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wczytywania projektów: {ex.Message}");
                Projects = new();
                WidgetLocation = new Point(100, 100);
                WidgetSize = new Size(310, 30);
            }
        }

        public void SaveProjects()
        {
            try
            {
                var state = new ApplicationState
                {
                    Projects = Projects.Select(p => new ProjectNameOnly { Name = p.Name }).ToList(),
                    WidgetLocation = WidgetLocation,
                    WidgetSize = WidgetSize
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(state, options);
                File.WriteAllText(filePath, json);
                ProjectsChanged?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania projektów: {ex.Message}");
            }
        }

        public void UpdateLocation(Point newLocation)
        {
            WidgetLocation = newLocation;
            SaveProjects();
            LocationChanged?.Invoke(WidgetLocation);
        }

        public void UpdateSize(Size newSize)
        {
            WidgetSize = newSize;
            SaveProjects();
            SizeChanged?.Invoke(WidgetSize);
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
}
