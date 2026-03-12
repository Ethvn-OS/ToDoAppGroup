using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplicationGroup;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        UpdateUserNameLabel();
        UpdateEmailLabel();
    }

    private void UpdateUserNameLabel()
    {
        UserNameLabel.Text = Preferences.Default.Get("auth_username", "");
    }

    private void UpdateEmailLabel()
    {
        EmailLabel.Text =  Preferences.Default.Get("auth_email", "");
    }

    private void SignOutBtn_OnClicked(object? sender, EventArgs e)
    {
        Application.Current!.Windows[0].Page = new NavigationPage(new MainPage());
    }
}