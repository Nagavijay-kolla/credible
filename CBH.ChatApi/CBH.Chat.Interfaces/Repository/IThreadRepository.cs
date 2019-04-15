using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Enumerations;

namespace CBH.Chat.Interfaces.Repository
{
    public interface IThreadRepository
    {
        Task<ThreadEntity> GetAsync(Guid threadId);
        Task<ThreadEntity> GetByTypeAsync(ThreadType threadType);
        Task<ThreadEntity> CreateAsync(ThreadEntity threadEntity);
        Task<ThreadEntity> UpdateAsync(ThreadEntity threadEntity);
        Task DeleteByMultiIdAsync(Guid multiId);
        Task DeleteByParticipantIdsAsync(int userId, int participantId);
        Task<ThreadEntity> SearchByParticipantIdsAsync(int firstParticipant, int secondParticipant);
        Task<ThreadEntity> SearchByGroupIdAsync(Guid groupId);
        Task<ThreadEntity> SearchByTeamIdAsync(int groupId);
        Task<IEnumerable<ThreadEntity>> SearchByParticipantIdAsync(int participant);
		Task<IEnumerable<ThreadEntity>> GetAllThreadsAsync();
    }
}