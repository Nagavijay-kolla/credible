using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.Core;

namespace CBH.Chat.Interfaces.Repository
{
    public interface IContactRepository
    {
        Task<ChatUser> GetChatUserAsync(int userId, int partnerId);
        Task<ICollection<ChatUser>> GetChatUsersAsync(ICollection<int> userIds, int partnerId);
        Task<ICollection<int>> GetPartnerIdsAsync(int chatUserId);
        Task<ICollection<int>> GetMyChatContactIdsAsync(int chatUserId);
        Task<ICollection<ChatUser>> GetContactsAsync(int chatUserId, ICollection<int> partnerListIds, ICollection<int> myChatContactIds);
        Task<ICollection<ChatUser>> GetContactsAsync(int chatUserId);
        Task<ICollection<ChatUser>> SearchContactsAsync(int chatUserId, string searchString);
        Task<ICollection<ChatUser>> GetMyChatContactsAsync(ICollection<int> myContactListIds);
        Task<bool> AddChatContactAsync(int chatUserId, int contactUserId);
        Task<bool> DeleteContactAsync(int chatUserId, int contactUserId);
        Task<ChatUser> GetChatUserDetailAsync(int userId);
        Task<IEnumerable<ChatUser>> GetChatUserDetailsAsync(IEnumerable<int> chatUserId);
        Task<IEnumerable<ChatUser>> GetChatUserDetailsFromUserIdsAsync(IEnumerable<int> userIDs);
        List<int> GetChatUsersForPartner(int partnerId);
    }
}
