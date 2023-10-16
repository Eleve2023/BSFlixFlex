namespace BSFlixFlex.Client.Shareds.Models
{
    /// <summary>
    /// Classe générique pour encapsuler une réponse de liste provenant d'une API.
    /// </summary>
    /// <typeparam name="T">Le type d'éléments dans la liste.</typeparam>
    public class ApiListResponse<T>
    {
        /// <summary>
        /// La liste des éléments retournés par l'API.
        /// </summary>
        public List<T> Items { get; set; } = [];

        /// <summary>
        /// Le nombre total d'éléments disponibles.
        /// </summary>
        public int TotalItems { get; set; }

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
