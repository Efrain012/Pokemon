/*

using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace API_Proyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly string _dataPath;

        public DataController(IWebHostEnvironment env)
        {
            _dataPath = Path.Combine(env.WebRootPath ?? "wwwroot", "data");
        }

        [HttpGet("projects")]
        public IActionResult GetProjects([FromQuery] string owner = null)
        {
            var filePath = Path.Combine(_dataPath, "projects.json");
            if (!System.IO.File.Exists(filePath)) return NotFound();

            var json = System.IO.File.ReadAllText(filePath);
            var projects = JsonSerializer.Deserialize<List<Project>>(json);

            if (!string.IsNullOrEmpty(owner))
            {
                projects = projects.Where(p => p.Owner == owner).ToList();
            }

            return Ok(projects);
        }

        private List<TaskItem> TopologicalSort(List<TaskItem> tasks)
        {
            var sorted = new List<TaskItem>();
            var visited = new HashSet<string>();

            void Visit(TaskItem task)
            {
                if (visited.Contains(task.Id)) return;
                visited.Add(task.Id);

                foreach (var dep in task.DependsOn)
                {
                    var depTask = tasks.FirstOrDefault(t => t.Id == dep);
                    if (depTask != null) Visit(depTask);
                }

                sorted.Add(task);
            }

            foreach (var task in tasks)
            {
                Visit(task);
            }

            return sorted;
        }


        [HttpGet("tasks")]
        public IActionResult GetTasks([FromQuery] string projectId = null)
        {
            var filePath = Path.Combine(_dataPath, "tasks.json");
            if (!System.IO.File.Exists(filePath)) return NotFound();
            Console.WriteLine($"Reading tasks from: {filePath}");

            var json = System.IO.File.ReadAllText(filePath);

            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (!string.IsNullOrEmpty(projectId))
            {
                tasks = tasks.Where(t => t.ProjectId == projectId).ToList();
            }

            // Sort tasks by dependencies (simple topological order)
            tasks = TopologicalSort(tasks);

            Console.WriteLine($"Total tasks loaded: {tasks.Count}");
            return Ok(tasks);
        }

        [HttpGet("task/{id}")]
        public IActionResult GetTaskById(string id)
        {
            var filePath = Path.Combine(_dataPath, "tasks.json");
            if (!System.IO.File.Exists(filePath)) return NotFound();
            Console.WriteLine($"Reading tasks from: {filePath}");

            var json = System.IO.File.ReadAllText(filePath);

            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null) return NotFound($"Task with ID {id} not found.");

            return Ok(task);
        }


    }

    // Minimal models
    public class Projects
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
    }


public class SingleOrArrayConverter : JsonConverter<List<string>>
    {
        public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new List<string> { reader.GetString() };
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                var list = new List<string>();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    list.Add(reader.GetString());
                }
                return list;
            }
            return new List<string>();
        }

        public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }


    public class TaskItem
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        [JsonConverter(typeof(SingleOrArrayConverter))]
        public List<string> DependsOn { get; set; } = new List<string>();
        public string Start { get; set; }      // YYYY-MM-DD
        public string Deadline { get; set; }   // YYYY-MM-DD
    }


}

*/