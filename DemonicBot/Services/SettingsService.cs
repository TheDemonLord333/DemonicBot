using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DemonicBot.Services
{
    public interface ISettingsService
    {
        Task<string> GetSettingAsync(string key);
        Task SaveSettingAsync(string key, string value);
        Task<bool> HasSettingAsync(string key);
        Task ClearSettingAsync(string key);
    }

    public class SettingsService : ISettingsService
    {
       
        public async Task<string> GetSettingAsync(string key)
        {
            try
            {
                if (await SecureStorage.Default.GetAsync(key) is string value)
                {
                    return value;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Abrufen der Einstellung '{key}': {ex.Message}");
                return string.Empty;
            }
        }

        public async Task SaveSettingAsync(string key, string value)
        {
            try
            {
                await SecureStorage.Default.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern der Einstellung '{key}': {ex.Message}");
            }
        }

        public async Task<bool> HasSettingAsync(string key)
        {
            try
            {
                return !string.IsNullOrEmpty(await SecureStorage.Default.GetAsync(key));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Überprüfen der Einstellung '{key}': {ex.Message}");
                return false;
            }
        }

        public async Task ClearSettingAsync(string key)
        {
            try
            {
                SecureStorage.Default.Remove(key);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Löschen der Einstellung '{key}': {ex.Message}");
            }
        }
    }
}