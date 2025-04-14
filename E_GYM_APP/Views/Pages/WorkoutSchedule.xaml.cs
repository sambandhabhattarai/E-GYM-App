using E_GYM_APP.ViewModels;

namespace E_GYM_APP.Views.Pages;

public partial class WorkoutSchedule : ContentPage
{
    private WorkoutScheduleViewModel _WorkoutScheduleViewModel;
    public WorkoutSchedule()
	{
		InitializeComponent();
        _WorkoutScheduleViewModel = new WorkoutScheduleViewModel();
        BindingContext = _WorkoutScheduleViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _WorkoutScheduleViewModel.LoadExercisesAsync();
    }
}
