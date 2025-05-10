using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using DemonicBot.Views;
using DemonicBot.Services;

namespace DemonicBot.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _apiUrl;
        private string _apiSecret;
        private string _userName;

        public string ApiUrl
        {
            get => _apiUrl;
            set => SetProperty(ref _apiUrl, value);
        }

        public string ApiSecret
        {
            get => _apiSecret;
            set => SetProperty(ref _apiSecret, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel() : base()
        {
            Title = "Login";
            LoginCommand = new Command(async () => await LoginAsync());
        }

        public override async Task InitializeAsync()
        {
            // Überprüfen, ob bereits Anmeldedaten vorhanden sind
            var hasApiUrl = await SettingsService.HasSettingAsync(SettingsKeys.API_URL_KEY);
            var hasApiSecret = await SettingsService.HasSettingAsync(SettingsKeys.API_SECRET_KEY);

            if (hasApiUrl && hasApiSecret)
            {
                ApiUrl = await SettingsService.GetSettingAsync(SettingsKeys.API_URL_KEY);
                ApiSecret = await SettingsService.GetSettingAsync(SettingsKeys.API_SECRET_KEY);
                UserName = await SettingsService.GetSettingAsync(SettingsKeys.USER_NAME_KEY);

                // Optional: Automatisch anmelden, wenn Daten vorhanden sind
                await LoginAsync();
            }
        }

        private async Task LoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(ApiUrl) || string.IsNullOrEmpty(ApiSecret))
                {
                    ErrorMessage = "Bitte gib die API-URL und den API-Schlüssel ein.";
                    return;
                }

                // Einstellungen speichern
                await SettingsService.SaveSettingAsync(SettingsKeys.API_URL_KEY, ApiUrl);
                await SettingsService.SaveSettingAsync(SettingsKeys.API_SECRET_KEY, ApiSecret);

                if (!string.IsNullOrEmpty(UserName))
                {
                    await SettingsService.SaveSettingAsync(SettingsKeys.USER_NAME_KEY, UserName);
                }

                // API initialisieren
                var success = await ApiService.InitializeAsync();

                if (!success)
                {
                    ErrorMessage = "Die API konnte nicht initialisiert werden.";
                    return;
                }

                // Zur Serverseite navigieren
                await NavigationService.NavigateToAsync($"//{nameof(ServersPage)}");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler bei der Anmeldung: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
