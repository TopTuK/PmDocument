using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt.ResponseDTO
{
    internal class ChatGptMessage : IChatGptMessage
    {
        public string Role { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
