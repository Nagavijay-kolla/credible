using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Interfaces.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBH.Chat.Repository.ChatRepository
{
    public class LogRepository : ILogRepository
    {
        private readonly IMongoCollection<LogEntity> _logs;
        private static readonly FindOptions<LogEntity, LogEntity> OrderByTimeFindOptions = new FindOptions<LogEntity, LogEntity>
        {
            Sort = Builders<LogEntity>.Sort.Ascending(x => x.CreatedAt)
        };
        private static readonly FindOneAndReplaceOptions<LogEntity, LogEntity> DefaultFindAndReplaceOption = new FindOneAndReplaceOptions<LogEntity, LogEntity>
        {
            ReturnDocument = ReturnDocument.After
        };

        public LogRepository(IChatDbClient dbClient)
        {
            _logs = dbClient.Database.GetCollection<LogEntity>(DbConstants.logCollection);
        }

        public async Task<LogEntity> CreateAsync(LogEntity newlog)
        {
            await _logs.InsertOneAsync(newlog);
            return newlog;
        }

        public async Task<IEnumerable<LogEntity>> GetAllAsync()
        {
            return (await _logs.FindAsync(x => true, OrderByTimeFindOptions)).ToEnumerable();
        }

        public async Task<IEnumerable<LogEntity>> GetByMessageIdAsync(Guid messageId)
        {
            return (await _logs.FindAsync(x => x.MessageId==messageId, OrderByTimeFindOptions)).ToEnumerable();
        }

        public async Task<IEnumerable<LogEntity>> GetByThreadIdAsync(Guid threadId)
        {
            return (await _logs.FindAsync(x => x.ThreadId == threadId, OrderByTimeFindOptions)).ToEnumerable();
        }
        public async Task<IEnumerable<LogEntity>> GetByuserIdAsync(int userid)
        {
            return (await _logs.FindAsync(x => x.SenderId == userid || x.RecipientId==userid, OrderByTimeFindOptions)).ToEnumerable();
        }

        public async Task<LogEntity> UpdateAsync(LogEntity message)
        {
            return await _logs.FindOneAndReplaceAsync<LogEntity>(x => x.Id == message.Id, message, DefaultFindAndReplaceOption);
        }
        public async Task ArchiveMessageAsync(Guid threadId)
        {
            var logs = (await _logs.FindAsync(x => x.ThreadId == threadId && !x.IsArchived, OrderByTimeFindOptions)).ToEnumerable();
            foreach(var log in logs)
            {
                log.IsArchived = true;
                log.ArchivedAt = DateTime.UtcNow;
                await _logs.FindOneAndReplaceAsync<LogEntity>(x => x.Id == log.Id, log, DefaultFindAndReplaceOption);
            }
        }
        public async Task ReadMessageAsync(Guid messageId)
        {
            var logs = (await _logs.FindAsync(x => x.MessageId == messageId && !x.IsRead, OrderByTimeFindOptions)).ToEnumerable();
            foreach (var log in logs)
            {
                log.IsRead = true;
                log.ReadAt = DateTime.UtcNow;
                await _logs.FindOneAndReplaceAsync<LogEntity>(x => x.Id == log.Id, log, DefaultFindAndReplaceOption);
            }
        }

        public async Task<IEnumerable<LogEntity>> GetByPartnerIdAsync(SentMessageReportRequestModel filter)
        {
            if (filter.IsImportant == null)
            {
                if (filter.EndDate == null || filter.StartDate == null)
                {
                    return (await _logs.FindAsync(x => true, OrderByTimeFindOptions)).ToEnumerable();
                }
                else
                {
                    return (await _logs.FindAsync(x => true && x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate, OrderByTimeFindOptions)).ToEnumerable();
                }
            }
            else
            {
                return (await _logs.FindAsync(x => true && x.IsImportant == filter.IsImportant && x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate, OrderByTimeFindOptions)).ToEnumerable();
            }
        }

        public async Task<IEnumerable<LogEntity>> GetByDatesAsync(AggregateUtilizationReportRequestModel filter, ICollection<int> employeeIds)
        {

            return (await _logs.FindAsync(x => true && x.CreatedAt >= filter.StartDate && x.CreatedAt <= filter.EndDate && 
            (employeeIds.Contains(x.SenderId) || employeeIds.Contains(x.RecipientId)) , OrderByTimeFindOptions)).ToEnumerable();
        }
    }
}
