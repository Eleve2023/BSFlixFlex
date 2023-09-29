using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSFlixFlex.Models
{
    public class MovieDetails : DetailsCommonProperty
    {
        [JsonPropertyName("belongs_to_collection")]
        public object? BelongsToCollection { get; set; }

        [JsonPropertyName("budget")]
        public int? Budget { get; set; }

        [JsonPropertyName("imdb_id")]
        public string? ImdbId { get; set; }

        [JsonPropertyName("original_title")]
        public string? OriginalTitle { get; set; }

        [JsonPropertyName("production_countries")]
        public List<ProductionCountry?> ProductionCountries { get; set; } = new();

        [JsonPropertyName("release_date")]
        public DateTime ReleaseDate { get; set; }

        [JsonPropertyName("revenue")]
        public int? Revenue { get; set; }

        [JsonPropertyName("runtime")]
        public int? Runtime { get; set; }

        [JsonPropertyName("spoken_languages")]
        public List<SpokenLanguage?> SpokenLanguages { get; set; } = new();

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("video")]
        public bool? Video { get; set; }
    }
}
