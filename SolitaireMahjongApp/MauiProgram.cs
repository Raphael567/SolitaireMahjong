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

#if DEBUG
            builder.Logging.AddDebug();
#endif
            //builder.Services.AddHttpClient<PlayerService>(client =>
            //{
            //    client.BaseAddress = new Uri("http://localhost:8080");
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //})
            //.ConfigurePrimaryHttpMessageHandler(() =>
            //{
            //    return new HttpClientHandler
            //    {
            //        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            //    };
            //});

            //builder.Services.AddTransient<PlayerViewModel>();
            //builder.Services.AddTransient<PlayerView>();

            return builder.Build();
        }
    }
}
