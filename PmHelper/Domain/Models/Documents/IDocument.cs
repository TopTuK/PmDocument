namespace PmHelper.Domain.Models.Documents
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDocument
    {
        int Id { get; }

        IDocumentType DocumentType { get; }

        string Title { get; }
        string RequestText { get; }
        string Content { get; }

        DateTime Created { get; }
        DateTime LastModified { get; }
    }
}
