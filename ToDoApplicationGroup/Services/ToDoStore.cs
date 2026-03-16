using ToDoApplicationGroup.Models;

namespace ToDoApplicationGroup.Services;

public static class TodoStore
{
    public static List<TodoItem> Items { get; } = new();
}