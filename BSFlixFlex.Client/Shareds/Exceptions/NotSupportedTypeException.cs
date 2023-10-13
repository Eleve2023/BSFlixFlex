namespace BSFlixFlex.Client.Shareds.Exceptions
{
    public class NotSupportedTypeException : Exception
    {
        public NotSupportedTypeException()
            : base("Le type fourni n'est pas supporté.")
        {
        }

        public NotSupportedTypeException(string message)
            : base(message)
        {
        }

        public NotSupportedTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
