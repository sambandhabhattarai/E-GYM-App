using CommunityToolkit.Maui;
using E_GYM_APP.ViewModels;
using E_GYM_APP.Views.Classes;
using E_GYM_APP.Views.Pages;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace E_GYM_APP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            // Register UserDatabase as a singleton
            builder.Services.AddSingleton<UserDatabase>();

            // Register ViewModels
            builder.Services.AddTransient<HomePageViewModel>();

            // Register Pages
            builder.Services.AddTransient<HomePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
