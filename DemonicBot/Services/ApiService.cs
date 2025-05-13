using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using DemonicBot.Models;

namespace DemonicBot.Services
{
    public interface IApiService
    {
        Task<bool> InitializeAsync();
        Task<List<DiscordServer>> GetServersAsync();
        Task<List<DiscordChannel>> GetChannelsAsync(string serverId);
        Task<bool> SendEmbedAsync(string channelId, EmbedMessage embed);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ISettingsService _settingsService;

        private string _apiUrl;
        private string _apiSecret;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _settingsService = DependencyService.Get<ISettingsService>();
        }

        public async Task<bool> InitializeAsync()
        {
            try
            {
                // API-Einstellungen aus den gespeicherten Einstellungen abrufen
                _apiUrl = await _settingsService.GetSettingAsync(SettingsKeys.API_URL_KEY);
                _apiSecret = await _settingsService.GetSettingAsync(SettingsKeys.API_SECRET_KEY);

                // Fallback to defaults if settings are empty
                if (string.IsNullOrEmpty(_apiUrl))
                {
                    _apiUrl = SettingsKeys.DEFAULT_API_URL;
                }

                if (string.IsNullOrEmpty(_apiSecret))
                {
                    _apiSecret = SettingsKeys.DEFAULT_API_SECRET;
                }

                if (string.IsNullOrEmpty(_apiUrl) || string.IsNullOrEmpty(_apiSecret))
                {
                    return false;
                }

                // HTTP-Client konfigurieren
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-API-Secret", _apiSecret);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ApiService Initialisierung fehlgeschlagen: {ex.Message}");
                return false;
            }
        }

        public async Task<List<DiscordServer>> GetServersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/api/servers");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ServerResponse>();
                    return result?.Guilds ?? new List<DiscordServer>();
                }

                return new List<DiscordServer>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Abrufen der Server: {ex.Message}");
                return new List<DiscordServer>();
            }
        }

        public async Task<List<DiscordChannel>> GetChannelsAsync(string serverId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/api/channels/{serverId}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ChannelResponse>();
                    return result?.Channels ?? new List<DiscordChannel>();
                }

                return new List<DiscordChannel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Abrufen der Kanäle: {ex.Message}");
                return new List<DiscordChannel>();
            }
        }

        public async Task<bool> SendEmbedAsync(string channelId, EmbedMessage embed)
        {
            try
            {
                var payload = new SendEmbedRequest
                {
                    ChannelId = channelId,
                    EmbedData = embed
                };

                var jsonContent = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiUrl}/api/send-embed", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden des Embeds: {ex.Message}");
                return false;
            }
        }

        // Hilfsklassen für die API-Antworten
        private class ServerResponse
        {
            public List<DiscordServer> Guilds { get; set; }
        }

        private class ChannelResponse
        {
            public List<DiscordChannel> Channels { get; set; }
        }

        private class SendEmbedRequest
        {
            public string ChannelId { get; set; }
            public EmbedMessage EmbedData { get; set; }
        }
    }
}
