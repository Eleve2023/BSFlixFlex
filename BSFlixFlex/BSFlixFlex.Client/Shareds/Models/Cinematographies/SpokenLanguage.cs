using System.Text.Json.Serialization;

namespace BSFlixFlex.Client.Shareds.Models.Cinematographies
{
    public class SpokenLanguage
    {
        [JsonPropertyName("english_name")]
        public string EnglishName { get; set; }

        [JsonPropertyName("iso_639_1")]
        public string Iso639_1 { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
