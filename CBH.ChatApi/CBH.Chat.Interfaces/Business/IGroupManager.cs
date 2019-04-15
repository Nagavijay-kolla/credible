using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;

namespace CBH.Chat.Interfaces.Business
{
    public interface IGroupManager
    {
        Task<GroupWithUsersResponseModel> FetchByIdAsync(Guid groupId);
        Task<GroupResponseModel> CreateNewGroupAsync(NewGroupRequestModel members);
        Task<GroupResponseModel> DeleteGroupByIdAsync(Guid groupId);
        Task<GroupResponseModel> AddUsersToGroupAsync(Guid groupId, IList<int> userId);
        Task<GroupResponseModel> DeleteUsersFromGroupAsync(Guid groupId, IList<int> userId);
        Task<IEnumerable<GroupResponseModel>> FetchByUserIdAsync(int userId);
        Task<IEnumerable<UserContactResponseModel>> FetchGroupUserByIdAsync(Guid groupId);
    }
}