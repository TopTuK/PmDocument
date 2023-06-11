using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt
{
    internal static class ChatGptRoles
    {
        public const string SystemRole = "system";
        public const string UserRole = "user";
        public const string AssistantRole = "assistant";
    }

    public interface IChatGptMessage
    {
        /// <summary>
        /// Gets or sets the role (source) of the message.
        /// </summary>
        /// <remarks>
        ///  Valid values are: <em>system</em>, <em>user</em> and <em>assistant</em>.
        ///  </remarks>
        [JsonPropertyName("role")]
        string Role { get; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        [JsonPropertyName("content")]
        string Content { get; }
    }
}
