using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Interfaces.Repository;
using MongoDB.Driver;

namespace CBH.Chat.Repository.ChatRepository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IMongoCollection<GroupEntity> _groups;
        private static readonly FindOptions<GroupEntity, GroupEntity> OrderByNameFindOptions = new FindOptions<GroupEntity, GroupEntity>
        {
            Sort = Builders<GroupEntity>.Sort.Ascending(x => x.Name)
        };

        public GroupRepository(IChatDbClient dbClient)
        {
            _groups = dbClient.Database.GetCollection<GroupEntity>(DbConstants.GroupCollection);
        }

        public async Task<GroupEntity> CreateAsync(GroupEntity newGroupEntity)
        {
            await _groups.InsertOneAsync(newGroupEntity);
            return newGroupEntity;
        }

        public async Task<GroupEntity> GetAsync(Guid groupId)
        {
            return await (await _groups.FindAsync(x => x.Id == groupId)).SingleOrDefaultAsync();
        }

        public async Task<GroupEntity> DeleteAsync(Guid groupId)
        {
            return await _groups.FindOneAndDeleteAsync(x => x.Id == groupId);
        }

        public async Task<GroupEntity> AppendUsersAsync(Guid groupId, IList<int> userIds)
        {
            await _groups.FindOneAndUpdateAsync(x => x.Id == groupId, Builders<GroupEntity>.Update.PushEach(x => x.Members, userIds));
            return await (await _groups.FindAsync(x => x.Id == groupId)).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<GroupEntity>> GetByUserIdAsync(int userId)
        {
            return (await _groups.FindAsync(x => x.Members.Contains(userId), OrderByNameFindOptions)).ToEnumerable();
        }

        public async Task<GroupEntity> RemoveUsersAsync(Guid groupId, IList<int> userIds)
        {
            await _groups.FindOneAndUpdateAsync(x => x.Id == groupId, Builders<GroupEntity>.Update.PullAll(x => x.Members, userIds));
            return await (await _groups.FindAsync(x => x.Id == groupId)).SingleOrDefaultAsync();
        }
    }
}