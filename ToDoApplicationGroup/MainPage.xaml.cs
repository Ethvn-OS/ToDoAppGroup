using Microsoft.Maui.Storage;
using System.Text;
using Newtonsoft.Json;

namespace ToDoApplicationGroup;

public partial class MainPage : ContentPage
{
    private static readonly HttpClient HttpClient = new()
    {
        BaseAddress = new Uri("https://todo-list.dcism.org/")
    };
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
        var inputEmail = InputEmail.Text?.Trim().ToLowerInvariant() ?? "";
        var inputPassword = InputPassword.Text ?? "";

        if (string.IsNullOrWhiteSpace(inputEmail) || string.IsNullOrWhiteSpace(inputPassword))
        {
            await DisplayAlertAsync("Missing Info", "Please enter email and password.", "OK");
            return;
        }

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlertAsync("No Internet", "Please check your internet connection.", "OK");
            return;
        }
        
        SignInBtn.IsEnabled = false;

        try
        {
            var emailParam = Uri.EscapeDataString(inputEmail);
            var passParam = Uri.EscapeDataString(inputPassword);

            var route = $"signin_action.php?email={emailParam}&password={passParam}";
            using var response = await HttpClient.GetAsync(route);
            var responseText = await response.Content.ReadAsStringAsync();

            SignInApiResponse? api = null;

            try
            {
                api = JsonConvert.DeserializeObject<SignInApiResponse>(responseText);
            }
            catch
            {
                await DisplayAlertAsync("Server Error", "Invalid response from server.", "OK");
                return;
            }

            if (api?.status == 200 && api.data != null)
            {
                Preferences.Default.Set("auth_email", api.data.email ?? inputEmail);
                Preferences.Default.Set("auth_fname", api.data.fname ?? "");
                Preferences.Default.Set("auth_lname", api.data.lname ?? "");

                var fullName = $"{api.data.fname} {api.data.lname}".Trim();
                await DisplayAlertAsync("Welcome!", $"Signed in as {fullName}", "OK");

                Application.Current!.Windows[0].Page = new AppShell();
            }
            else
            {
                await DisplayAlertAsync("Login Failed", api?.message ?? "Account does not exist.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Request failed: {ex.Message}", "OK");
        }
        finally
        {
            SignInBtn.IsEnabled = true;
        }
    }

    private class SignInApiResponse
    {
        public int status { get; set; }
        public SignInData? data { get; set; }
        public string? message { get; set; }
    }

    private class SignInData
    {
        public int id { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? email { get; set; }
        public string? timemodified { get; set; }
    }
}