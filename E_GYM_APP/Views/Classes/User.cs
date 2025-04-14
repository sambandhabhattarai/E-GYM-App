using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace E_GYM_APP.Views.Classes
{
    public partial class User : ObservableObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ObservableProperty]
        private double _Weight;
        [ObservableProperty]
        private double _Height;
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePicturePath { get; set; }
    }
}
