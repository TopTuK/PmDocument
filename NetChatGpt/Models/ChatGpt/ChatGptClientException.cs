using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt
{
    public class ChatGptClientException : Exception
    {
        /// <summary>
        /// Gets the detailed error information.
        /// </summary>
        /// <seealso cref="IChatGptError"/>
        public IChatGptError? Error { get; } = null;

        public ChatGptClientException()
            : base() { }

        public ChatGptClientException(string message)
            : base(message) { }

        public ChatGptClientException(string message, Exception innerException)
            : base(message, innerException) { }

        public ChatGptClientException(string message, IChatGptError error)
            : base(message)
        {
            Error = error;
        }
    }
}
