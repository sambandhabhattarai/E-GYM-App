namespace E_GYM_APP.Views.Pages;

public partial class FirstWelcomePage : ContentPage
{
	public FirstWelcomePage()
	{
		InitializeComponent();
	}
    private async void NextButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new SecondWelcomePage());
    }
}
