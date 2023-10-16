using BSFlixFlex.Client.Shareds.Models.Cinematographies;
using System.Text.Json.Serialization;

namespace BSFlixFlex.Models
{
    public class ApiTmBdVideoResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("results")]
        public List<Video?> Results { get; set; } = [];
    }
}
