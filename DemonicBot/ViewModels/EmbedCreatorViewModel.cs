using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows.Input;
using DemonicBot.Models;

namespace DemonicBot.ViewModels
{
    [QueryProperty(nameof(ChannelId), "ChannelId")]
    [QueryProperty(nameof(ChannelName), "ChannelName")]
    [QueryProperty(nameof(ServerId), "ServerId")]
    [QueryProperty(nameof(ServerName), "ServerName")]
    public class EmbedCreatorViewModel : BaseViewModel
    {
        private string _channelId;
        private string _channelName;
        private string _serverId;
        private string _serverName;

        private string _title;
        private string _description;
        private string _selectedColor;
        private string _imageUrl;
        private string _thumbnailUrl;
        private string _footerText;
        private bool _includeTimestamp;

        private ObservableCollection<EmbedField> _fields;
        private EmbedField _selectedField;

        private string _previewTitle;
        private string _previewDescription;

        // Navigationsparameter
        public string ChannelId
        {
            get => _channelId;
            set => SetProperty(ref _channelId, value);
        }

        public string ChannelName
        {
            get => _channelName;
            set => SetProperty(ref _channelName, value);
        }

        public string ServerId
        {
            get => _serverId;
            set => SetProperty(ref _serverId, value);
        }

        public string ServerName
        {
            get => _serverName;
            set => SetProperty(ref _serverName, value);
        }

        // Embed-Eigenschaften
        public string EmbedTitle
        {
            get => _title;
            set
            {
                if (SetProperty(ref _title, value))
                {
                    UpdatePreview();
                }
            }
        }

        public string EmbedDescription
        {
            get => _description;
            set
            {
                if (SetProperty(ref _description, value))
                {
                    UpdatePreview();
                }
            }
        }

        public string SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (SetProperty(ref _selectedColor, value))
                {
                    UpdatePreview();
                }
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (SetProperty(ref _imageUrl, value))
                {
                    UpdatePreview();
                }
            }
        }

        public string ThumbnailUrl
        {
            get => _thumbnailUrl;
            set
            {
                if (SetProperty(ref _thumbnailUrl, value))
                {
                    UpdatePreview();
                }
            }
        }

        public string FooterText
        {
            get => _footerText;
            set
            {
                if (SetProperty(ref _footerText, value))
                {
                    UpdatePreview();
                }
            }
        }

        public bool IncludeTimestamp
        {
            get => _includeTimestamp;
            set
            {
                if (SetProperty(ref _includeTimestamp, value))
                {
                    UpdatePreview();
                }
            }
        }

        // Felder
        public ObservableCollection<EmbedField> Fields
        {
            get => _fields;
            set => SetProperty(ref _fields, value);
        }

        public EmbedField SelectedField
        {
            get => _selectedField;
            set => SetProperty(ref _selectedField, value);
        }

        // Vorschau
        public string PreviewTitle
        {
            get => _previewTitle;
            set => SetProperty(ref _previewTitle, value);
        }

        public string PreviewDescription
        {
            get => _previewDescription;
            set => SetProperty(ref _previewDescription, value);
        }

        // Befehle
        public ICommand SendEmbedCommand { get; }
        public ICommand AddFieldCommand { get; }
        public ICommand RemoveFieldCommand { get; }
        public ICommand GoBackCommand { get; }

        // Verfügbare Farben
        public ObservableCollection<ColorOption> AvailableColors { get; }

        public EmbedCreatorViewModel() : base()
        {
            Title = "Embed erstellen";

            // Standardwerte
            SelectedColor = "#5865F2"; // Discord Blurple
            IncludeTimestamp = true;
            Fields = new ObservableCollection<EmbedField>();

            // Befehle
            SendEmbedCommand = new Command(async () => await SendEmbedAsync());
            AddFieldCommand = new Command(AddField);
            RemoveFieldCommand = new Command<EmbedField>(RemoveField);
            GoBackCommand = new Command(async () => await NavigationService.GoBackAsync());

            // Verfügbare Farben
            AvailableColors = new ObservableCollection<ColorOption>
            {
                new ColorOption { Name = "Blurple", Value = "#5865F2" },
                new ColorOption { Name = "Grün", Value = "#57F287" },
                new ColorOption { Name = "Gelb", Value = "#FEE75C" },
                new ColorOption { Name = "Rot", Value = "#ED4245" },
                new ColorOption { Name = "Grau", Value = "#95A5A6" },
                new ColorOption { Name = "Schwarz", Value = "#000000" },
                new ColorOption { Name = "Weiß", Value = "#FFFFFF" }
            };
        }

        public override Task InitializeAsync()
        {
            UpdatePreview();
            return Task.CompletedTask;
        }

        private void UpdatePreview()
        {
            PreviewTitle = string.IsNullOrEmpty(EmbedTitle) ? "Vorschau-Titel" : EmbedTitle;
            PreviewDescription = string.IsNullOrEmpty(EmbedDescription) ? "Vorschau-Beschreibung" : EmbedDescription;
        }

        private async Task SendEmbedAsync()
        {
            if (IsBusy)
                return;

            if (string.IsNullOrEmpty(EmbedTitle) || string.IsNullOrEmpty(EmbedDescription))
            {
                ErrorMessage = "Titel und Beschreibung sind erforderlich.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                // Embed-Nachricht erstellen
                var embed = new EmbedMessage
                {
                    Title = EmbedTitle,
                    Description = EmbedDescription,
                    Color = SelectedColor,
                    Fields = Fields.ToList(),
                    Timestamp = IncludeTimestamp ? DateTime.Now : null
                };

                // Optionale Felder hinzufügen
                if (!string.IsNullOrEmpty(ImageUrl))
                {
                    embed.Image = new EmbedImage { Url = ImageUrl };
                }

                if (!string.IsNullOrEmpty(ThumbnailUrl))
                {
                    embed.Thumbnail = new EmbedThumbnail { Url = ThumbnailUrl };
                }

                if (!string.IsNullOrEmpty(FooterText))
                {
                    embed.Footer = new EmbedFooter { Text = FooterText };
                }

                // Embed senden
                var success = await ApiService.SendEmbedAsync(ChannelId, embed);

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Erfolg",
                        "Embed-Nachricht wurde erfolgreich gesendet!", "OK");

                    // Zurück zur Kanalseite
                    await NavigationService.GoBackAsync();
                }
                else
                {
                    ErrorMessage = "Beim Senden der Embed-Nachricht ist ein Fehler aufgetreten.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void AddField()
        {
            var newField = new EmbedField
            {
                Name = "Neues Feld",
                Value = "Feldinhalt",
                Inline = false
            };

            Fields.Add(newField);
            SelectedField = newField;
        }

        private void RemoveField(EmbedField field)
        {
            if (field != null)
            {
                Fields.Remove(field);
            }
        }
    }

    public class ColorOption
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
