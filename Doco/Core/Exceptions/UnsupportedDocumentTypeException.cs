namespace Doco.Core.Exceptions
{
    public class UnsupportedDocumentTypeException : Exception
    {
        public UnsupportedDocumentTypeException(string message) : base(message)
        {
        }
        public UnsupportedDocumentTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
