using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_GYM_APP.Views.Classes;
using E_GYM_APP.Views.Pages;

namespace E_GYM_APP.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly UserDatabase _UserDatabase;

        [ObservableProperty]
        private User _User;
        [ObservableProperty]
        private double _BMI;
        [ObservableProperty]
        private string _Description;
        [ObservableProperty]
        private string _Icon;
        [ObservableProperty]
        private bool _ShowButton;
                
        public HomePageViewModel(UserDatabase userDatabase)
        {
            _UserDatabase = userDatabase;
            UpdateBMI();
            Icon = "darkmode.svg";
        }
        [RelayCommand]
        public async Task UpdateBMI()
        {
           await LoadUserProfile();
            if (User != null)
            {
                BMI = User.Weight/Math.Pow(User.Height,2);
               
                if (BMI > 0 && BMI < 18.5)
                {
                    Description = "You are underweight";
                }
                else if (BMI > 18.5 && BMI < 25)
                {
                    Description = "You have healthy weight";
                }
                else if (BMI > 25 && BMI < 30)
                {
                    Description = "You are overweight";
                }
                else if (double.IsNaN(BMI))
                {
                    Description = "Update Profile";

                }
                else
                {
                    Description = "You are obese";
                }
            }
        }
        [RelayCommand]
        public async Task Check()
        {
            await Shell.Current.GoToAsync("//myworkout");
        }
        [RelayCommand]
        public async Task LoadUserProfile()
        {
            User = await _UserDatabase.GetUserByIdAsync();
        }
        [RelayCommand]
        public async Task GoToExercise()
        {
            await Shell.Current.GoToAsync("//workout");
        }
        [RelayCommand]
        public void ChangeAppTheme()
        {
            if (Application.Current.RequestedTheme == AppTheme.Dark)
            {
                Application.Current.UserAppTheme = AppTheme.Light;
                Icon = "darkmode.svg";
            }
            else
            {
                Application.Current.UserAppTheme = AppTheme.Dark;
                Icon = "lightmode.svg";
            }
        }
        [RelayCommand]
        public async Task UpdateProfile()
        {
            await Shell.Current.GoToAsync("//myprofile");
        }
    }
}
