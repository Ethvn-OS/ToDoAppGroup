using Microsoft.Maui.Storage;

namespace ToDoApplicationGroup;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        var firstName = Preferences.Default.Get("auth_fname", "");
        var lastName = Preferences.Default.Get("auth_lname", "");
        var fullName = $"{firstName} {lastName}".Trim();

        UserNameLabel.Text = string.IsNullOrWhiteSpace(fullName) ? "User" : fullName;
        EmailLabel.Text = Preferences.Default.Get("auth_email", "");
    }

    // Sign out — kept exactly as original
    private async void SignOutBtn_OnClicked(object? sender, EventArgs e)
    {
        Preferences.Default.Remove("auth_email");
        Preferences.Default.Remove("auth_fname");
        Preferences.Default.Remove("auth_lname");
        await DisplayAlertAsync("Signed Out", "You have been logged out.", "OK");
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