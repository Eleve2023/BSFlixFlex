namespace BSFlixFlex.Exceptions
{
    public class HttpRequest422Exception : Exception
    {
        public HttpRequest422Exception()
            : base("Response status code does not indicate success: 422 (Unknown).")
        {
        }

        public HttpRequest422Exception(string message)
            : base(message)
        {
        }

        public HttpRequest422Exception(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
