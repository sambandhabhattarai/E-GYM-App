using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_GYM_APP.Views.Classes;
using E_GYM_APP.Views.Pages;
using System.Collections.ObjectModel;

namespace E_GYM_APP.ViewModels
{
    public partial class BackExercisesViewModel : ObservableObject
    {
        private readonly WorkoutDatabase _WorkoutDatabase;
        [ObservableProperty]
        private ObservableCollection<Exercise> _Exercises;
        public IAsyncRelayCommand<Exercise> AddToWorkoutCommand { get; }
        public IAsyncRelayCommand ShowWorkoutCommand { get; }
        public BackExercisesViewModel()
        {
            _WorkoutDatabase = new WorkoutDatabase();
            Exercises = new ObservableCollection<Exercise>
            {
               new Exercise { ExerciseName = "Bent Over", ImageSource = "bent_over.gif", Sets="3-4", Time = "45-60 seconds per set" },
                new Exercise { ExerciseName = "Ladmine Row", ImageSource = "ladmine_row.gif", Sets = "3-4", Time = "45-60 seconds per set" },
                new Exercise { ExerciseName = "Lat Pulldown", ImageSource = "lat_pulldown.gif", Sets = "3-4", Time = "45-60 seconds per set" },
                new Exercise { ExerciseName = "Dumbell Row", ImageSource = "dumbell_row.gif", Sets = "3-4", Time = "45-60 seconds per set" }
            };
            AddToWorkoutCommand = new AsyncRelayCommand<Exercise>(AddToWorkoutAsync);
            ShowWorkoutCommand = new AsyncRelayCommand(ShowWorkoutAsync);
        }
        private async Task AddToWorkoutAsync(Exercise exercise)
        {
            try
            {
                string userId = Preferences.Get("UserId", string.Empty);

                if (string.IsNullOrEmpty(userId))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "User is not authenticated.", "OK");
                    return;
                }
                exercise.UserId = userId; 
                var existingExercise = await _WorkoutDatabase.GetExerciseByNameAndUserIdAsync(exercise.ExerciseName, userId);
                if (existingExercise != null)
                {
                    if (existingExercise.IsCompleted)
                    {
                        existingExercise.IsCompleted = false; 
                        await _WorkoutDatabase.UpdateExerciseAsync(existingExercise); 
                        MessagingCenter.Send(this, "ExerciseAdded", existingExercise);
                        await App.Current.MainPage.DisplayAlert("Success", $"{exercise.ExerciseName} re-added to workout!", "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Info", $"{exercise.ExerciseName} is already in your workout.", "OK");
                    }
                    return; 
                }
                await _WorkoutDatabase.SaveExerciseAsync(exercise);
                MessagingCenter.Send(this, "ExerciseAdded", exercise);
                await App.Current.MainPage.DisplayAlert("Success", $"{exercise.ExerciseName} added to workout!", "OK");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task ShowWorkoutAsync()
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new WorkoutShowPage());
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
