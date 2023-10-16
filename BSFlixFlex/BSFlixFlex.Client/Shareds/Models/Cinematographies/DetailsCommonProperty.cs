using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSFlixFlex.Client.Shareds.Models.Cinematographies
{
    public abstract class DetailsCommonProperty : DiscovryCommonProperty, IDetailsCommonProperty
    {
        [JsonPropertyName("adult")]
        public bool? Adult { get; set; }

        [JsonPropertyName("genres")]
        public List<Genre?> Genres { get; set; } = new();

        [JsonPropertyName("homepage")]
        public string? Homepage { get; set; }

        [JsonPropertyName("production_companies")]
        public List<ProductionCompany?> ProductionCompanies { get; set; } = new();

        [JsonPropertyName("production_countries")]
        public List<ProductionCountry?> ProductionCountries { get; set; } = new();

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("tagline")]
        public string? Tagline { get; set; }


    }
}
