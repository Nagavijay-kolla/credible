using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Entity;

namespace CBH.Chat.Interfaces.Repository
{
    public interface IMessageRepository
    {
        Task<MessageEntity> GetAsync(Guid messageId);
        Task<MessageEntity> UpdateAsync(MessageEntity message);
        Task<IEnumerable<MessageEntity>> AddAndGetAsync(MessageEntity newMessage);
        Task<IEnumerable<MessageEntity>> GetByThreadIdAsync(Guid threadId);
    }
}