namespace BSFlixFlex.Exceptions.ApiTMBD
{
    public class HttpRequestApiTMBDException : ApiTMBDException
    {
        public HttpRequestApiTMBDException(string message) : base(message)
        {
        }

        public HttpRequestApiTMBDException(string message, HttpRequestError httpRequestError) : base(message)
        {
            HttpRequestError = httpRequestError;
        }

        public HttpRequestError HttpRequestError { get; }
    }
}
