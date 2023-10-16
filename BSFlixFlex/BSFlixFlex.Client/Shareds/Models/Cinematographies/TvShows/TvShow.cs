using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSFlixFlex.Client.Shareds.Models.Cinematographies.TvShows
{
    public class TvShow : DiscovryCommonProperty
    {
        [JsonPropertyName("original_name")]
        public override string? OriginalTitle { get; set; }

        [JsonPropertyName("name")]
        public override string? Title { get; set; }

        [JsonPropertyName("origin_country")]
        public List<string>? OriginCountry { get; set; }

        [JsonPropertyName("first_air_date")]
        public string? FirstAirDate { get; set; }
    }


}
