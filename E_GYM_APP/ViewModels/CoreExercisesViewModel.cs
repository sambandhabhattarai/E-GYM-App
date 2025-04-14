using CommunityToolkit.Mvvm.Input;
using E_GYM_APP.Views.Classes;
using E_GYM_APP.Views.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace E_GYM_APP.ViewModels
{
    public class CoreExercisesViewModel : INotifyPropertyChanged
    {
        private readonly WorkoutDatabase _WorkoutDatabase;
        private ObservableCollection<Exercise> _Exercises;
        public ObservableCollection<Exercise> Exercises
        {
            get => _Exercises;
            set
            {
                _Exercises = value;
                OnPropertyChanged(nameof(Exercises));
            }
        }
        public ICommand AddToWorkoutCommand { get; }
        public ICommand ShowWorkoutCommand { get; }
        public CoreExercisesViewModel()
        {
            _WorkoutDatabase = new WorkoutDatabase();
            Exercises = new ObservableCollection<Exercise>
            {
                new Exercise { ExerciseName = "Cable Crunch", ImageSource = "cable_crunch.gif", Sets = "3-4", Time = "45-60 seconds per set" },
                new Exercise { ExerciseName = "Crunch", ImageSource = "crunch.gif", Sets = "3-4", Time = "45-60 seconds per set" },
                new Exercise { ExerciseName = "Russian Twist", ImageSource = "russian_twist.gif", Sets = "3-4", Time = "45-60 seconds per set" },
                new Exercise { ExerciseName = "Side Plank", ImageSource = "side_plank.gif", Sets = "3-4", Time = "45-60 seconds per set" }
            };

            AddToWorkoutCommand = new RelayCommand<Exercise>(async (exercise) => await AddToWorkoutAsync(exercise));
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
