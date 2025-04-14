using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_GYM_APP.Views.Classes;
using E_GYM_APP.Views.Pages;

namespace E_GYM_APP.ViewModels;

public partial class UpdateProfileViewModel : ObservableObject
{
    private readonly UserDatabase _UserDatabase;
    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    [ObservableProperty]
    private User _User;
    [ObservableProperty]
    private ImageSource _ProfilePictureSource;
    [RelayCommand]
    public async Task UpdateUserInfoAsync()
    {
        if (_UserDatabase == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "UserDatabase is not initialized.", "OK");
            return;
        }
        if (User == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "User is not loaded.", "OK");
            return;
        }    
        await _UserDatabase.SaveUserAsync(User);
        string updatedUserInfo = $"Username: {User.Username}\nEmail: {User.Email}\nWeight: {User.Weight}\nHeight: {User.Height}";
        var toast = Toast.Make("User Updated Successfully!", ToastDuration.Short, 14);
        await toast.Show(cancellationTokenSource.Token);
        await Shell.Current.GoToAsync("//myprofile");
    }
    public async Task InitializeAsync()
    {
        if (_UserDatabase == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "UserDatabase is not initialized.", "OK");
            return;
        }
        User = await _UserDatabase.GetUserByIdAsync();
        if (User != null)
        {
            LoadProfilePicture();
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to load user data.", "OK");
        }
    }
    [RelayCommand]
    public async Task PickAndSaveProfilePictureAsync()
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a profile picture"
            });
            if (result != null)
            {
                using (var stream = await result.OpenReadAsync())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        ProfilePictureSource = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));

                        if (User == null)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "User is not loaded.", "OK");
                            return;
                        }
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        var filePath = await SaveProfilePictureAsync(memoryStream, $"{User.Uid}_profile_picture_{timestamp}.png");
                        User.ProfilePicturePath = filePath;
                        await _UserDatabase.SaveUserAsync(User);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }
    private async Task<string> SaveProfilePictureAsync(Stream imageStream, string filename)
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, filename);
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await imageStream.CopyToAsync(fileStream);
        }
        return filePath;
    }
    public void LoadProfilePicture()
    {
        if (User == null)
        {
            ProfilePictureSource = "profile.png";
            return;
        }
        var filePath = User.ProfilePicturePath;
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
        {
            ProfilePictureSource = ImageSource.FromFile(filePath);
        }
        else
        {
            ProfilePictureSource = "account.png";
        }
    }
    public UpdateProfileViewModel(UserDatabase userDatabase)
    {
        _UserDatabase = userDatabase ?? throw new ArgumentNullException(nameof(userDatabase));
    }
    public async Task OnInitializedAsync()
    {
        await InitializeAsync();
    }
}
