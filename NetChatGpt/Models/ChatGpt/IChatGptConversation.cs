using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt
{
    public interface IChatGptConversation
    {
        Guid ConversationId { get; }
        IReadOnlyList<IChatGptMessage> Messages { get; }

        IChatGptMessage AddMessage(string role, string content);
    }
}
