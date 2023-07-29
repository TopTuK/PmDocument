namespace PmHelper.Domain.Models.Requests
{
    public class GenerateDocumentInfo
    {
        public int TypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
