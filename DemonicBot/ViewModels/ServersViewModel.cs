using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows.Input;
using DemonicBot.Models;
using DemonicBot.Views;

namespace DemonicBot.ViewModels
{
    public class ServersViewModel : BaseViewModel
    {
        private ObservableCollection<DiscordServer> _servers;
        private DiscordServer _selectedServer;

        public ObservableCollection<DiscordServer> Servers
        {
            get => _servers;
            set => SetProperty(ref _servers, value);
        }

        public DiscordServer SelectedServer
        {
            get => _selectedServer;
            set
            {
                if (SetProperty(ref _selectedServer, value) && value != null)
                {
                    SelectServer(value);
                }
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand GoToSettingsCommand { get; }

        public ServersViewModel() : base()
        {
            Title = "Deine Server";
            Servers = new ObservableCollection<DiscordServer>();

            RefreshCommand = new Command(async () => await LoadServersAsync());
            GoToSettingsCommand = new Command(async () => await NavigateToSettingsAsync());
        }

        public override async Task InitializeAsync()
        {
            await LoadServersAsync();
        }

        private async Task LoadServersAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                // API initialisieren, falls noch nicht geschehen
                var initialized = await ApiService.InitializeAsync();

                if (!initialized)
                {
                    ErrorMessage = "API nicht initialisiert. Bitte überprüfe deine Einstellungen.";
                    await NavigationService.NavigateToAsync($"//{nameof(LoginPage)}");
                    return;
                }

                // Server-Liste abrufen
                var servers = await ApiService.GetServersAsync();

                Servers.Clear();
                foreach (var server in servers)
                {
                    Servers.Add(server);
                }

                if (Servers.Count == 0)
                {
                    ErrorMessage = "Keine Server gefunden. Der Bot muss möglicherweise zu deinen Servern hinzugefügt werden.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler beim Laden der Server: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void SelectServer(DiscordServer server)
        {
            if (server == null)
                return;

            // Zur Kanalseite navigieren und Server-ID übergeben
            var parameters = new Dictionary<string, object>
            {
                { "ServerId", server.Id },
                { "ServerName", server.Name }
            };

            await NavigationService.NavigateToAsync($"{nameof(ChannelsPage)}", parameters);

            // Auswahl zurücksetzen
            SelectedServer = null;
        }

        private async Task NavigateToSettingsAsync()
        {
            await NavigationService.NavigateToAsync($"{nameof(SettingsPage)}");
        }
    }
}
