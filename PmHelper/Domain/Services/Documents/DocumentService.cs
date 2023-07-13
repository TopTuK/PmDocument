using Microsoft.EntityFrameworkCore;
using PmHelper.Domain.Models.Documents;
using PmHelper.Domain.Repository;

namespace PmHelper.Domain.Services.Documents
{
    internal class DocumentService : IDocumentService
    {
        private readonly ILogger<IDocumentService> _logger;
        private readonly AppDbContext _dbContext;

        public DocumentService(AppDbContext dbContext, ILogger<IDocumentService> logger)
        {
            _dbContext = dbContext;

            _logger = logger;
        }

        public async Task<IEnumerable<IUserDocument>> GetUserDocumentsAsync(int userId)
        {
            _logger.LogInformation(
                "DocumentService::GetUserDocumentsAsync: start get documents for used {}",
                userId
            );

            var userDocuments = await _dbContext.UserDocuments
                .Where(dbDoc => dbDoc.UserId == userId)
                .Select(dbDoc => new UserDocument(dbDoc))
                .ToListAsync();

            _logger.LogInformation(
                "DocumentService::GetUserDocumentsAsync: got {} for user {}",
                userDocuments.Count, userId
            );
            return userDocuments;
        }
    }
}
