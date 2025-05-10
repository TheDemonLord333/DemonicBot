using DemonicBot.Services;
using DemonicBot.Views;


namespace DemonicBot;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Services registrieren
        DependencyService.Register<IApiService, ApiService>();
        DependencyService.Register<ISettingsService, SettingsService>();

        // NavigationService initialisieren
        var navigationService = new NavigationService();
        DependencyService.RegisterSingleton<INavigationService>(navigationService);

        // Startseite festlegen
        MainPage = navigationService.Initialize();
    }

    protected override void OnStart()
    {
        // App wird gestartet
    }

    protected override void OnSleep()
    {
        // App wird in den Hintergrund verschoben
    }

    protected override void OnResume()
    {
        // App kehrt in den Vordergrund zurück
    }
}