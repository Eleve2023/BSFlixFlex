using System.Text.Json.Serialization;

namespace BSFlixFlex.Client.Shareds.Models.Cinematographies
{
    public class Genre
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
