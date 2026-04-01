using System.Text.Json.Serialization;

namespace TodoList
{
    public class Task
    {
        public string Title { get; set; } = "";
        public DateOnly DueDate { get; set; }
        public bool IsDone { get; set; } = false;
        public string Project { get; set; } = "";

        [JsonIgnore]
        public bool IsOverdue => !IsDone && DueDate < DateOnly.FromDateTime(DateTime.Today);
    }
}