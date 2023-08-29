using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PmHelper.Domain.Models.Requests;
using PmHelper.Domain.Models.Users;
using PmHelper.Domain.Services.Documents;
using PmHelper.Domain.Services.Users;

namespace PmHelper.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        
        private readonly IDocumentService _documentService;
        private readonly IUserService _userService;

        public DocumentController(IDocumentService documentService, IUserService userService,
            ILogger<DocumentController> logger)
        {
            _documentService = documentService;
            _userService = userService;

            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUserDocuments()
        { 
            _logger.LogInformation("DocumentController::GetCurrentUserDocuments: start handling request");
            
            var userId = (int) HttpContext.Items["userId"]!;

            _logger.LogInformation(
                message: "DocumentController::GetCurrentUserDocuments: get documents for user {}",
                userId
            );
            var userDocuments = await _documentService.GetDocumentsByUserIdAsync(userId);
            _logger.LogInformation(
                message: "DocumentController::GetCurrentUserDocuments: got {} documents for user {}",
                userDocuments.Count(), userId
            );

            return new JsonResult(userDocuments);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDocument([FromBody] GenerateDocumentInfo documentRequest)
        {
            _logger.LogInformation("DocumentController::GenerateDocument: start generate document. Document type={}",
                documentRequest.TypeId);

            var userId = (int)HttpContext.Items["userId"]!;
            _logger.LogInformation("DocumentController::GenerateDocument: userId={}", userId);

            try
            {
                var userDocument = await _documentService.GenerateUserDocumentAsync(
                    userId,
                    documentRequest.TypeId, documentRequest.Name, 
                    documentRequest.Text
                );

                _logger.LogInformation("DocumentController::GenerateDocument: generated user document");
                return new JsonResult(userDocument);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("DocumentController::GenerateDocument: Exception raised. Msg: ", ex.Message);
                return BadRequest("Exception raised while document generation");
            }
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUserDocument(int documentId)
        {
            var userId = (int)HttpContext.Items["userId"]!;
            _logger.LogInformation("DocumentController::RemoveUserDocument: start removing document. DocumentId={}, UserId={}", 
                documentId, userId);

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("DocumentController::RemoveUserDocument: can't find user with Id={}", userId);
                return BadRequest();
            }

            _logger.LogInformation("DocumentController::RemoveUserDocument: current user is {}. IsAdmin={}",
                user.Email, user.IsAdmin);

            var userDocument = await _documentService.GetUserDocumentAsync(documentId);
            if (userDocument != null)
            {
                if ((userDocument.User.Id == user.Id) || (user.IsAdmin))
                {
                    _logger.LogInformation("DocumentController::RemoveUserDocument: user {} has rights to remove document with Id={}",
                        userId, documentId);

                    var document = await _documentService.RemoveUserDocumentAsync(documentId);
                    if (document != null)
                    {
                        _logger.LogInformation("DocumentController::RemoveUserDocument: document with Id={} was removed",
                            documentId);
                        return new JsonResult(new
                        {
                            Status = "OK",
                            document.Id,
                            document.Title,
                        });
                    }
                    else
                    {
                        _logger.LogError("DocumentController::RemoveUserDocument: can't remove document with Id={}",
                            documentId);
                        return BadRequest("Can't remove document");
                    }
                }
                else
                {
                    _logger.LogWarning("DocumentController::RemoveUserDocument: user {} has no rights to remove document with Id={}",
                        userId, documentId);
                    return BadRequest("Access Denied");
                }
            }

            _logger.LogError("DocumentController::RemoveUserDocument: document with Id={} is not found", documentId);
            return BadRequest("Document with Id={} is not found");
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetDocumentsStatistic()
        {
            _logger.LogInformation("DocumentController::GetDocumentsStatistic: start getting user documents statistic");

            var userDocuments = await _documentService.GetAllUserDocumentsAsync();
            var userDocumentsCount = userDocuments.Count();
            _logger.LogInformation("DocumentController::GetDocumentsStatistic: got {} users with document",
                userDocumentsCount);

            var users = userDocuments
                .Select(ud => ud.User)
                .ToList();
            _logger.LogInformation("DocumentController::GetDocumentsStatistic: users count={}", users.Count);

            var totalDocumentsCount = 0;
            var userDocumentsStatistic = new Dictionary<int, int>(userDocumentsCount);
            foreach (var userDocument in userDocuments)
            {
                totalDocumentsCount += userDocument.Documents.Count;
                userDocumentsStatistic.Add(userDocument.User.Id, userDocument.Documents.Count);
            }
            
            _logger.LogInformation("DocumentController::GetDocumentsStatistic: Total Documents Count = {}",
                totalDocumentsCount);
            return new JsonResult(new
            {
                TotalDocuments = totalDocumentsCount,
                UserDocumentsStatistic = userDocumentsStatistic,
                Users = users,
            });
        }
    }
}
