namespace BSFlixFlex.Exceptions
{
    public class PageSizeMismatchException : Exception
    {
        public PageSizeMismatchException()
            : base("La taille de page demandée par le client est supérieure à la taille de page de l'API.")
        {
        }

        public PageSizeMismatchException(string message)
            : base(message)
        {
        }

        public PageSizeMismatchException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
