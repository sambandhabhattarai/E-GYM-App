using E_GYM_APP.Views.Classes;

namespace E_GYM_APP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            string userId = Preferences.Get("UserId", string.Empty);

            if (!string.IsNullOrEmpty(userId))
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new MainPage();
            }
        }
        protected override void OnStart()
        {
            
            var notificationService = new NotificationService();
            notificationService.ScheduleDailyWorkoutNotification();
        }
    }
}
