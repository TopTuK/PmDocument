using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChatGptCLient.Models.ChatGpt.Cache
{
    internal class DefaultConversationCache : IChatGptConversationCache
    {
        private readonly Dictionary<Guid, IChatGptConversation> _conversations = new();

        public async Task AddAsync(IChatGptConversation conversation)
        {
            _conversations.TryAdd(conversation.ConversationId, conversation);
            await Task.CompletedTask;
        }

        public async Task<IChatGptConversation?> GetAsync(Guid conversationId)
        {
            if (_conversations.TryGetValue(conversationId, out var conversation))
            { 
                return await Task.FromResult<IChatGptConversation?>(conversation);
            }

            return await Task.FromResult<IChatGptConversation?>(null);
        }

        public async Task RemoveAsync(Guid conversationId)
        {
            _conversations.Remove(conversationId);
            await Task.CompletedTask;
        }
    }
}
