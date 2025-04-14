using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using E_GYM_APP.Views.Pages;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows.Input;

namespace E_GYM_APP.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
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
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginUserAsync());
            RegisterCommand = new Command(async () => await RegisterUserAsync());
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async Task LoginUserAsync()
        {
            try
            {
                // Check if the Email or Password fields are empty
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Validation Error", "Email and Password cannot be empty.", "OK");
                    return;
                }

                IsBusy = true;

                var config = new FirebaseAuthConfig
                {
                    ApiKey = "AIzaSyBdkVYbW3Xcbyn7VkztUGW-S55uLWihTw4",
                    AuthDomain = "e-gym-b1a24.firebaseapp.com",
                    Providers = new FirebaseAuthProvider[]
                    {
                new EmailProvider()
                    }
                };

                var client = new FirebaseAuthClient(config);
                if (client == null)
                {
                    throw new NullReferenceException("FirebaseAuthClient initialization failed.");
                }

                var userCredential = await client.SignInWithEmailAndPasswordAsync(Email, Password);
                if (userCredential == null || userCredential.User == null)
                {
                    throw new NullReferenceException("User credential or user object is null.");
                }

                string token = await userCredential.User.GetIdTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    var content = JsonConvert.SerializeObject(userCredential.User);
                    Preferences.Set("FreshFirebaseToken", content);
                    string userId = userCredential.User.Uid;
                    Preferences.Set("UserId", userId);

                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    string text = "User Login successfully";
                    ToastDuration duration = ToastDuration.Short;
                    double fontSize = 14;
                    var toast = Toast.Make(text, duration, fontSize);
                    await toast.Show(cancellationTokenSource.Token);

                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    throw new Exception("Failed to retrieve ID token.");
                }
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Message.Contains("INVALID_LOGIN_CREDENTIALS"))
                {
                    await Application.Current.MainPage.DisplayAlert("Login Error", "Invalid login credentials. Please check your email and password.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Login Error", "Something Went Wrong", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Login Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task RegisterUserAsync()
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new SignUpPage());
        }
    }
}
