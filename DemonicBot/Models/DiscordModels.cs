using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace DemonicBot.Models
{
    public class DiscordServer
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("icon")]
        public string IconUrl { get; set; }

        public string DisplayName => Name;

        public string InitialLetter => !string.IsNullOrEmpty(Name)
            ? Name.Substring(0, 1).ToUpper()
            : "?";
    }

    public class DiscordChannel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parentId")]
        public string ParentId { get; set; }

        public string DisplayName => $"#{Name}";
    }

    public class EmbedMessage
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; } = "#5865F2"; // Discord Blurple

        [JsonPropertyName("author")]
        public EmbedAuthor Author { get; set; }

        [JsonPropertyName("fields")]
        public List<EmbedField> Fields { get; set; } = new List<EmbedField>();

        [JsonPropertyName("image")]
        public EmbedImage Image { get; set; }

        [JsonPropertyName("thumbnail")]
        public EmbedThumbnail Thumbnail { get; set; }

        [JsonPropertyName("footer")]
        public EmbedFooter Footer { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; } = DateTime.Now;
    }

    public class EmbedAuthor
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("iconURL")]
        public string IconUrl { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class EmbedField
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("inline")]
        public bool Inline { get; set; }
    }

    public class EmbedFooter
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("iconURL")]
        public string IconUrl { get; set; }
    }

    public class EmbedImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class EmbedThumbnail
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
