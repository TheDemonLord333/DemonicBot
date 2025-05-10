using Microsoft.Extensions.Logging;
using DemonicBot.Converters;

namespace DemonicBot;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemiBold");
            });

        // Konverter für XAML hinzufügen
        builder.ConfigureMauiHandlers(handlers =>
        {
            // Hier können weitere Handler-Konfigurationen hinzugefügt werden
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}