using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt
{
    internal interface IChatGptRequest
    {
        [JsonPropertyName("model")]
        string model { get; }

        [JsonPropertyName("temperature")]
        double temperature { get; }

        [JsonPropertyName("top_p")]
        double top_p { get; }

        [JsonPropertyName("max_tokens")]
        int max_tokens { get; }

        [JsonPropertyName("presence_penalty")]
        double presence_penalty { get; }

        [JsonPropertyName("frequency_penalty")]
        double frequency_penalty { get; }

        [JsonPropertyName("messages")]
        IReadOnlyList<IChatGptMessage> messages { get; }

        [JsonPropertyName("stream")]
        bool stream { get; }
    }
}
