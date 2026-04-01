using TodoList;

const string SaveFile = "tasks.json";
var manager = new TaskManager();
manager.LoadFromFile(SaveFile);
Console.WriteLine();

while (true)
{
    PrintLine();
    PrintColor(">> Todo List", ConsoleColor.Cyan);
    int todo = manager.Tasks.Count(task => !task.IsDone);
    int done = manager.Tasks.Count(task => task.IsDone);
    Console.WriteLine($">> You have {todo} tasks todo and {done} tasks are done!");
    PrintLine();

    Console.WriteLine(">> Pick an option:");
    Console.WriteLine(">>   (1) Show Todo List");
    Console.WriteLine(">>   (2) Add New Task");
    Console.WriteLine(">>   (3) Edit Todo List");
    Console.WriteLine(">>   (4) Save and Quit");
    PrintLine();
    Console.Write(">> ");

    var input = Console.ReadLine()?.Trim();

    switch (input)
    {
        case "1": ShowTodoList(); break;
        case "2": AddNewTask(); break;
        case "3": EditTodoList(); break;
        case "4":
            manager.SaveToFile(SaveFile);
            Console.Clear();
            PrintColor(">> Todo list saved. Goodbye!", ConsoleColor.Green);
            return;
        default:
            Console.Clear();
            PrintColor(">> Invalid option, try again.", ConsoleColor.Red);
            break;
    }
}

void ShowTodoList()
{
    Console.Clear();
    if (manager.Tasks.Count == 0)
    {
        PrintColor(">> No tasks found.", ConsoleColor.Yellow);
        return;
    }

    List<TodoList.Task> todoList = manager.SortByDate();
    Console.WriteLine();

    while (true)
    {
        PrintLine();
        PrintColor($"  {"STATUS",-12} {"DUE DATE",-15} {"PROJECT",-15} TITLE", ConsoleColor.Cyan);
        PrintLine();

        foreach (var task in todoList)
        {
            var status = task.IsDone ? "[DONE]" : task.IsOverdue ? "[OVERDUE]" : "[TODO]";
            Console.ForegroundColor = task.IsDone ? ConsoleColor.DarkGreen : task.IsOverdue ? ConsoleColor.DarkRed : ConsoleColor.White;
            Console.WriteLine($"  {status,-12} {task.DueDate,-15} {task.Project,-15} {task.Title}");
            Console.ResetColor();
        }

        PrintLine();
        Console.WriteLine(">> Pick an option:");
        Console.WriteLine(">>   (1) Sort by date");
        Console.WriteLine(">>   (2) Sort by project");
        Console.WriteLine(">>   (0) Back");
        PrintLine();
        Console.Write(">> ");


        var input = Console.ReadLine()?.Trim();
        Console.Clear();
        switch (input)
        {
            case "1":
                todoList = manager.SortByDate();
                Console.WriteLine();
                break;
            case "2":
                todoList = manager.SortByProject();
                Console.WriteLine();
                break;
            case "0":
                Console.WriteLine();
                return;
            default:
                PrintColor(">> Invalid option, try again.", ConsoleColor.Red);
                break;
        }
    }
}

void AddNewTask()
{
    Console.Clear();
    Console.WriteLine();
    PrintLine();
    PrintColor(">> Add New Task", ConsoleColor.Cyan);
    PrintLine();

    string title = GetRequiredInputStringWithPrompt(">> Title: ");
    string project = GetRequiredInputStringWithPrompt(">> Project: ");
    DateOnly dueDate = GetRequiredInputDateWithPrompt(">> Due date (yyyy-MM-dd): ");

    var task = new TodoList.Task
    {
        Title = title,
        Project = project,
        DueDate = dueDate,
        IsDone = false
    };

    manager.Add(task);
    Console.Clear();
    PrintColor($">> Task '{title}' added!", ConsoleColor.Green);
}

