using E_GYM_APP.ViewModels;
using E_GYM_APP.Views.Classes;

namespace E_GYM_APP.Views.Pages;

public partial class Profile : ContentPage
{
    public Profile()
    {
        InitializeComponent();
        UserDatabase userDatabase = new UserDatabase();
        BindingContext = new ProfilePageViewModel(userDatabase);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ProfilePageViewModel)?.LoadUserProfile();
    }
}
