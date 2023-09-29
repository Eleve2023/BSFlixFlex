using System.Text.Json.Serialization;

namespace BSFlixFlex.Models
{
    public class TvShowResponse : IDiscoverResponse<TvShow>
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public ICollection<TvShow?> Results { get; set; } = new List<TvShow?>();

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }
}
