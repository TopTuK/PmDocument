using NetChatGptCLient.Models.ChatGpt;
using NetChatGptCLient.Models.ChatGpt.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Services.ChatGptClient
{
    public static class ChatGptClientFactory
    {
        private record ChatGptClientOptions : IChatGptOptions
        {
            public string ApiSecret { get; init; }
            public string Model { get; set; } = OpenAIChatGptModels.Gpt35Turbo;
            public double Temperature { get; set; } = 1.0;
            public double TopP { get; set; } = 1.0;
            public int MaxTokens { get; set; } = 256;
            public double PresencePenalty { get; set; } = 0.0;
            public double FrequencyPenalty { get; set; } = 0.0;

            public ChatGptClientOptions(string apiSecret)
            {
                ApiSecret = apiSecret;
            }
        }

        public static IChatGptClient CreateDefaultClient(string apiSecret)
        {
            var options = new ChatGptClientOptions(apiSecret);
            var cache = new DefaultConversationCache();

            return new ChatGptClientImpl(options, cache);
        }

        public static IChatGptClient CreateClient(string apiSecret, IChatGptConversationCache cache)
        {
            var options = new ChatGptClientOptions(apiSecret);
            return new ChatGptClientImpl(options, cache);
        }
    }
}
