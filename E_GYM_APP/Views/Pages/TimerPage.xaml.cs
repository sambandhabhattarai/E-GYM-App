using E_GYM_APP.ViewModels;
using E_GYM_APP.Views.Classes;

namespace E_GYM_APP.Views.Pages;

public partial class TimerPage : ContentPage
{
	public TimerPage(Exercise exercise)
	{
		InitializeComponent();
        BindingContext = new TimerViewModel(exercise);
    }
}