// using ToDoApplicationGroup.Services;
//
// namespace ToDoApplicationGroup;
//
// public partial class CompletedPage : ContentPage
// {
//     public CompletedPage()
//     {
//         InitializeComponent();
//     }
//
//     protected override void OnAppearing()
//     {
//         base.OnAppearing();
//         var done = TodoStore.Items.Where(t => t.IsCompleted).ToList();
//         CompletedCollectionView.ItemsSource = null;
//         CompletedCollectionView.ItemsSource = done;
//         EmptyLabel.IsVisible = done.Count == 0;
//     }
// }

using ToDoApplicationGroup.Models;
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
        RefreshList();
    }

    private void RefreshList()
    {
        var done = TodoStore.Items.Where(t => t.IsCompleted).ToList();
        CompletedCollectionView.ItemsSource = null;
        CompletedCollectionView.ItemsSource = done;
        EmptyLabel.IsVisible = done.Count == 0;
    }

    // ── Delete ────────────────────────────────────────────────────────────────
    private async void OnDeleteTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not TodoItem item) return;

        bool confirmed = await DisplayAlert("Delete Task",
            $"Remove \"{item.Title}\"?", "Yes, delete ✗", "Cancel");
        if (!confirmed) return;

        TodoStore.Items.Remove(item);
        RefreshList();
    }

    // ── Edit ─────────────────────────────────────────────────────────────────
    private async void OnEditTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is TodoItem item)
            await ShowEditDialog(item);
    }
    
    // ── Mark Incomplete ───────────────────────────────────────────────────────
    private void OnIncompleteTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not TodoItem item) return;

        item.IsCompleted = false;
        RefreshList();
    }

    // ── Edit Dialog ───────────────────────────────────────────────────────────
    private async Task ShowEditDialog(TodoItem existing)
    {
        var page = new ContentPage
        {
            BackgroundColor = Color.FromArgb("#FCF5EE"),
            Title = "✎ Edit Task"
        };

        var titleEntry = new Entry
        {
            Placeholder = "Task title…",
            Text = existing.Title,
            FontSize = 15,
            TextColor = Color.FromArgb("#850E35"),
            PlaceholderColor = Color.FromArgb("#EE6983"),
            BackgroundColor = Colors.White
        };

        var detailsEditor = new Editor
        {
            Placeholder = "Details (optional)…",
            Text = existing.Details,
            FontSize = 13,
            HeightRequest = 100,
            TextColor = Color.FromArgb("#850E35"),
            PlaceholderColor = Color.FromArgb("#EE6983"),
            BackgroundColor = Colors.White
        };

        var saveBtn = new Button
        {
            Text = "Save Changes ♡",
            BackgroundColor = Color.FromArgb("#EE6983"),
            TextColor = Colors.White,
            CornerRadius = 14,
            FontAttributes = FontAttributes.Bold
        };

        var cancelBtn = new Button
        {
            Text = "Cancel",
            BackgroundColor = Color.FromArgb("#FFC4C4"),
            TextColor = Colors.White,
            CornerRadius = 14
        };
        cancelBtn.Clicked += async (_, _) => await Navigation.PopModalAsync(false);

        var layout = new VerticalStackLayout
        {
            Padding = new Thickness(24),
            Spacing = 14,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Label
                {
                    Text = "✎ Edit Task",
                    FontSize = 22,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#850E35"),
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 0, 6)
                },
                new Frame
                {
                    BackgroundColor = Colors.White,
                    CornerRadius = 12,
                    HasShadow = false,
                    Padding = new Thickness(10, 4),
                    Content = titleEntry
                },
                new Frame
                {
                    BackgroundColor = Colors.White,
                    CornerRadius = 12,
                    HasShadow = false,
                    Padding = new Thickness(10, 4),
                    Content = detailsEditor
                },
                saveBtn,
                cancelBtn
            }
        };

        page.Content = new ScrollView { Content = layout };

        saveBtn.Clicked += async (_, _) =>
        {
            var title = titleEntry.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(title))
            {
                await page.DisplayAlert("Oops!", "Please enter a title ♡", "OK");
                return;
            }

            existing.Title = title;
            existing.Details = detailsEditor.Text?.Trim() ?? "";

            await Navigation.PopModalAsync(false);
            RefreshList();
        };

        await Navigation.PushModalAsync(new NavigationPage(page)
        {
            BarBackgroundColor = Color.FromArgb("#EE6983"),
            BarTextColor = Colors.White
        }, false);
    }
}