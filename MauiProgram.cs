using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using pcrespoT3.Services;
using pcrespoT3.Views;

namespace pcrespoT3
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

            builder.Services.AddSingleton<ContactoService>();
            builder.Services.AddTransient<vHome>();
            builder.Services.AddTransient<vResumen>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
