using ToDoApplicationGroup.Models;
using ToDoApplicationGroup.Services;

namespace ToDoApplicationGroup;

public partial class ToDoMain : ContentPage
{
    public ToDoMain()
    {
        InitializeComponent();
        RefreshList();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshList();
    }

    private void RefreshList()
    {
        var active = TodoStore.Items.Where(t => !t.IsCompleted).ToList();
        TodoCollectionView.ItemsSource = null;
        TodoCollectionView.ItemsSource = active;
        EmptyLabel.IsVisible = active.Count == 0;
    }

    // ── Add ──────────────────────────────────────────────────────────────────
    private async void OnAddTapped(object sender, EventArgs e)
    {
        await ShowTodoDialog(null);
    }

    // ── Edit ─────────────────────────────────────────────────────────────────
    private async void OnEditTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is TodoItem item)
            await ShowTodoDialog(item);
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
    
    // ── Marking tasks as completed ──────────────────────────────────────────────
    private void OnCompleteTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is TodoItem item)
        {
            item.IsCompleted = true;
            RefreshList();
        }
    }

    // ── Shared Add / Edit Dialog ──────────────────────────────────────────────
    private async Task ShowTodoDialog(TodoItem? existing)
    {
        bool isEdit = existing != null;
        string heading = isEdit ? "✎ Edit Task" : "✦ New Task";

        var page = new ContentPage
        {
            BackgroundColor = Color.FromArgb("#FCF5EE"),
            Title = heading
        };

        var titleEntry = new Entry
        {
            Placeholder = "Task title…",
            Text = existing?.Title ?? "",
            FontSize = 15,
            TextColor = Color.FromArgb("#850E35"),
            PlaceholderColor = Color.FromArgb("#EE6983"),
            BackgroundColor = Colors.White
        };

        var detailsEditor = new Editor
        {
            Placeholder = "Details (optional)…",
            Text = existing?.Details ?? "",
            FontSize = 13,
            HeightRequest = 100,
            TextColor = Color.FromArgb("#850E35"),
            PlaceholderColor = Color.FromArgb("#EE6983"),
            BackgroundColor = Colors.White
        };

        var saveBtn = new Button
        {
            Text = isEdit ? "Save Changes ♡" : "Add Task ♡",
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

        Button? completeBtn = null;
        if (isEdit)
        {
            completeBtn = new Button
            {
                Text = "Mark as Done ✓",
                BackgroundColor = Color.FromArgb("#850E35"),
                TextColor = Colors.White,
                CornerRadius = 14
            };
        }

        var layout = new VerticalStackLayout
        {
            Padding = new Thickness(24),
            Spacing = 14,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Label
                {
                    Text = heading,
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

        if (completeBtn != null)
            layout.Children.Add(completeBtn);

        page.Content = new ScrollView { Content = layout };

        // Wire save
        saveBtn.Clicked += async (_, _) =>
        {
            var title = titleEntry.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(title))
            {
                await page.DisplayAlert("Oops!", "Please enter a title ♡", "OK");
                return;
            }

            if (isEdit && existing != null)
            {
                existing.Title = title;
                existing.Details = detailsEditor.Text?.Trim() ?? "";
            }
            else
            {
                TodoStore.Items.Add(new TodoItem
                {
                    Title = title,
                    Details = detailsEditor.Text?.Trim() ?? ""
                });
            }

            await Navigation.PopModalAsync(false);
            RefreshList();
        };

        // Wire mark complete
        if (completeBtn != null && existing != null)
        {
            completeBtn.Clicked += async (_, _) =>
            {
                existing.IsCompleted = true;
                await Navigation.PopModalAsync(false);
                RefreshList();
            };
        }

        await Navigation.PushModalAsync(new NavigationPage(page)
        {
            BarBackgroundColor = Color.FromArgb("#EE6983"),
            BarTextColor = Colors.White
        }, false);
    }
}