using PmHelper.Domain.Repository.Entities;

namespace PmHelper.Domain.Models.Documents
{
    internal class DocumentTypeImpl : IDocumentType
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public DocumentTypeImpl(DbDocumentType dbDocumentType)
        {
            Id = dbDocumentType.Id;

            Name = dbDocumentType.Name!;
        }

        private DocumentTypeImpl()
        {
            Id = -1;
            Name = "Unknown type";
        }

        public static IDocumentType UnknownDocumentType() => new DocumentTypeImpl();
    }

    internal class DocumentPromtType : DocumentTypeImpl, IDocumentPromtType
    {
        public string Promt { get; init; }
        public string AssistantName { get; init; }
        public DocumentFormatType FormatType { get; init; }

        public DocumentPromtType(DbDocumentType dbDocumentType) : base(dbDocumentType)
        {
            Promt = dbDocumentType.Prompt!;
            AssistantName = dbDocumentType.AssistantName!;
            FormatType = (DocumentFormatType) dbDocumentType.ResultFormat;
        }
    }
}
