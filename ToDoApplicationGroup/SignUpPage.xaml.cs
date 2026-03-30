using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace ToDoApplicationGroup;

public partial class SignUpPage : ContentPage
{
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("https://todo-list.dcism.org/")
    };
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
        // var userName = UserNameEntry.Text?.Trim() ?? "";
        var firstName = FirstNameEntry.Text?.Trim() ?? "";
        var lastName = LastNameEntry.Text?.Trim() ?? "";
        var email = EmailEntry.Text?.Trim().ToLowerInvariant() ?? "";
        var password = PasswordEntry.Text ?? "";
        var confirmPassword = ConfirmPasswordEntry.Text ?? "";

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlertAsync("Missing Info", "Please fill in all fields.", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlertAsync("Password Mismatch", "Passwords do not match.", "OK");
            return;
        }
        
        // Preferences.Default.Set("auth_username", userName);
        // Preferences.Default.Set("auth_email",  email);
        // Preferences.Default.Set("auth_password", password);

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlertAsync("No Internet", "Please check your internet connection", "OK");
            return;
        }
        
        SignUpBtn.IsEnabled = false;

        try
        {
            var payload = new
            {
                first_name = firstName,
                last_name = lastName,
                email = email,
                password = password,
                confirm_password = confirmPassword
            };

            var json = JsonConvert.SerializeObject(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await HttpClient.PostAsync($"signup_action.php", content);
            var responseText = await response.Content.ReadAsStringAsync();

            var api = JsonConvert.DeserializeObject<SignUpApiResponse>(responseText);

            if (api?.status == 200)
            {
                await DisplayAlertAsync("Success", "Account created!", "OK");
                await Navigation.PopModalAsync(false);
            }
            else
            {
                await DisplayAlertAsync("Sign Up Failed", api?.message ?? "Unable to create account.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Request failed: {ex.Message}", "OK");
        }
        finally
        {
            SignUpBtn.IsEnabled = true;
        }
    }

    private class SignUpApiResponse
    {
        public int status {get; set;}
        public string? message { get; set; }
    }
}