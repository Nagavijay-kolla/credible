using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Interfaces.Repository;
using MongoDB.Driver;

namespace CBH.Chat.Repository.ChatRepository
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly IMongoCollection<ThreadEntity> _threads;
        private static readonly FindOneAndReplaceOptions<ThreadEntity, ThreadEntity> DefaultFindAndReplaceOptions = new FindOneAndReplaceOptions<ThreadEntity, ThreadEntity>
        {
            ReturnDocument = ReturnDocument.After
        };

        public ThreadRepository(IChatDbClient dbClient)
        {
            _threads = dbClient.Database.GetCollection<ThreadEntity>(DbConstants.ThreadCollection);
        }

        public async Task<ThreadEntity> CreateAsync(ThreadEntity threadEntity)
        {
            await _threads.InsertOneAsync(threadEntity);
            return threadEntity;
        }

        public async Task<ThreadEntity> GetAsync(Guid threadId)
        {
            return await (await _threads.FindAsync(x => x.Id == threadId)).SingleOrDefaultAsync();
        }

        public async Task<ThreadEntity> SearchByParticipantIdsAsync(int firstParticipant, int secondParticipant)
        {
            return await (await _threads.FindAsync(x => x.Participants.Contains(firstParticipant) && x.Participants.Contains(secondParticipant) && x.Type == ThreadType.User)).SingleOrDefaultAsync();
        }


        public async Task<ThreadEntity> UpdateAsync(ThreadEntity threadEntity)
        {
            return await _threads.FindOneAndReplaceAsync<ThreadEntity>(x => x.Id == threadEntity.Id, threadEntity, DefaultFindAndReplaceOptions);
        }

        public async Task<ThreadEntity> SearchByGroupIdAsync(Guid groupId)
        {
            return await(await _threads.FindAsync(x => groupId == x.GroupId)).SingleOrDefaultAsync();
        }

        public async Task<ThreadEntity> SearchByTeamIdAsync(int teamId)
        {
            return await(await _threads.FindAsync(x => teamId == x.TeamId)).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ThreadEntity>> SearchByParticipantIdAsync(int participant)
        {
            return (await _threads.FindAsync(x => x.Participants.Contains(participant))).ToEnumerable();
        }

        public async Task DeleteByMultiIdAsync(Guid multiId)
        {
            await _threads.DeleteOneAsync(x => x.GroupId == multiId);
        }

        public async Task DeleteByParticipantIdsAsync(int userId, int participantId)
        {
            await _threads.DeleteOneAsync(x => x.Participants.Contains(userId) && x.Participants.Contains(participantId) && x.Type == ThreadType.User);
        }

        public async Task<ThreadEntity> GetByTypeAsync(ThreadType threadType)
        {
            return await (await _threads.FindAsync(x => x.Type == ThreadType.Broadcast)).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ThreadEntity>> GetAllThreadsAsync()
        {
            return (await _threads.FindAsync(x => true)).ToEnumerable();
        }
    }
}