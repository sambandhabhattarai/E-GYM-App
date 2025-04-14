using E_GYM_APP.ViewModels;

namespace E_GYM_APP.Views.Pages;

public partial class WorkoutShowPage : ContentPage
{
    private WorkoutShowViewModel _WorkoutShowViewModel;
    public WorkoutShowPage()
	{
		InitializeComponent();
        _WorkoutShowViewModel = new WorkoutShowViewModel();
        BindingContext = _WorkoutShowViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _WorkoutShowViewModel.LoadExercisesAsync();
    }
}
