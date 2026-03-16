using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplicationGroup;

public partial class EditCompletePage : ContentPage
{
    private readonly ToDoClass task;
    
    public EditCompletePage(ToDoClass taskToEdit)
    {
        InitializeComponent();
        
        task = taskToEdit;
        TitleEntry.Text = task.title;
        DetailsEditor.Text = task.detail;

        UpdateBtn.IsVisible = true;
        IncompleteBtn.IsVisible = true;
        DeleteBtn.IsVisible = true;
        CancelBtn.IsVisible = true;
    }

    private async void UpdateBtn_OnClicked(object? sender, EventArgs e)
    {
        task.title = TitleEntry.Text ?? "";
        task.detail = DetailsEditor.Text ?? "";
        await DisplayAlertAsync("Success", "Task updated!", "OK");
        await Navigation.PopAsync();
    }

    private async void IncompleteBtn_OnClickedBtn_OnClicked(object? sender, EventArgs e)
    {
        TaskStore.CompletedTasks.Remove(task);
        TaskStore.ToDoTasks.Add(task);
        await DisplayAlertAsync("Success", "Moved task to incomplete!", "OK!");
        await Navigation.PopAsync();
    }

    private async void CancelBtn_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void DeleteBtn_OnClicked(object? sender, EventArgs e)
    {
        TaskStore.CompletedTasks.Remove(task);
        await DisplayAlertAsync("Success", "Task deleted!", "OK!");
        await Navigation.PopAsync();
    }
}