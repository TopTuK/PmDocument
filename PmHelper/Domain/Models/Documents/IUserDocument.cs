namespace PmHelper.Domain.Models.Documents
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserDocument
    {
        int Id { get; }

        IDocumentType DocumentType { get; }

        string Title { get; }
        string Content { get; }

        DateTime Created { get; }
        DateTime LastModified { get; }
    }
}
