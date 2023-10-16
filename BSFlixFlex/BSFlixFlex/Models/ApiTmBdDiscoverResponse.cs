using BSFlixFlex.Client.Shareds.Models.Cinematographies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSFlixFlex.Models
{
    public class ApiTmBdDiscoverResponse<T> where T : class
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public ICollection<T> Results { get; set; } = new List<T>();

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }
}
