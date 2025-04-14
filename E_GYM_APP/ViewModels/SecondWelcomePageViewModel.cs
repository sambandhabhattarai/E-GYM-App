using E_GYM_APP.Views.Pages;
using System.ComponentModel;

namespace E_GYM_APP.ViewModels
{
    public class SecondWelcomePageViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private readonly INavigation _navigation;
        public SecondWelcomePageViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
        public async Task NavigateToRegisterPage()
        {
            IsBusy = true;
            await Task.Delay(2000);
            await _navigation.PushModalAsync(new SignUpPage());
            IsBusy = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
