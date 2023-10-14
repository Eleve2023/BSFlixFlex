using System.Text.Json.Serialization;

namespace BSFlixFlex.Client.Shareds.Models
{   
    [JsonDerivedType(typeof(MovieDetails), typeDiscriminator: "MovieDetails")]
    [JsonDerivedType(typeof(Movie), typeDiscriminator: "Movie")]
    [JsonDerivedType(typeof(TvShowDetails), typeDiscriminator: "TvShowDetails")]
    [JsonDerivedType(typeof(TvShow), typeDiscriminator: "TvShow")]
    public interface IDiscovryCommonProperty
    {
        string? BackdropPath { get; set; }
        List<int>? GenreIds { get; set; }
        int? Id { get; set; }
        string? OriginalLanguage { get; set; }
        string? Overview { get; set; }
        double? Popularity { get; set; }
        string? PosterPath { get; set; }
        double? VoteAverage { get; set; }
        int? VoteCount { get; set; }
        string? Title { get; set; }
        string? OriginalTitle { get; set; }
    }
}