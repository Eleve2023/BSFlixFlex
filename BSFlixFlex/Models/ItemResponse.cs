
namespace BSFlixFlex.Models
{
    /// <summary>
    /// Classe générique pour encapsuler une réponse d'élément provenant d'une API.
    /// </summary>
    /// <typeparam name="T">Le type de l'élément dans la réponse.</typeparam>
    public class ApiItemResponse<T>
    {
        /// <summary>
        /// L'élément retourné par l'API.
        /// </summary>
        public T? Item { get; set; }

        /// <summary>
        /// Indique si la requête à l'API a réussi.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Un message d'erreur ou d'information retourné par l'API.
        /// </summary>
        public string? Message { get; set; }
    }
}
