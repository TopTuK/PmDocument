using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt
{
    /// <summary>
    /// Contains information about the error occurred while invoking the service.
    /// </summary>
    /// <remarks>
    /// See <see href="https://platform.openai.com/docs/guides/error-codes">Error codes</see> for more information.
    /// </remarks>
    public interface IChatGptError
    {
        /// <summary>
        /// Gets the error message.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets the error type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Gets the parameter that caused the error.
        /// </summary>
        string? Parameter { get; }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        string? Code { get; }
    }
}
