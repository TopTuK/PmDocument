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

        IChatGptConversation StartNewConversation(string systemMessage);
        Task AskAsync(IChatGptConversation conversation, string message);
        Task AskAsync(Guid conversationId, string message);
    }
}
