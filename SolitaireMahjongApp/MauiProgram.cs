using Microsoft.Extensions.Logging;
using SolitaireMahjongApp.Services;
using SolitaireMahjongApp.ViewModels;
using SolitaireMahjongApp.Views;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

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
                    fonts.AddFont("ZCOOLKuaiLe-Regular.ttf", "zCool");
                });

            builder.Services.AddSingleton<SessionService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
