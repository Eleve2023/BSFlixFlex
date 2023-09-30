namespace BSFlixFlex.Models
{
    public class ListResponse<T>
    {
        public List<T>? Items { get; set; }
        public int TotalItems { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

    }
}
