using Microsoft.Extensions.Logging;
using Transport.Client;
using AV00_Control_Application.ViewModels;
using AV00_Shared.Configuration;
using AV00_Control_Application.Views;
using AV00_Control_Application.Models;

// Build trigger
namespace AV00_Control_Application
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
            builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
            builder.Services.AddSingleton<ITransportClient, TransportClient>();
            builder.Services.AddSingleton<ApplicationMainViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddDbContext<ApplicationDbContext>();

            var dbContext = new ApplicationDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
