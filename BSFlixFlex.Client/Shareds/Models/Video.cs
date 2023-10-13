using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSFlixFlex.Client.Shareds.Models
{
    public class Video
    {
        [JsonPropertyName("id")]
        public string? VideoId { get; set; }

        [JsonPropertyName("iso_639_1")]
        public string? LanguageCode { get; set; }

        [JsonPropertyName("iso_3166_1")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("key")]
        public string? VideoKey { get; set; }

        [JsonPropertyName("name")]
        public string? VideoName { get; set; }

        [JsonPropertyName("site")]
        public string? HostingSite { get; set; }

        [JsonPropertyName("size")]
        public int VideoQuality { get; set; }

        [JsonPropertyName("type")]
        public string? VideoType { get; set; }
    }
}
