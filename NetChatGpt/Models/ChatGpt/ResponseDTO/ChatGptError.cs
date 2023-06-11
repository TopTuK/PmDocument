using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt.ResponseDTO
{
    
    internal class ChatGptError : IChatGptError
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        public string Type { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the parameter that caused the error.
        /// </summary>
        [JsonPropertyName("param")]
        public string? Parameter { get; init; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string? Code { get; init; }
    }
}
