using Microsoft.Extensions.Logging;
using SolitaireMahjongApp.Services;
using SolitaireMahjongApp.ViewModels;
using System;
using System.Net.Http;

namespace SolitaireMahjongApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddHttpClient<PlayerService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:8080");
            });

            builder.Services.AddTransient<PlayerViewModel>();

            return builder.Build();
        }
    }
}
