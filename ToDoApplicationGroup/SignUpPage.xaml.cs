using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace ToDoApplicationGroup;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    private void SignInBtn_OnClicked(object? sender, EventArgs e)
    {
        Navigation.PopModalAsync(false);
    }

    private async void SignUpBtn_OnClicked(object? sender, EventArgs e)
    {
        var userName = UserNameEntry.Text?.Trim() ?? "";
        var email = EmailEntry.Text?.Trim().ToLowerInvariant() ?? "";
        var password = PasswordEntry.Text ?? "";
        var confirmPassword = ConfirmPasswordEntry.Text ?? "";

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlertAsync("Missing Info", "Please fill in all fields.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlertAsync("Password Mismatch", "Passwords do not match.", "OK");
            return;
        }
        
        Preferences.Default.Set("auth_username", userName);
        Preferences.Default.Set("auth_email",  email);
        Preferences.Default.Set("auth_password", password);

        await DisplayAlertAsync("Success", "Account created!", "OK");
        await Navigation.PopModalAsync(false);
    }
}