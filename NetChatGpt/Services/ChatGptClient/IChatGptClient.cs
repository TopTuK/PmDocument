using NetChatGptCLient.Models.ChatGpt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Services.ChatGptClient
{
    public interface IChatGptClient
    {
        IChatGptOptions Options { get; }

        IChatGptConversationCache ConversationCache { get; }

        Task<IChatGptConversation> StartNewConversation(string systemMessage);
        Task<string> AskAsync(IChatGptConversation conversation, string message);
        Task<string> AskAsync(Guid conversationId, string message);
    }
}
