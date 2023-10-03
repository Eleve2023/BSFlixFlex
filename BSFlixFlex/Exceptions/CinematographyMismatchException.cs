namespace BSFlixFlex.Exceptions
{
    public class CinematographyMismatchException : Exception
    {
        public CinematographyMismatchException()
            : base("La cinématographie ne correspond pas au type.")
        {
        }

        public CinematographyMismatchException(string message)
            : base(message)
        {
        }

        public CinematographyMismatchException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

}
