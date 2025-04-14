using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_GYM_APP.Views.Pages;

namespace E_GYM_APP.ViewModels
{
    public partial class WorkoutViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _IsLoading;
        [RelayCommand]
        public async void ChestExercise()
        {
            IsLoading = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new ChestExercisesPage());
            IsLoading = false;
        }
        [RelayCommand]
        public async void BackExercise()
        {
            IsLoading = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new BackExercisesPage());
            IsLoading = false;
        }
        [RelayCommand]
        public async void CoreExercise()
        {
            IsLoading = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new CoreExercisesPage());
            IsLoading = false;
        }
        [RelayCommand]
        public async void ShoulderExercise()
        {
            IsLoading = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new ShoulderExercisesPage());
            IsLoading = false;
        }
        [RelayCommand]
        public async void BicepsExercise()
        {
            IsLoading = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new BicepsExercisesPage());
            IsLoading = false;
        }
    }
}
