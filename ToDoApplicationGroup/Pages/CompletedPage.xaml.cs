using ToDoApplicationGroup.Services;

namespace ToDoApplicationGroup;

public partial class CompletedPage : ContentPage
{
    public CompletedPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var done = TodoStore.Items.Where(t => t.IsCompleted).ToList();
        CompletedCollectionView.ItemsSource = null;
        CompletedCollectionView.ItemsSource = done;
        EmptyLabel.IsVisible = done.Count == 0;
    }
}