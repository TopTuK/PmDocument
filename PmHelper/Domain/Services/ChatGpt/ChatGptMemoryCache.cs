using Microsoft.Extensions.Caching.Memory;
using NetChatGptCLient.Models.ChatGpt;

namespace PmHelper.Domain.Services.ChatGpt
{
    internal class ChatGptMemoryCache : IChatGptConversationCache
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _expirationTimeout;

        public ChatGptMemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _expirationTimeout = TimeSpan.FromHours(1);
        }

        public Task AddAsync(IChatGptConversation conversation)
        {
            _memoryCache.Set(conversation.ConversationId, conversation, _expirationTimeout);
            
            return Task.CompletedTask;
        }

        public Task<IChatGptConversation?> GetAsync(Guid conversationId)
        {
            //var conversation = _memoryCache.TryGetValue<IChatGptConversation>()
            var conversation = _memoryCache.Get<IChatGptConversation>(conversationId);

            return Task.FromResult(conversation);
        }

        public Task RemoveAsync(Guid conversationId)
        {
            _memoryCache.Remove(conversationId);
            return Task.CompletedTask;
        }
    }
}
