using System.Collections.ObjectModel;

namespace ToDoApplicationGroup;

public static class TaskStore
{
    public static ObservableCollection<ToDoClass> ToDoTasks { get; } = new();
    public static ObservableCollection<ToDoClass> CompletedTasks { get; } = new();
}