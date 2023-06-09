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
    }
}
