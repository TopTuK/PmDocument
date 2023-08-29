using PmHelper.Domain.Models.Documents;

namespace PmHelper.Domain.Services.Documents
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task<IUserDocument?> GetUserDocumentAsync(int documentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<IDocument>> GetDocumentsByUserIdAsync(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="documentTypeId"></param>
        /// <param name="documentTitle"></param>
        /// <param name="requestText"></param>
        /// <returns></returns>
        Task<IDocument?> GenerateUserDocumentAsync(int userId, int documentTypeId, string documentTitle, string requestText);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IUserDocuments>> GetAllUserDocumentsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Task<IDocument?> RemoveUserDocumentAsync(int documentId);
    }
}
