namespace PmHelper.Domain.Models
{
    public class DocumentException : Exception
    {
        public DocumentException() { }
        public DocumentException(string message) : base(message) { }
        public DocumentException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
