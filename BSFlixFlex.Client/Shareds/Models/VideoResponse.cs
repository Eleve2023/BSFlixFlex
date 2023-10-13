using System.Text.Json.Serialization;

namespace BSFlixFlex.Client.Shareds.Models
{
    public class VideoResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("results")]
        public List<Video?> Results { get; set; } = new();
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

}
