using E_GYM_APP.ViewModels;
using E_GYM_APP.Views.Classes;

namespace E_GYM_APP.Views.Pages;

public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as HomePageViewModel)?.UpdateBMI();
    }
}
