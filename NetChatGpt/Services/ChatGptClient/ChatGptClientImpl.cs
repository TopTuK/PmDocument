using NetChatGptCLient.Models.ChatGpt;
using NetChatGptCLient.Models.ChatGpt.ResponseDTO;
using NetChatGptCLient.Models.HttpClient;
using NetChatGptCLient.Services.HttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Services.ChatGptClient
{
    internal class ChatGptClientImpl : IChatGptClient
    {
        private const string API_ACTION = "v1/chat/completions";

        private class ChatGptConversation : IChatGptConversation
        {
            private readonly ConcurrentQueue<IChatGptMessage> _queue = new();

            public Guid ConversationId { get; } = Guid.NewGuid();
            public IReadOnlyList<IChatGptMessage> Messages => _queue.ToList();

            public IChatGptMessage AddMessage(string role, string content)
            {
                var message = new ChatGptMessage {  Role = role, Content = content };

                _queue.Enqueue(message);

                return message;
            }
        }

        private readonly IHttpClient _httpClient;
        private readonly IChatGptOptions _options;

        public IChatGptOptions Options => _options;

        public IChatGptConversationCache ConversationCache { get; init; }

        public ChatGptClientImpl(IChatGptOptions options, IChatGptConversationCache cache)
        {
            _options = options;
            _httpClient = HttpClientFactory.CreateHttpClient(options.ApiUrl, options.ApiSecret);

            ConversationCache = cache;
        }

        public async Task<IChatGptConversation> StartNewConversation(string systemMessage)
        {
            var conversetation = new ChatGptConversation();
            conversetation.AddMessage(ChatGptRoles.SystemRole, systemMessage);

            await ConversationCache.AddAsync(conversetation);

            return conversetation;
        }

        public async Task<string> AskAsync(IChatGptConversation conversation, string message)
        {
            return await AskAsync(conversation.ConversationId, message);
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

        private IChatGptRequest CreateRequest(IReadOnlyList<IChatGptMessage> messages) 
            => new ChatGptRequest
        {
            model = _options.Model,
            temperature = _options.Temperature,
            top_p = _options.TopP,
            max_tokens = _options.MaxTokens,
            presence_penalty = _options.PresencePenalty,
            frequency_penalty = _options.FrequencyPenalty,
            messages = messages,
        };

        private static bool EnsureErrorIsNotSet(ChatGptResponse response)
        {
            return response.Error == null;
        }

        public async Task<string> AskAsync(Guid conversationId, string message)
        {
            var converastion = await ConversationCache.GetAsync(conversationId);

            if (converastion == null)
            {
                throw new ChatGptClientException("IChatGptClient::Exception: can't find conversation");
            }

            converastion.AddMessage(ChatGptRoles.UserRole, message);

            var request = CreateRequest(converastion.Messages);

            try
            {
                var response = await _httpClient.PostJsonAsync(API_ACTION, request);

                if ((response != null) && (response.Content != null))
                {   
                    if (response.IsSuccess)
                    {
                        // Parse ChatGptResponse
                        var chatGptResponse = JsonConvert
                            .DeserializeObject<ChatGptResponse>(response.Content);

                        // Ensure Error is null or empty
                        if ((chatGptResponse != null) && (EnsureErrorIsNotSet(chatGptResponse)))
                        {
                            var assistantMessage = chatGptResponse.GetMessage();

                            if (assistantMessage != null)
                            {
                                // Add message to conversation
                                converastion.AddMessage(ChatGptRoles.AssistantRole, assistantMessage);
                                return assistantMessage;
                            }
                            else
                            {
                                throw new ChatGptClientException("IChatGptClient::Exception: assistant message is empty");
                            }
                        }
                        else
                        {
                            throw new ChatGptClientException("IChatGptClient::Exception: Chat GPT response has error");
                        }
                    }
                    else
                    {
                        throw new ChatGptClientException("IChatGptClient::Exception: HTTP response is not success");
                    }
                }
                else
                {
                    throw new ChatGptClientException("IChatGptClient::Exception: HTTP response error");
                }
            }
            catch (Exception ex)
            {
                throw new ChatGptClientException("IChatGptClient::Exception: exception raised", ex);
            }
        }
    }
}
