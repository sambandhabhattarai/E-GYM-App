using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_GYM_APP.Views.Classes;
using E_GYM_APP.Views.Pages;
using Plugin.LocalNotification;
using System.Collections.ObjectModel;

namespace E_GYM_APP.ViewModels
{
    public partial class WorkoutShowViewModel : ObservableObject
    {
        private readonly WorkoutDatabase _WorkoutDatabase;
        [ObservableProperty]
        private ObservableCollection<Exercise> _Exercises;
        [ObservableProperty]
        private bool _IsWorkoutListEmpty;
        public IAsyncRelayCommand<Exercise> CompleteWorkoutCommand { get; }
        public WorkoutShowViewModel()
        {
            _WorkoutDatabase = new WorkoutDatabase();
            Exercises = new ObservableCollection<Exercise>();
            CompleteWorkoutCommand = new AsyncRelayCommand<Exercise>(CompleteWorkoutAsync);
            MessagingCenter.Subscribe<object, Exercise>(this, "ExerciseAdded", (sender, exercise) =>
            {
                if (exercise != null)
                {
                    Exercises.Add(exercise);
                }
            });
            MessagingCenter.Subscribe<object, Exercise>(this, "ExerciseDeleted", (sender, exercise) =>
            {
                var exerciseToRemove = Exercises.FirstOrDefault(e => e.Id == exercise.Id);
                if (exerciseToRemove != null)
                {
                    Exercises.Remove(exerciseToRemove);
                    UpdateWorkoutListStatus();
                }
            });
        }
        public async Task LoadExercisesAsync()
        {
            string userId = Preferences.Get("UserId", string.Empty);
            if (string.IsNullOrEmpty(userId))
            {
                await App.Current.MainPage.DisplayAlert("Error", "User is not authenticated.", "OK");
                return;
            }
            var storedExercises = await _WorkoutDatabase.GetExercisesAsync(userId);
            Exercises.Clear();
            foreach (var exercise in storedExercises.Where(e => !e.IsCompleted))
            {
                Exercises.Add(exercise);
            }
            UpdateWorkoutListStatus();
        }
        private async Task CompleteWorkoutAsync(Exercise exercise)
        {
            try
            {
                exercise.IsCompleted = !exercise.IsCompleted;
                exercise.CompletedDate = exercise.IsCompleted ? DateTime.Now : (DateTime?)null;
                await _WorkoutDatabase.UpdateExerciseAsync(exercise);
                if (exercise.IsCompleted)
                {
                    Exercises.Remove(exercise);
                }
                UpdateWorkoutListStatus();
                MessagingCenter.Send(this, "ExerciseStatusChanged", exercise);
                var notificationRequest = new NotificationRequest
                {
                    NotificationId = 1002,
                    Title = "Workout Completed!",
                    Description = "Great job! You've completed this workout for today.",
                    Schedule = { NotifyTime = exercise.CompletedDate }
                };

                LocalNotificationCenter.Current.Show(notificationRequest);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void UpdateExerciseInCollection(Exercise updatedExercise)
        {
            var existingExercise = Exercises.FirstOrDefault(e => e.Id == updatedExercise.Id);
            if (existingExercise != null)
            {
                var index = Exercises.IndexOf(existingExercise);
                Exercises[index] = updatedExercise;
            }
        }
        [RelayCommand]
        public async Task Start(Exercise selectedExercise)
        {
            if (selectedExercise != null)
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new TimerPage(selectedExercise));
            }
        }
        private void UpdateWorkoutListStatus()
        {
            IsWorkoutListEmpty = !Exercises.Any();
        }

    }
}
