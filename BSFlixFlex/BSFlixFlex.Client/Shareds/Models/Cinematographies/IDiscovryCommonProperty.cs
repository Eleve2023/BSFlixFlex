using System.Text.Json.Serialization;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.Movies;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.TvShows;

namespace BSFlixFlex.Client.Shareds.Models.Cinematographies
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