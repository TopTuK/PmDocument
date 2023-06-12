using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt.ResponseDTO
{
    /// <summary>
    /// Represent a chat completion choice.
    /// </summary>
    internal class ChatGptChoice
    {
        /// <summary>
        /// Gets or sets the index of the choice in the list.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the message associated with this <see cref="ChatGptChoice"/>.
        /// </summary>
        /// <seealso cref="ChatGptChoice"/>
        public ChatGptMessage Message { get; set; } = new();

        /// <summary>
        /// When using streaming responses, gets or sets the partial message delta associated with this <see cref="ChatGptChoice"/>.
        /// </summary>
        /// <see cref="ChatGptRequest.Stream"/>
        public IChatGptMessage? Delta { get; set; } = null;

        /// <summary>
        /// Gets or sets a value specifying why the choice has been returned.
        /// </summary>
        /// <remarks>
        /// Possible values are: <em>stop</em> (API returned complete model output), <em>length</em> (incomplete model output due to max_tokens parameter or token limit), <em>content_filter</em> (omitted content due to a flag from content filters) or <em>null</em> (API response still in progress or incomplete).
        /// </remarks>
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = string.Empty;
    }
}
