﻿using Microsoft.AspNetCore.Authorization;
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

            var userDocumentsSTatistic = userDocuments
                .Select(ud => new
                {
                    ud.User,
                    ud.Documents.Count,
                })
                .ToList();

            var totalDocumentsCount = 0;
            foreach (var userDocument in userDocumentsSTatistic)
            {
                totalDocumentsCount += userDocument.Count;
            }

            _logger.LogInformation("DocumentController::GetDocumentsStatistic: Total Documents Count = {}",
                totalDocumentsCount);

            return new JsonResult(new
            {
                TotalDocuments = totalDocumentsCount,
                UserDocumentsStatistic = userDocumentsSTatistic,
            });
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetDocumentsTypes()
        {
            _logger.LogInformation("DocumentController::GetDocumentsTypes: start getting document types");

            var documentTypes = await _documentService.GetDocumentsTypesAsync();

            _logger.LogInformation("DocumentController::GetDocumentsTypes: return {} document types",
                documentTypes.Count());
            return new JsonResult(documentTypes);
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetDocumentPromtType(int id)
        {
            _logger.LogInformation("DocumentController::GetDocumentType: get document type with promt. Id={}",
                id);

            var documentType = await _documentService.GetDocumentTypeAsync(id);
            if (documentType == null)
            {
                _logger.LogError("DocumentController::GetDocumentType: document type with id={} is not found", id);
                return BadRequest($"Document type with id={id} is not found");
            }

            _logger.LogInformation("DocumentController::GetDocumentType: return document type: {} {}",
                documentType.Id, documentType.Name);
            return new JsonResult(documentType);
        }
    }
}
