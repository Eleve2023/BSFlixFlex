namespace BSFlixFlex.Models
{
    public class ItemResponse<T>
    {
        public T? Item { get; set; }        
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

    }
}
