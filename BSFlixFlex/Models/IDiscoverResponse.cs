namespace BSFlixFlex.Models
{
    public interface IDiscoverResponse<T> where T : class
    {
        int Page { get; set; }
        ICollection<T?> Results { get; set; }
        int TotalPages { get; set; }
        int TotalResults { get; set; }
    }
}