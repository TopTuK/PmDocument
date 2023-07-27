namespace PmHelper.Domain.Models.Requests
{
    public class GenerateDocumentInfo
    {
        public int TypeId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
