namespace BSFlixFlex.Client.Shareds.Models
{
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
    }
}