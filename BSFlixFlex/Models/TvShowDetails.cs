using System.Text.Json.Serialization;

namespace BSFlixFlex.Models
{
    public class TvShowDetails : DetailsCommonProperty
    {

        [JsonPropertyName("created_by")]
        public List<TvShowCreator?> Creators { get; set; } = new();

        [JsonPropertyName("in_production")]
        public bool InProduction { get; set; }

        [JsonPropertyName("languages")]
        public List<string?> Languages { get; set; } = new();

        [JsonPropertyName("last_air_date")]
        public string? LastAirDate { get; set; }

        [JsonPropertyName("last_episode_to_air")]
        public TvShowEpisode? LastEpisodeToAir { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("episode_run_time")]
        public List<int?> EpisodeRunTime { get; set; } = new();

        [JsonPropertyName("first_air_date")]
        public DateOnly? FirstAirDate { get; set; }

        [JsonPropertyName("next_episode_to_air")]
        public TvShowEpisode? NextEpisodeToAir { get; set; }

        [JsonPropertyName("networks")]
        public List<TvShowNetwork?> Networks { get; set; } = new();

        [JsonPropertyName("number_of_episodes")]
        public int NumberOfEpisodes { get; set; }

        [JsonPropertyName("number_of_seasons")]
        public int NumberOfSeasons { get; set; }

        [JsonPropertyName("origin_country")]
        public List<string?> OriginCountry { get; set; } = new();

        [JsonPropertyName("original_name")]
        public string? OriginalName { get; set; }

        [JsonPropertyName("production_countries")]
        public List<ProductionCountry?> ProductionCountry { get; set; } = new();

        [JsonPropertyName("seasons")]
        public List<TvShowSeason?> Seasons { get; set; } = new();

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
