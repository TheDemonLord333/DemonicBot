using DemonicBot.Services;
using DemonicBot.Views;
using DemonicBot.Converters;

namespace DemonicBot;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Konverter im ResourceDictionary registrieren
        Application.Current.Resources.Add("StringNotEmptyToBoolConverter", new StringNotEmptyToBoolConverter());
        Application.Current.Resources.Add("StringEmptyToBoolConverter", new StringEmptyToBoolConverter());
        Application.Current.Resources.Add("InvertedBoolConverter", new InvertedBoolConverter());
        Application.Current.Resources.Add("CollectionCountToHeightConverter", new CollectionCountToHeightConverter());

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