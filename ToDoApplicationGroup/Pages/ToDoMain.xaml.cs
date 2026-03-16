using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplicationGroup;

public partial class ToDoMain : ContentPage
{
    private ObservableCollection<ToDoClass> todos = TaskStore.ToDoTasks;
    private ToDoClass selectedItem = null;
    public ToDoMain()
    {
        InitializeComponent();
        ToDoLV.ItemsSource = todos;

        todos.CollectionChanged += (_, __) => UpdateEmptyState();
        UpdateEmptyState();
    }

    private void UpdateEmptyState()
    {
        bool hasTasks = todos.Count > 0;
        ToDoLV.IsVisible = hasTasks;
        EmptyStateLabel.IsVisible = !hasTasks;
    }

    private void ToDoLV_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            return;
        }
        
        selectedItem = (ToDoClass)e.SelectedItem;
        
        TitleEntry.Text = selectedItem.title;
        DetailsEditor.Text = selectedItem.detail;

        AddBtn.IsVisible = false;
        EditBtn.IsVisible = true;
        CancelBtn.IsVisible = true;
    }

    private async void AddBtn_OnClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
            return;

        todos.Add(new ToDoClass
        {
            id = todos.Count + 1,
            title = TitleEntry.Text,
            detail = DetailsEditor.Text
        });

        TitleEntry.Text = "";
        DetailsEditor.Text = "";

        await DisplayAlertAsync("Success", "Sucessfully added task!", "OK!");
    }

    private async void EditBtn_OnClicked(object? sender, EventArgs e)
    {
        if (selectedItem == null)
            return;

        selectedItem.title = TitleEntry.Text;
        selectedItem.detail = DetailsEditor.Text;

        ExitEditMode();

        await DisplayAlertAsync("Success", "Sucessfully edited task!", "OK!");
    }
    
    private void ExitEditMode()
    {
        selectedItem = null;

        TitleEntry.Text = "";
        DetailsEditor.Text = "";

        AddBtn.IsVisible = true;
        EditBtn.IsVisible = false;
        CancelBtn.IsVisible = false;

        ToDoLV.SelectedItem = null;
    }

    private void CancelBtn_OnClicked(object? sender, EventArgs e)
    {
        ExitEditMode();
    }

    private void ToDoLV_OnItemTapped(object? sender, ItemTappedEventArgs e)
    {
        ((ListView)sender).SelectedItem = null;
    }

    private async void DeleteBtn_OnClicked(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int id = int.Parse(btn.ClassId);

        var itemToRemove = todos.FirstOrDefault(x => x.id == id);

        if (itemToRemove != null)
            todos.Remove(itemToRemove);
        await DisplayAlertAsync("Success", "Task deleted!", "OK!");
    }

    private async void CompleteBtn_OnClicked(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int id = int.Parse(btn.ClassId);
        
        var item = todos.FirstOrDefault(x => x.id == id);
        if (item == null) return;

        todos.Remove(item);
        TaskStore.CompletedTasks.Add(item);
        
        if (selectedItem?.id == id)
            ExitEditMode();
        
        await DisplayAlertAsync("Success", "Task completed!", "OK!");
    }
}