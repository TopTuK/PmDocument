using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PmHelper.Domain.Services.Documents;

namespace PmHelper.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService, ILogger<DocumentController> logger)
        {
            _documentService = documentService;

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
            var userDocuments = await _documentService.GetUserDocumentsAsync(userId);
            _logger.LogInformation(
                message: "DocumentController::GetCurrentUserDocuments: got {} documents",
                userDocuments.Count()
            );

            return new JsonResult(userDocuments);
        }
    }
}
