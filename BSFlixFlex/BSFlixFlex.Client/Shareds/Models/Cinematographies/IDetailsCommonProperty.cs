using BSFlixFlex.Client.Shareds.Models.Cinematographies.Movies;
using BSFlixFlex.Client.Shareds.Models.Cinematographies.TvShows;
using System.Text.Json.Serialization;

namespace BSFlixFlex.Client.Shareds.Models.Cinematographies
{
    [JsonDerivedType(typeof(MovieDetails), typeDiscriminator: "MovieDetails")]    
    [JsonDerivedType(typeof(TvShowDetails), typeDiscriminator: "TvShowDetails")]    
    public interface IDetailsCommonProperty
    {
        bool? Adult { get; set; }
        string? BackdropPath { get; set; }
        List<Genre?> Genres { get; set; }
        string? Homepage { get; set; }
        int? Id { get; set; }
        string? OriginalLanguage { get; set; }
        string? Overview { get; set; }
        double? Popularity { get; set; }
        string? PosterPath { get; set; }
        List<ProductionCompany?> ProductionCompanies { get; set; }
        string? Status { get; set; }
        string? Tagline { get; set; }
        double? VoteAverage { get; set; }
        int? VoteCount { get; set; }
        List<ProductionCountry?> ProductionCountries { get; set; }
    }
}