using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoList
{
    public class TaskManager
    {
        public List<Task> Tasks { get; set; } = [];

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            Converters = { new DateOnlyJsonConverter() }
        };

        public List<Task> SortByDate() => [.. Tasks.OrderBy(task => task.DueDate).ThenBy(task => task.Project)];
        public List<Task> SortByProject() => [.. Tasks.OrderBy(task => task.Project).ThenBy(task => task.DueDate)];

        public void Add(Task task) => Tasks.Add(task);
        public void Remove(Task task) => Tasks.Remove(task);

        public void SaveToFile(string path)
        {
            var json = JsonSerializer.Serialize(Tasks, JsonOptions);
            File.WriteAllText(path, json);
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) return;
            var json = File.ReadAllText(path);
            Tasks = JsonSerializer.Deserialize<List<Task>>(json, JsonOptions) ?? [];
        }
    }

    // DateOnly is not supported by default in System.Text.Json, a converter is needed to serialize/deserialize it as a string in "yyyy-MM-dd" format.
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateOnly.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}