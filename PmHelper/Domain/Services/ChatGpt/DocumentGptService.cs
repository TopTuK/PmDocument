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
#if DEBUG
                await Task.Delay(5000);
                var chatResponse = """
                    ## Title: AI DocGen Assistant - Your Ultimate Documentation Partner!

                    ## Content of Works:
                    The project aims to create an AI-powered web service that will serve as an assistant for project managers and team members in generating various types of documents. This includes project charters, project documentation, specifications, and more, in multiple formats. The service will require users to subscribe to access the document generation feature, with different subscription tiers offering varying document generation limits. To generate documents, users will provide a description for the document they need, and the AI system will utilize ChatGpt technology to automatically generate the desired documents based on predefined templates.

                    ## Cost of Work:
                    The estimated cost for completing the project is $200,000. This includes expenses for development, AI technology integration, infrastructure setup, testing, documentation, and project management.

                    ## Terms of Work:
                    The project is expected to be completed within a period of 3 months. The timeline is as follows:

                    - Month 1: Requirements gathering, system design, and development kick-off.
                    - Month 2: AI integration, UI/UX development, testing, and performance optimization.
                    - Month 3: Final testing, bug fixing, documentation, user acceptance testing, and project handover.

                    ## Main Stakeholders:
                    - Project Manager: [Your Name]
                    - Development Team
                    - AI Research Team
                    - Quality Assurance Team
                    - Marketing and Sales Team
                    - Subscription Management Team
                    - Users (Project Managers and Project Team Members)

                    ## Terminating Risks:
                    1. Lack of user adoption and acceptance of the AI-generated documents.
                    2. Technical constraints or limitations in generating specific document types or formats.
                    3. Insufficient availability or access to relevant document templates or specifications.
                    4. Legal and compliance concerns regarding the generated documents.
                    5. Inability to provide satisfactory customer support or address user queries effectively.

                    ## Delivery Results:
                    - AI-powered web service with a user-friendly interface.
                    - Document generation functionality, utilizing ChatGpt technology.
                    - Multiple document templates and formatting options.
                    - Subscription management system and payment integration.

                    ## Key Performance Indicators:
                    - Number of active users and user retention rate.
                    - Average time taken to generate a document.
                    - User satisfaction and feedback ratings.
                    - Monthly revenue from subscription sales.
                    - Number of document formats and templates available.

                    Please note that the information provided is based on the description provided and may require further refinement during the project initiation phase.
                    """;
#else
                var conversation = await _chatGptClient.StartNewConversation(systemMessage);
                var chatResponse = await _chatGptClient.AskAsync(conversation, documentPromt);
#endif

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