void EditTodoList()
{
    Console.Clear();
    if (manager.Tasks.Count == 0)
    {
        PrintColor(">> No tasks to edit.", ConsoleColor.Yellow);
        return;
    }
    Console.WriteLine();

    while (true)
    {
        var tasks = manager.SortByDate();

        PrintLine();
        PrintColor(">> Edit Todo List:", ConsoleColor.Cyan);
        PrintLine();

        Console.WriteLine(">> Pick a Task To Edit: ");
        for (int i = 0; i < tasks.Count; i++)
        {
            var task = tasks[i];
            var status = task.IsDone ? "[DONE]" : "[TODO]";
            Console.WriteLine($">>   ({i + 1}) {status} {task.DueDate} | {task.Project} | {task.Title}");
        }
        Console.WriteLine($">>   ({0}) Back");

        PrintLine();
        Console.Write(">> ");

        if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index > tasks.Count)
        {
            Console.Clear();
            PrintColor(">> Invalid option, try again.", ConsoleColor.Red);
            continue;
        }

        if (index == 0)
        {
            Console.Clear();
            Console.WriteLine();
            return;
        }

        EditTask(tasks[index - 1]);
        if (manager.Tasks.Count == 0)
        {
            return;
        }
    }
}

void EditTask(TodoList.Task task)
{
    Console.Clear();
    Console.WriteLine();

    while (true)
    {
        PrintLine();
        PrintColor($">> Edit Task:", ConsoleColor.Cyan);
        var status = task.IsDone ? "[DONE]" : "[TODO]";
        Console.WriteLine($">> {status} {task.DueDate} | {task.Project} | {task.Title}");
        PrintLine();
        Console.WriteLine(">> Pick an option:");
        Console.WriteLine(">>   (1) Update title");
        Console.WriteLine(">>   (2) Update due date");
        Console.WriteLine(">>   (3) Update project name");
        Console.WriteLine(">>   (4) Toggle task done");
        Console.WriteLine(">>   (5) Remove task");
        Console.WriteLine(">>   (0) Cancel");
        PrintLine();
        Console.Write(">> ");

        switch (Console.ReadLine()?.Trim())
        {
            case "1":
                task.Title = GetRequiredInputStringWithPrompt(">> New title: ");
                Console.Clear();
                PrintColor(">> Title updated!", ConsoleColor.Green);
                break;
            case "2":
                task.DueDate = GetRequiredInputDateWithPrompt(">> New due date (yyyy-MM-dd): ");
                Console.Clear();
                PrintColor(">> Due date updated!", ConsoleColor.Green);
                break;
            case "3":
                task.Project = GetRequiredInputStringWithPrompt(">> New project name: ");
                Console.Clear();
                PrintColor(">> Project name updated!", ConsoleColor.Green);
                break;
            case "4":
                Console.Clear();
                if (task.IsDone = !task.IsDone)
                {
                    PrintColor($">> '{task.Title}' marked as DONE!", ConsoleColor.Green);
                }
                else
                {
                    PrintColor($">> '{task.Title}' marked as TODO!", ConsoleColor.Red);
                }
                break;
            case "5":
                manager.Remove(task);
                if(manager.Tasks.Count == 0)
                {
                    Console.Clear();
                    PrintColor($">> '{task.Title}' removed. No more tasks left.", ConsoleColor.Red);
                    return;
                }
                Console.Clear();
                PrintColor($">> '{task.Title}' removed.", ConsoleColor.Red);
                return;
            case "0":
                Console.Clear();
                Console.WriteLine();
                return;
            default:
                Console.Clear();
                PrintColor(">> Invalid option, try again.", ConsoleColor.Red);
                break;
        }
    }
}


//HELPERS
void PrintLine() => Console.WriteLine(new string('-', Console.WindowWidth - 1));

void PrintColor(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}

string GetRequiredInputStringWithPrompt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(input)) return input;
        PrintColor(">> Cannot be empty, try again.", ConsoleColor.Red);
    }
}

DateOnly GetRequiredInputDateWithPrompt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (DateOnly.TryParse(Console.ReadLine(), out var date)) return date;
        PrintColor(">> Invalid date format. Use yyyy-MM-dd.", ConsoleColor.Red);
    }
}