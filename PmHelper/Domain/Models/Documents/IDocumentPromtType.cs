namespace PmHelper.Domain.Models.Documents
{
    public enum DocumentFormatType: byte
    {
        Plain = 0,
        Markdown = 1,
    }

    public interface IDocumentPromtType : IDocumentType
    {
        string Promt { get; }
        string AssistantName { get; }
        DocumentFormatType FormatType { get; }
    }
}
