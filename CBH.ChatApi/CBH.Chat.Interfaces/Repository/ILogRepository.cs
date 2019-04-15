using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CBH.Chat.Interfaces.Repository
{
    public interface ILogRepository
    {
        Task<IEnumerable<LogEntity>> GetAllAsync();
        Task<IEnumerable<LogEntity>> GetByThreadIdAsync(Guid threadId);
        Task<IEnumerable<LogEntity>> GetByMessageIdAsync(Guid messageId);
        Task<LogEntity> CreateAsync(LogEntity newlog);
        Task<LogEntity> UpdateAsync(LogEntity message);
        Task ArchiveMessageAsync(Guid threadId);
        Task ReadMessageAsync(Guid messageId);
        Task<IEnumerable<LogEntity>> GetByuserIdAsync(int userid);

        Task<IEnumerable<LogEntity>> GetByDatesAsync(AggregateUtilizationReportRequestModel filter, ICollection<int> employeeIds);
    }
}

