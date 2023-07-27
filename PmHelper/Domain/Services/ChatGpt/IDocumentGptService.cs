namespace PmHelper.Domain.Services.ChatGpt
{
    public interface IDocumentGptService
    {
        Task<string?> GenerateDocumentAsync(string systemMessage, string documentPromt);
    }
}
