using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt.ResponseDTO
{
    /// <summary>
    /// Contains information about the API usage.
    /// </summary>
    /// <remarks>
    /// See <see href="https://help.openai.com/en/articles/4936856-what-are-tokens-and-how-to-count-them">What are tokens and how to count them?</see> for more information.
    /// </remarks>
    internal class ChatGptUsage
    {
        /// <summary>
        /// Gets or sets the number of tokens of the request.
        /// </summary>
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens { get; set; }

        /// <summary>
        /// Gets or sets the number of token of the response.
        /// </summary>
        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        /// <summary>
        /// Gets the total number of tokens.
        /// </summary>
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
