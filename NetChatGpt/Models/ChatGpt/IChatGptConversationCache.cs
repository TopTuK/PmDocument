using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt
{
    /// <summary>
    /// Represents a local in-memory cache.
    /// </summary>
    public interface IChatGptConversationCache
    {
        /// <summary>
        /// Adds given conversation in cache.
        /// </summary>
        /// <param name="conversation">The conversation.</param>
        /// <param name="expiration">The amount of time in which messages must be stored in cache.</param>
        /// <returns>The <see cref="Task"/> corresponding to the asynchronous operation.</returns>
        Task AddAsync(IChatGptConversation conversation);

        /// <summary>
        /// Gets the conversaation for the given <paramref name="conversationId"/>.
        /// </summary>
        /// <param name="conversationId"></param>
        /// <returns>The conversation, or <see langword="null"/> if the ConversationId Id does not exist.</returns>
        Task<IChatGptConversation?> GetAsync(Guid conversationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        Task RemoveAsync(Guid conversationId);
    }
}
