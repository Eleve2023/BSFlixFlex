using System.Text.Json.Serialization;

namespace BSFlixFlex.Client.Shareds.Models
{
    public class TvShowCreator
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("credit_id")]
        public string? CreditId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("gender")]
        public int Gender { get; set; }

        [JsonPropertyName("profile_path")]
        public string? ProfilePath { get; set; }
    }
}
