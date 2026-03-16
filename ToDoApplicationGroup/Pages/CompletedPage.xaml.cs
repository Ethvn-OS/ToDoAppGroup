using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplicationGroup;

public partial class CompletedPage : ContentPage
{
    private ObservableCollection<ToDoClass> todos = TaskStore.CompletedTasks;
    private ToDoClass selectedItem = null;
    public CompletedPage()
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

    private async void DeleteBtn_OnClicked(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int id = int.Parse(btn.ClassId);

        var itemToRemove = todos.FirstOrDefault(x => x.id == id);

        if (itemToRemove != null)
            todos.Remove(itemToRemove);
        await DisplayAlertAsync("Success", "Task deleted!", "OK!");
    }

    private async void ToDoLV_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is not ToDoClass tappedTask)
            return;

        await Navigation.PushAsync(new EditCompletePage(tappedTask));
        ToDoLV.SelectedItem = null;
    }

    private async void ToDoLV_OnItemTapped(object? sender, ItemTappedEventArgs e)
    {
        // if (e.Item is not ToDoClass tappedTask)
        //     return;
        //
        // await Navigation.PushAsync(new EditCompletePage(tappedTask));
        // ((ListView)sender).SelectedItem = null;
    }
}