using E_GYM_APP.ViewModels;
using E_GYM_APP.Views.Classes;

namespace E_GYM_APP.Views.Pages;

public partial class UpdateProfilePage : ContentPage
{
    private readonly UpdateProfileViewModel _viewModel;
    public UpdateProfilePage(UserDatabase userDatabase)
    {
        InitializeComponent();
        _viewModel = new UpdateProfileViewModel(userDatabase);
        BindingContext = _viewModel;
        _ = _viewModel.OnInitializedAsync();
    }
}
