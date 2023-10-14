namespace BSFlixFlex.Exceptions.ApiTMBD
{
    public class ApiTMBDException : Exception
    {
        public ApiTMBDException()
            : base("")
        {
        }

        public ApiTMBDException(string message)
            : base(message)
        {
        }

        public ApiTMBDException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    
}
