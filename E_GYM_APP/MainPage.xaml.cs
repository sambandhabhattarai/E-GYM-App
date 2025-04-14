using E_GYM_APP.Views.Pages;

namespace E_GYM_APP
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void Welcome_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FirstWelcomePage());
        }
    }
}
