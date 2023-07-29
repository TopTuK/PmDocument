using NetChatGptCLient.Services.ChatGptClient;

namespace PmHelper.Domain.Services.ChatGpt
{
    internal class DocumentGptService : IDocumentGptService
    {
        private const string CHAT_GPT_API_URL = "ChatGptApiUrl";
        private const string CHAT_GPT_KEY = "ChatGptApiKey";

        private readonly ILogger<IDocumentGptService> _logger;

        private readonly IChatGptClient _chatGptClient;

        public DocumentGptService(IConfiguration configuration, ILogger<IDocumentGptService> logger)
        {
            var chatGptApiUrl = configuration[CHAT_GPT_API_URL]!;
            var chatGptApiKey = configuration[CHAT_GPT_KEY] ?? string.Empty;

            _chatGptClient = ChatGptClientFactory.CreateApiClient(chatGptApiUrl, chatGptApiKey);

            _logger = logger;
        }

        public async Task<string?> GenerateDocumentAsync(string systemMessage, string documentPromt)
        {
            try
            {
                var conversation = await _chatGptClient.StartNewConversation(systemMessage);
                var chatResponse = await _chatGptClient.AskAsync(conversation, documentPromt);

                return chatResponse;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("IDocumentGptService::GenerateDocumentAsync: Exception raised. Msg: {}", ex.Message);
                return null;
            }
        }
    }
}
