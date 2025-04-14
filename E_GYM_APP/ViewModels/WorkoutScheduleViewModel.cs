using CommunityToolkit.Mvvm.ComponentModel;
using E_GYM_APP.Views.Classes;
using Plugin.Maui.Calendar.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace E_GYM_APP.ViewModels
{
    public class WorkoutScheduleViewModel : INotifyPropertyChanged
    {
        public EventCollection Events { get; set; } = new EventCollection();
        private readonly WorkoutDatabase _WorkoutDatabase;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public WorkoutScheduleViewModel()
        {
            _WorkoutDatabase = new WorkoutDatabase();
            LoadExercisesAsync().ConfigureAwait(false);
            MessagingCenter.Subscribe<object, Exercise>(this, "ExerciseStatusChanged", (sender, exercise) =>
            {
                UpdateExerciseStatus(exercise);
            });

            MessagingCenter.Subscribe<object, Exercise>(this, "ExerciseAdded", (sender, exercise) =>
            {
                AddOrUpdateEvent(exercise);
                OnPropertyChanged(nameof(Events));
            });
        }
        public async Task LoadExercisesAsync()
        {
            IsLoading = true;
            try
            {
                string userId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(userId))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "User is not authenticated.", "OK");
                    return;
                }
                var exercises = await _WorkoutDatabase.GetExercisesAsync(userId);
                Events.Clear();
                foreach (var exercise in exercises)
                {
                    AddOrUpdateEvent(exercise);
                }
                OnPropertyChanged(nameof(Events));
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
        private void AddOrUpdateEvent(Exercise exercise)
        {
            try
            {
                var eventDate = exercise.CompletedDate?.Date ?? DateTime.Now.Date;
                var eventModel = new EventModel
                {
                    Name = exercise.ExerciseName,
                    Status = exercise.CompletedDate.HasValue ? "Complete" : "Incomplete"
                };
                if (!Events.ContainsKey(eventDate))
                {
                    Events[eventDate] = new ObservableCollection<EventModel>();
                }
                var eventList = Events[eventDate] as ObservableCollection<EventModel>;
                var existingEvent = eventList.FirstOrDefault(e => e.Name == exercise.ExerciseName);
                if (existingEvent != null)
                {
                    existingEvent.Status = eventModel.Status;
                }
                else
                {
                    eventList?.Add(eventModel);
                }
                OnPropertyChanged(nameof(Events));
            }
            finally
            {
                IsLoading = false;
            }
        }
        private void UpdateExerciseStatus(Exercise exercise)
        {
            var eventDate = exercise.CompletedDate?.Date ?? DateTime.Now.Date;

            if (Events.ContainsKey(eventDate))
            {
                var eventList = Events[eventDate] as ObservableCollection<EventModel>;
                var eventModel = eventList?.FirstOrDefault(e => e.Name == exercise.ExerciseName);
                if (eventModel != null)
                {
                    eventModel.Status = exercise.CompletedDate.HasValue ? "Complete" : "Incomplete";
                }
            }
            else
            {
                AddOrUpdateEvent(exercise);
            }
            OnPropertyChanged(nameof(Events));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
