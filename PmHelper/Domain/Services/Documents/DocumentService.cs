using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetChatGptCLient.Services.ChatGptClient;
using PmHelper.Domain.Models.Documents;
using PmHelper.Domain.Repository;
using PmHelper.Domain.Repository.Entities;
using PmHelper.Domain.Services.ChatGpt;
using System.Text;

namespace PmHelper.Domain.Services.Documents
{
    internal class DocumentService : IDocumentService
    {
        private readonly ILogger<IDocumentService> _logger;

        private readonly AppDbContext _dbContext;
        private readonly IDocumentGptService _docGptService;

        public DocumentService(AppDbContext dbContext, IDocumentGptService docGptService, ILogger<IDocumentService> logger)
        {
            _dbContext = dbContext;
            _docGptService = docGptService;

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

        private async Task<string> MakeDocumentPromtAsync(DbDocumentType dbDocumentType, string requestText)
        {
            var docTypeId = dbDocumentType.Id;
            var docTypeName = dbDocumentType.Name;

            _logger.LogInformation("DocumentService::MakeDocumentPromt: start making document promt for id={}, type={}",
                docTypeId, docTypeName);

            var resultDocumentFormat = string.Empty;
            switch ((DocumentFormatType)dbDocumentType.ResultFormat)
            {
                case DocumentFormatType.Plain:
                    resultDocumentFormat = "plain text";
                    break;
                case DocumentFormatType.Markdown:
                    resultDocumentFormat = "markdown";
                    break;
                default:
                    resultDocumentFormat = "plain text";
                    break;
            }
            _logger.LogInformation("DocumentService::MakeDocumentPromt: for type {} result format is {}",
                docTypeName, resultDocumentFormat);

            /*
             * You have to create a {DocumentType.Name} document.
             * {DocumentType.Promt}: "<user text request>".
             * Provide result in {DocumentType.ResultFormat}.
             * The result document must contain next sections in order of priority: { ...DocumentType.DocumentSection }.
             * The result must satisfy the following rules: { ...DocumentRule.RuleText }
            */
            var documentPromt = new StringBuilder();

            documentPromt.Append($"You have to create a {dbDocumentType.Name} document.");
            documentPromt.Append($"{dbDocumentType.Prompt}: \"{requestText}\".");
            documentPromt.Append($"Provide result in {resultDocumentFormat} format.");

            // Add sections
            var dbSections = await _dbContext.DocumentSections
                .Where(section => section.TypeId == docTypeId)
                .OrderBy(section => section.Priority)
                .Select(section => section.Title)
                .ToListAsync();
            if ((dbSections != null) && (dbSections.Count > 0))
            {
                _logger.LogInformation("DocumentService::MakeDocumentPromt: for document type {} added {} sections",
                    docTypeName, dbSections.Count);

                var sections = string.Join(",", dbSections);
                documentPromt.Append($"The result document must contain next sections in order of priority: {sections}");
            }
            // End add sections

            // Add rules
            var dbRules = await _dbContext.DocumentRuleTypes
                .Where(ruleType => ((ruleType.TypeId == docTypeId) && (ruleType.DocumentRule != null)))
                .Select(ruletype => ruletype.DocumentRule)
                .OrderBy(rule => rule!.Priority)
                .Select(rule => rule!.RuleText)
                .ToListAsync();
            if ((dbRules != null) && (dbRules.Count > 0))
            {
                _logger.LogInformation("DocumentService::MakeDocumentPromt: for document type {} added {} rules",
                    docTypeName, dbRules.Count);

                var rules = string.Join(";", dbRules);
                documentPromt.Append($"The result must satisfy the following rules:{rules}");
            }
            // end add rules

#if DEBUG
            _logger.LogInformation("DocumentService::MakeDocumentPromt: promt for {} is \"{}\"",
                docTypeName, documentPromt);
#endif
            return documentPromt.ToString();
        }

        public async Task<IUserDocument?> GenerateUserDocumentAsync(int userId, int documentTypeId, string requestText)
        {
            _logger.LogInformation(
                "DocumentService::GenerateUserDocumentAsync: start generate document for user={}. DocumentType={}",
                userId, documentTypeId
            );

            var dbDocumentType = await _dbContext.DocumentTypes
                .FirstOrDefaultAsync(doc => doc.Id == documentTypeId);

            if (dbDocumentType == null )
            {
                throw new Exception("");
            }

            _logger.LogInformation(
                "DocumentService::GenerateUserDocumentAsync: document type -> Name: {}, AssistantName: {}, ResultFormat: {}",
                dbDocumentType.Name, dbDocumentType.AssistantName, dbDocumentType.ResultFormat
            );

            // Make document promt
            var systemPromt = $"You are {dbDocumentType.AssistantName}. You are expert in subject.";
            var documentPromt = await MakeDocumentPromtAsync(dbDocumentType, requestText);

            // Ask ChatGpt to create document
            _logger.LogInformation(
                "DocumentService::GenerateUserDocumentAsync: ask ChatGpt to create document {}",
                dbDocumentType.Name);
            var documentText = await _docGptService.GenerateDocumentAsync(systemPromt, documentPromt);

            _logger.LogInformation(documentText);

            throw new NotImplementedException();
        }
    }
}
