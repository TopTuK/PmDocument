using PmHelper.Domain.Models.Users;

namespace PmHelper.Domain.Models.Documents
{
    public interface IUserDocuments
    {
        IUser User { get; }
        IReadOnlyList<IDocument> Documents { get; }
    }
}
