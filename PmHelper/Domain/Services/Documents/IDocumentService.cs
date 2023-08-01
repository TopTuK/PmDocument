using PmHelper.Domain.Models.Documents;

namespace PmHelper.Domain.Services.Documents
{
    public interface IDocumentService
    {
        Task<IEnumerable<IUserDocument>> GetUserDocumentsAsync(int userId);
        Task<IUserDocument?> GenerateUserDocumentAsync(int userId, int documentTypeId, string documentTitle, string requestText);
    }
}
