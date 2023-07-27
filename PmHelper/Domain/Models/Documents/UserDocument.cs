using PmHelper.Domain.Repository.Entities;

namespace PmHelper.Domain.Models.Documents
{
    internal class UserDocument : IUserDocument
    {
        public int Id { get; init; }
        public IDocumentType DocumentType { get; init; }
        public string Title { get; init; }
        public string Content { get; init; }
        public DateTime Created { get; init; }
        public DateTime LastModified { get; init; }

        public UserDocument(DbUserDocument dbUserDocument)
        {
            Id = dbUserDocument.Id;

            Title = dbUserDocument.Title!;
            Content = dbUserDocument.Content!;

            Created = dbUserDocument.CreatedDate;
            LastModified = dbUserDocument.EditedDate;

            DocumentType = dbUserDocument.DocumentType != null
                ? new DocumentTypeImpl(dbUserDocument.DocumentType!)
                : DocumentTypeImpl.UnknownDocumentType();
        }
    }
}
