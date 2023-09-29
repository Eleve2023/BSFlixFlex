using System.Text.Json.Serialization;

namespace BSFlixFlex.Models
{
    public class VideoResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("results")]
        public List<Video?> Results { get; set; } = new();
    }

}
