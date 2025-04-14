using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_GYM_APP.Views.Classes;
using E_GYM_APP.Views.Pages;

namespace E_GYM_APP.ViewModels
{
    public partial class ProfilePageViewModel : ObservableObject
    {
        private readonly UserDatabase _UserDatabase;
        [ObservableProperty]
        private User _User;
        [ObservableProperty]
        private ImageSource _ProfilePictureSource;
        [ObservableProperty]
        private bool _IsLoading;
        public ProfilePageViewModel(UserDatabase userDatabase)
        {
            _UserDatabase = userDatabase;
            LoadUserProfile();
        }
        public async void LoadUserProfile()
        {
            User = await _UserDatabase.GetUserByIdAsync();
            LoadProfilePicture();
        }
        private void LoadProfilePicture()
        {
            if (User == null || string.IsNullOrEmpty(User.ProfilePicturePath) || !File.Exists(User.ProfilePicturePath))
            {
                ProfilePictureSource = "profile.png";
            }
            else
            {
                ProfilePictureSource = ImageSource.FromFile(User.ProfilePicturePath);
            }
        }
        [RelayCommand]
        public async void EditProfile()
        {
            IsLoading = true;
            try
            {
                var userDatabase = new UserDatabase();
                await App.Current.MainPage.Navigation.PushModalAsync(new UpdateProfilePage(userDatabase));
            }
            finally
            {
                IsLoading = false;
            }
        }
        [RelayCommand]
        public async void Logout()
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Confirm Logout", "Do you really want to logout?", "Yes", "No");
            if (answer)
            {
                Preferences.Remove("UserId");
                Preferences.Remove("FreshFirebaseToken");
                Application.Current.MainPage = new MainPage();
            }
        }
    }
}
