using E_GYM_APP.ViewModels;

namespace E_GYM_APP.Views.Pages;

public partial class SecondWelcomePage : ContentPage
{
    public SecondWelcomePage()
    {
        InitializeComponent();
        BindingContext = new SecondWelcomePageViewModel(Navigation);
    }
    private async void NextButton_Clicked(object sender, EventArgs e)
    {
        var viewModel = BindingContext as SecondWelcomePageViewModel;
        if (viewModel != null)
        {
            await viewModel.NavigateToRegisterPage();
        }
    }
}
