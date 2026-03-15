using Microsoft.Maui.Storage;

namespace ToDoApplicationGroup;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        UserNameLabel.Text = Preferences.Default.Get("auth_username", "");
        EmailLabel.Text = Preferences.Default.Get("auth_email", "");
    }

    // Sign out — kept exactly as original
    private void SignOutBtn_OnClicked(object? sender, EventArgs e)
    {
        Application.Current!.Windows[0].Page = new NavigationPage(new MainPage());
    }

    // Bottom nav — uses NavigationPage.PushAsync to match the rest of your app
    private async void OnToDoTabTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ToDoMain());
    }

    private async void OnCompletedTabTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CompletedPage());
    }

    private void OnProfileTabTapped(object sender, EventArgs e)
    {
        // Already on Profile — do nothing
    }
}