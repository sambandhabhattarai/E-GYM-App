using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace E_GYM_APP.Views.Classes
{
    public partial class Exercise : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ObservableProperty]
        public string exerciseName;
        [ObservableProperty]
        public string imageSource;
        [ObservableProperty]
        public string sets;
        [ObservableProperty]
        public string time;
        [ObservableProperty]
        public string userId;
        [ObservableProperty]
        public bool isCompleted;
        [ObservableProperty]
        public DateTime? completedDate;
    }
}

