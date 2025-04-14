using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_GYM_APP.Views.Classes;

namespace E_GYM_APP.ViewModels
{
    public partial class TimerViewModel : ObservableObject
    {
        [ObservableProperty]
        private Exercise _selectedExercise;
        [ObservableProperty]
        private string exerciseName;
        [ObservableProperty]
        private string exerciseImageSource;
        [ObservableProperty]
        private string timerDisplay = "00:00:00";
        [ObservableProperty]
        private bool isAnimationPlaying = false;
        private Stopwatch _stopwatch;
        private bool _isRunning;
        public TimerViewModel(Exercise exercise)
        {
            ExerciseName = exercise.exerciseName;
            ExerciseImageSource = exercise.imageSource;
            SelectedExercise = exercise;
            _stopwatch = new Stopwatch();
            _isRunning = false;
        }
        [RelayCommand]
        private void Start()
        {
            if (!_isRunning)
            {
                _stopwatch.Start();
                _isRunning = true;
                IsAnimationPlaying = true; 
                UpdateTimer();
            }
        }
        [RelayCommand]
        private void Pause()
        {
            if (_isRunning)
            {
                _stopwatch.Stop();
                _isRunning = false;
                IsAnimationPlaying = false; 
            }
        }
        [RelayCommand]
        private void Reset()
        {
            _stopwatch.Reset();
            _isRunning = false;
            TimerDisplay = "00:00:00";
            IsAnimationPlaying = false; 
        }
        private async void UpdateTimer()
        {
            while (_isRunning)
            {
                TimerDisplay = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
                await Task.Delay(1000);
            }
        }
    }
}
