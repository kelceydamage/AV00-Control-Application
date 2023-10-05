using Microsoft.Extensions.Logging;
using Transport.Client;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using AV00_Control_Application.View;
using AV00_Control_Application.ViewModel;
using AV00_Shared.Configuration;

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
            try
            {
                builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();
                builder.Services.AddSingleton<ITransportClient>();
                builder.Services.AddSingleton<ApplicationMainViewModel>();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
