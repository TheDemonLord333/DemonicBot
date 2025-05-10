using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DemonicBot.Views;


namespace DemonicBot.Services
{
    public interface INavigationService
    {
        Shell Initialize();
        Task NavigateToAsync(string route, Dictionary<string, object> parameters = null);
        Task GoBackAsync();
    }

    public class NavigationService : INavigationService
    {
        public Shell Initialize()
        {
            // Shell-Routen registrieren
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ServersPage), typeof(ServersPage));
            Routing.RegisterRoute(nameof(ChannelsPage), typeof(ChannelsPage));
            Routing.RegisterRoute(nameof(EmbedCreatorPage), typeof(EmbedCreatorPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));

            // AppShell erstellen und zurückgeben
            return new AppShell();
        }

        public async Task NavigateToAsync(string route, Dictionary<string, object> parameters = null)
        {
            try
            {
                if (parameters != null)
                {
                    await Shell.Current.GoToAsync(route, parameters);
                }
                else
                {
                    await Shell.Current.GoToAsync(route);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation zu {route} fehlgeschlagen: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Navigationsfehler",
                    $"Navigation zu {route} nicht möglich.", "OK");
            }
        }

        public async Task GoBackAsync()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Zurücknavigation fehlgeschlagen: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Navigationsfehler",
                    "Zurücknavigation nicht möglich.", "OK");
            }
        }
    }
}