using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBH.Chat.Domain.Core;
using CBH.Chat.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository.ChatRepository
{
    public class ContactRepository : IContactRepository
    {
        private readonly IChatUserDbContext _chatUserDbContext;

        public ContactRepository(IChatUserDbContext chatUserDbContext)
        {
            _chatUserDbContext = chatUserDbContext ?? throw new ArgumentNullException(nameof(chatUserDbContext));
        }

        public async Task<ChatUser> GetChatUserAsync(int userId, int partnerId)
        {
            return await _chatUserDbContext.ChatUsers.Where(x => x.UserId == userId && x.PartnerId == partnerId).SingleOrDefaultAsync();
        }
        public List<int> GetChatUsersForPartner(int partnerId)
        {
            return _chatUserDbContext.ChatUsers.Where(x => x.PartnerId == partnerId).Select(x=> x.ChatUserId).ToList();
        }

        public async Task<ICollection<ChatUser>> GetChatUsersAsync(ICollection<int> userIds, int partnerId)
        {
            if (userIds == null)
            {
                throw new ArgumentNullException(nameof(userIds));
            }

            return await _chatUserDbContext.ChatUsers.Where(x => x.PartnerId == partnerId && userIds.Contains(x.UserId)).ToListAsync();
        }

        public async Task<ICollection<int>> GetPartnerIdsAsync(int chatUserId)
        {
            return await _chatUserDbContext.UserPartners.Where(x => x.UserId == chatUserId).Select(x => x.PartnerId).ToListAsync();
        }

        public async Task<ICollection<int>> GetMyChatContactIdsAsync(int chatUserId)
        {
            return await _chatUserDbContext.UserContacts.Where(x => x.UserId == chatUserId).Select(x => x.ContactUserId).ToListAsync();
        }

        public async Task<ICollection<ChatUser>> GetContactsAsync(int chatUserId, ICollection<int> partnerListIds, ICollection<int> myChatContactIds)
        {
            return await _chatUserDbContext.ChatUsers.Where(x => partnerListIds.Contains(x.PartnerId) && x.ChatUserId != chatUserId && !myChatContactIds.Contains(x.ChatUserId)).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToListAsync();
        }

        public async Task<ICollection<ChatUser>> SearchContactsAsync(int chatUserId, string searchString)
        {
            var partnerIds = await GetPartnerIdsAsync(chatUserId);
            var excludeMyContactIds = await GetMyChatContactIdsAsync(chatUserId);
            return await _chatUserDbContext.ChatUsers
                        .Where(x => partnerIds.Contains(x.PartnerId)
                            && x.ChatUserId != chatUserId
                            && !excludeMyContactIds.Contains(x.ChatUserId)
                            && (x.LastName.Contains(searchString) || x.FirstName.Contains(searchString)))
                        .OrderBy(x => x.LastName)
                        .ThenBy(x => x.FirstName)
                        .ToListAsync();
        }

 

        public async Task<ICollection<ChatUser>> GetContactsAsync(int chatUserId)
        {
            return await GetMyChatContactsAsync(await GetMyChatContactIdsAsync(chatUserId));
        }

        public async Task<ICollection<ChatUser>> GetMyChatContactsAsync(ICollection<int> myContactListIds)
        {
            return await _chatUserDbContext.ChatUsers.Where(x => myContactListIds.Contains(x.ChatUserId)).ToListAsync();
        }

        public async Task<bool> AddChatContactAsync(int chatUserId, int contactUserId)
        {
            var contact = await _chatUserDbContext.UserContacts.Where(x => x.UserId == chatUserId && x.ContactUserId == contactUserId).SingleOrDefaultAsync();
            if (contact != null)
            {
                return true;
            }

            _chatUserDbContext.UserContacts.Add(new UserContact
            {
                UserId = chatUserId,
                ContactUserId = contactUserId
            });
            return _chatUserDbContext.SaveChanges() > 0;
        }

        public async Task<bool> DeleteContactAsync(int chatUserId, int contactUserId)
        {
            var contact = await _chatUserDbContext.UserContacts.Where(x => x.UserId == chatUserId && x.ContactUserId == contactUserId).SingleOrDefaultAsync();

            if (contact == null)
            {
                return false;
            }

            _chatUserDbContext.UserContacts.Remove(contact);
            return _chatUserDbContext.SaveChanges() > 0;
        }

        public async Task<ChatUser> GetChatUserDetailAsync(int chatUserID)
        {
            return await _chatUserDbContext.ChatUsers.Where(x => x.ChatUserId == chatUserID).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ChatUser>> GetChatUserDetailsAsync(IEnumerable<int> chatUserId)
        {
            return await _chatUserDbContext.ChatUsers.Where(x => chatUserId.Contains(x.ChatUserId)).ToListAsync();
        }
        public async Task<IEnumerable<ChatUser>> GetChatUserDetailsFromUserIdsAsync(IEnumerable<int> userIDs)
        {
            return await _chatUserDbContext.ChatUsers.Where(x => userIDs.Contains(x.UserId)).ToListAsync();
        }
    }
}