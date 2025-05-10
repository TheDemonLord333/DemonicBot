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
    [QueryProperty(nameof(ServerId), "ServerId")]
    [QueryProperty(nameof(ServerName), "ServerName")]
    public class ChannelsViewModel : BaseViewModel
    {
        private string _serverId;
        private string _serverName;
        private ObservableCollection<DiscordChannel> _channels;
        private DiscordChannel _selectedChannel;

        public string ServerId
        {
            get => _serverId;
            set
            {
                if (SetProperty(ref _serverId, value) && !string.IsNullOrEmpty(value))
                {
                    LoadChannelsAsync().ConfigureAwait(false);
                }
            }
        }

        public string ServerName
        {
            get => _serverName;
            set => SetProperty(ref _serverName, value);
        }

        public ObservableCollection<DiscordChannel> Channels
        {
            get => _channels;
            set => SetProperty(ref _channels, value);
        }

        public DiscordChannel SelectedChannel
        {
            get => _selectedChannel;
            set
            {
                if (SetProperty(ref _selectedChannel, value) && value != null)
                {
                    SelectChannel(value);
                }
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand GoBackCommand { get; }

        public ChannelsViewModel() : base()
        {
            Title = "Kanäle";
            Channels = new ObservableCollection<DiscordChannel>();

            RefreshCommand = new Command(async () => await LoadChannelsAsync());
            GoBackCommand = new Command(async () => await NavigationService.GoBackAsync());
        }

        private async Task LoadChannelsAsync()
        {
            if (IsBusy || string.IsNullOrEmpty(ServerId))
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;
            Title = $"Kanäle in {ServerName}";

            try
            {
                // Kanal-Liste abrufen
                var channels = await ApiService.GetChannelsAsync(ServerId);

                Channels.Clear();
                foreach (var channel in channels)
                {
                    Channels.Add(channel);
                }

                if (Channels.Count == 0)
                {
                    ErrorMessage = "Keine Textkanäle in diesem Server gefunden.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler beim Laden der Kanäle: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void SelectChannel(DiscordChannel channel)
        {
            if (channel == null)
                return;

            // Zur Embed-Creator-Seite navigieren und Kanal-ID übergeben
            var parameters = new Dictionary<string, object>
            {
                { "ChannelId", channel.Id },
                { "ChannelName", channel.Name },
                { "ServerId", ServerId },
                { "ServerName", ServerName }
            };

            await NavigationService.NavigateToAsync($"{nameof(EmbedCreatorPage)}", parameters);

            // Auswahl zurücksetzen
            SelectedChannel = null;
        }
    }
}
