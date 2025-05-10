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
    public class SettingsViewModel : BaseViewModel
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

        public ICommand SaveCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand GoBackCommand { get; }

        public SettingsViewModel() : base()
        {
            Title = "Einstellungen";

            SaveCommand = new Command(async () => await SaveSettingsAsync());
            LogoutCommand = new Command(async () => await LogoutAsync());
            GoBackCommand = new Command(async () => await NavigationService.GoBackAsync());
        }

        public override async Task InitializeAsync()
        {
            // Aktuelle Einstellungen laden
            ApiUrl = await SettingsService.GetSettingAsync(SettingsKeys.API_URL_KEY);
            ApiSecret = await SettingsService.GetSettingAsync(SettingsKeys.API_SECRET_KEY);
            UserName = await SettingsService.GetSettingAsync(SettingsKeys.USER_NAME_KEY);
        }

        private async Task SaveSettingsAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(ApiUrl) || string.IsNullOrEmpty(ApiSecret))
                {
                    ErrorMessage = "API-URL und API-Schlüssel sind erforderlich.";
                    return;
                }

                // Einstellungen speichern
                await SettingsService.SaveSettingAsync(SettingsKeys.API_URL_KEY, ApiUrl);
                await SettingsService.SaveSettingAsync(SettingsKeys.API_SECRET_KEY, ApiSecret);

                if (!string.IsNullOrEmpty(UserName))
                {
                    await SettingsService.SaveSettingAsync(SettingsKeys.USER_NAME_KEY, UserName);

                }

                // API neu initialisieren
                var success = await ApiService.InitializeAsync();

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Erfolg",
                        "Einstellungen wurden erfolgreich gespeichert.", "OK");

                    await NavigationService.GoBackAsync();
                }
                else
                {
                    ErrorMessage = "Die API konnte mit den neuen Einstellungen nicht initialisiert werden.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler beim Speichern der Einstellungen: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LogoutAsync()
        {
            if (IsBusy)
                return;

            var confirm = await Application.Current.MainPage.DisplayAlert(
                "Abmelden",
                "Möchtest du dich wirklich abmelden? Alle gespeicherten Einstellungen werden gelöscht.",
                "Ja", "Nein");

            if (!confirm)
                return;

            IsBusy = true;

            try
            {
                // Alle gespeicherten Einstellungen löschen
                await SettingsService.ClearSettingAsync(SettingsKeys.API_URL_KEY);
                await SettingsService.ClearSettingAsync(SettingsKeys.API_SECRET_KEY);
                await SettingsService.ClearSettingAsync(SettingsKeys.USER_NAME_KEY);

                // Zurück zur Login-Seite navigieren
                await NavigationService.NavigateToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler beim Abmelden: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
