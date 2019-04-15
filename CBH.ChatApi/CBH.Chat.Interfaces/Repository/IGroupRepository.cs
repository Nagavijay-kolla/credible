using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Entity;

namespace CBH.Chat.Interfaces.Repository
{
    public interface IGroupRepository
    {
        Task<GroupEntity> GetAsync(Guid groupId);
        Task<IEnumerable<GroupEntity>> GetByUserIdAsync(int userId);
        Task<GroupEntity> CreateAsync(GroupEntity newGroupEntity);
        Task<GroupEntity> DeleteAsync(Guid groupId);
        Task<GroupEntity> AppendUsersAsync(Guid groupId, IList<int> userIds);
        Task<GroupEntity> RemoveUsersAsync(Guid groupId, IList<int> userIds);
    }
}