using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Interfaces.Repository;
using MongoDB.Driver;

namespace CBH.Chat.Repository.ChatRepository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<MessageEntity> _messages;
        private readonly int _archivalDays;
        private static readonly FindOptions<MessageEntity, MessageEntity> OrderByTimeFindOptions = new FindOptions<MessageEntity, MessageEntity>
        {
            Sort = Builders<MessageEntity>.Sort.Ascending(x => x.CreatedAt)
        };
        private static readonly FindOneAndReplaceOptions<MessageEntity, MessageEntity> DefaultFindAndReplaceOption = new FindOneAndReplaceOptions<MessageEntity, MessageEntity>
        {
            ReturnDocument = ReturnDocument.After
        };

        public MessageRepository(IChatDbClient dbClient,int archivalDays)
        {
            _messages = dbClient.Database.GetCollection<MessageEntity>(DbConstants.MessageCollection);
            _archivalDays = archivalDays;
        }

        public async Task<IEnumerable<MessageEntity>> AddAndGetAsync(MessageEntity newMessage)
        {
            await _messages.InsertOneAsync(newMessage);
            return (await _messages.FindAsync(x => x.ThreadId == newMessage.ThreadId, OrderByTimeFindOptions)).ToEnumerable();
        }

        public async Task<IEnumerable<MessageEntity>> GetByThreadIdAsync(Guid threadId)
        {
            var res= (await _messages.FindAsync(x => x.ThreadId == threadId , OrderByTimeFindOptions)).ToList();
            return res?.FindAll(x => DateTime.UtcNow.Subtract(x.CreatedAt).TotalDays < _archivalDays);
        }

        public async Task<MessageEntity> GetAsync(Guid messageId)
        {
            return await (await _messages.FindAsync(x => x.Id == messageId)).SingleOrDefaultAsync();
        }

        public async Task<MessageEntity> UpdateAsync(MessageEntity message)
        {
            return await _messages.FindOneAndReplaceAsync<MessageEntity>(x => x.Id == message.Id, message, DefaultFindAndReplaceOption);
        }
    }
}