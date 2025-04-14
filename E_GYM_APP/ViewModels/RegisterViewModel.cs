using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using E_GYM_APP.Views.Classes;
using E_GYM_APP.Views.Pages;
using Firebase.Auth;
using Firebase.Auth.Providers;
using System.ComponentModel;
using System.Windows.Input;

namespace E_GYM_APP.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly UserDatabase _UserDatabase;
        private string _Username;
        private string _Email;
        private string _Password;
        private bool _isBusy;
        public string Email
        {
            get => _Email;
            set
            {
                _Email = value;
                RaisePropertyChanged(nameof(Email));
            }
        }
        public string Password
        {
            get => _Password;
            set
            {
                _Password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }
        public string Username
        {
            get => _Username;
            set
            {
                _Username = value;
                RaisePropertyChanged(nameof(Username));
            }
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }
        public ICommand SignUpCommand { get; }
        public ICommand LoginCommand { get; }
        public RegisterViewModel()
        {
            _UserDatabase = new UserDatabase();
            SignUpCommand = new Command(async () => await RegisterUserAsync());
            LoginCommand = new Command(async () => await LoginUserAsync());
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async Task RegisterUserAsync()
        {
            try
            {
                IsBusy = true;
                var config = new FirebaseAuthConfig
                {
                    ApiKey = "AIzaSyBdkVYbW3Xcbyn7VkztUGW-S55uLWihTw4",
                    AuthDomain = "e-gym-b1a24.firebaseapp.com", 
                    Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider()  
                    },
                };
                var client = new FirebaseAuthClient(config);
                var userCredential = await client.CreateUserWithEmailAndPasswordAsync(Email, Password,Username);
                string token = await userCredential.User.GetIdTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    string text = "User Registered successfully";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 14;
                    var toast = Toast.Make(text, duration, fontSize);
                    await toast.Show(cancellationTokenSource.Token);
                    E_GYM_APP.Views.Classes.User dbUser = new E_GYM_APP.Views.Classes.User
                    {
                        Uid = userCredential.User.Info.Uid,
                        Email = userCredential.User.Info.Email,
                        Username = userCredential.User.Info.DisplayName
                    };
                    await _UserDatabase.SaveUserAsync(dbUser);
                    await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("EMAIL_EXISTS"))
                {
                    await Application.Current.MainPage.DisplayAlert("Registration Error", "The email is already registered", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Something Went Wrong", "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task LoginUserAsync()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
        }
    }
}
