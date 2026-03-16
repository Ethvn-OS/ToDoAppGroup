using Microsoft.Maui.Storage;

namespace ToDoApplicationGroup;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void SignUpBtn_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PushModalAsync(new SignUpPage(), false);
    }

    private async void SignInBtn_OnClicked(object? sender, EventArgs e)
    {
        if (!Preferences.Default.ContainsKey("auth_email") || !Preferences.Default.ContainsKey("auth_password"))
        {
            await DisplayAlertAsync("No Account", "Please sign up first.", "OK");
            return;
        }
        
        var savedEmail = Preferences.Default.Get("auth_email", "");
        var savedPassword = Preferences.Default.Get("auth_password", "");

        var inputEmail = InputEmail.Text?.Trim().ToLowerInvariant() ?? "";
        var inputPassword = InputPassword.Text ?? "";

        if (inputEmail == savedEmail && inputPassword == savedPassword)
        {
            await DisplayAlertAsync("Welcome!", "You have successfully signed in.", "OK");
            // await Navigation.PushAsync(new NavigationPage(new AppShell()));
            Application.Current!.Windows[0].Page = new AppShell();
        }
        else
        {
            await DisplayAlertAsync("Login Failed!", "Invalid email or password.", "OK");
        }
    }
}