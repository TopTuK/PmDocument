using NetChatGptCLient.Models.ChatGpt;
using NetChatGptCLient.Models.ChatGpt.ResponseDTO;
using NetChatGptCLient.Models.HttpClient;
using NetChatGptCLient.Services.HttpClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Services.ChatGptClient
{
    internal class ChatGptClientImpl : IChatGptClient
    {
        private const string API_ACTION = "v1/chat/completions";

        private class ChatGptMessage : IChatGptMessage
        {
            public string Role { get; set; } = string.Empty;
            public string Content { get; set; } = string.Empty;
        }

        private class ChatGptConversation : IChatGptConversation
        {
            private readonly List<IChatGptMessage> _messages = new();

            public Guid ConversationId { get; } = Guid.NewGuid();
            public IReadOnlyList<IChatGptMessage> Messages => _messages;

            public IChatGptMessage AddMessage(string role, string content)
            {
                var message = new ChatGptMessage {  Role = role, Content = content };

                _messages.Add(message);

                return message;
            }
        }

        private readonly IHttpClient _httpClient;
        private readonly IChatGptOptions _options;

        private readonly Dictionary<Guid, ChatGptConversation> _conversations = new();

        public IChatGptOptions Options => _options;

        public ChatGptClientImpl(IChatGptOptions options)
        {
            _options = options;
            _httpClient = HttpClientFactory.CreateHttpClient(options.ApiUrl, options.ApiSecret);
        }

        public IChatGptConversation StartNewConversation(string systemMessage)
        {
            var conversetation = new ChatGptConversation();
            conversetation.AddMessage(ChatGptRoles.SystemRole, systemMessage);

            _conversations.Add(conversetation.ConversationId, conversetation);

            return conversetation;
        }

        public async Task AskAsync(IChatGptConversation conversation, string message)
        {
            await AskAsync(conversation.ConversationId, message);
        }

        private class ChatGptRequest : IChatGptRequest
        {
            public string model { get; init; } = string.Empty;
            public double temperature { get; init; }
            public double top_p { get; init; }
            public int max_tokens { get; init; }
            public double presence_penalty { get; init; }
            public double frequency_penalty { get; init; }

            public IReadOnlyList<IChatGptMessage> messages { get; init; } = new List<IChatGptMessage>();
            public bool stream => false;
        }

        private IChatGptRequest CreateRequest(IReadOnlyList<IChatGptMessage> messages) => new ChatGptRequest
        {
            model = _options.Model,
            temperature = _options.Temperature,
            top_p = _options.TopP,
            max_tokens = _options.MaxTokens,
            presence_penalty = _options.PresencePenalty,
            frequency_penalty = _options.FrequencyPenalty,
            messages = messages,
        };

        public async Task AskAsync(Guid conversationId, string message)
        {
            if(!_conversations.ContainsKey(conversationId)) 
            {
                throw new ChatGptClientException();
            }

            var converastion = _conversations[conversationId]!;
            converastion.AddMessage(ChatGptRoles.UserRole, message);

            var request = CreateRequest(converastion.Messages);

            try
            {
                var response = await _httpClient.PostJsonAsync(API_ACTION, request);

                if ((response != null) && (response.Content != null))
                {
                    var content = JObject.Parse(response.Content);
                    
                    if (response.IsSuccess)
                    {
                        // Parse ChatGptResponse
                        // Ensure Error is null or empty
                    }
                    else
                    {
                        // Ensure Error is set
                    }
                }
                else
                {
                    throw new ChatGptClientException("");
                }
            }
            catch (Exception ex)
            {
                throw new ChatGptClientException("", ex);
            }
        }
    }
}
