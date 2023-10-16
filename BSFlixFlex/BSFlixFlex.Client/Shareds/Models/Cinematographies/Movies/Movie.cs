using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSFlixFlex.Client.Shareds.Models.Cinematographies.Movies
{
    public class Movie : DiscovryCommonProperty
    {
        [JsonPropertyName("adult")]
        public bool? Adult { get; set; }

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("original_title")]
        public override string? OriginalTitle { get; set; }

        [JsonPropertyName("title")]
        public override string? Title { get; set; }

        [JsonPropertyName("video")]
        public bool? Video { get; set; }
    }
}