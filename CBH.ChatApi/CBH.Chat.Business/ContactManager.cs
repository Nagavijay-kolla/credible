using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CBH.Chat.Domain.Core;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Infrastructure.Chat.Exception;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;

namespace CBH.Chat.Business
{
    public class ContactManager : IContactManager
    {
        private readonly IContactRepository _contactRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public ContactManager(IContactRepository contactRepository, IGroupRepository groupRepository, IMapper mapper)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _groupRepository= groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> AddContactAsync(int chatUserId, int contactUserId) => await _contactRepository.AddChatContactAsync(chatUserId, contactUserId);

        public async Task<bool> DeleteContactAsync(int chatUserId, int contactUserId) => await _contactRepository.DeleteContactAsync(chatUserId, contactUserId);

        public async Task<ContactUsersDto> GetContactsAsync(int userId)
        {
            var partnerListIds = await _contactRepository.GetPartnerIdsAsync(userId);
            var myChatContactListIds = await _contactRepository.GetMyChatContactIdsAsync(userId);
            var chatUsers = await _contactRepository.GetContactsAsync(userId, partnerListIds, myChatContactListIds);

            return new ContactUsersDto
            {
                UserId = userId,
                ContactUsers = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ContactUser>>(chatUsers).OrderBy(x => x.UserName)
            };
        }

        public async Task<ContactUsersDto> SearchContactsAsync(int chatUserId, string searchString) =>
            new ContactUsersDto
            {
                UserId = chatUserId,
                ContactUsers = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ContactUser>>(await _contactRepository.SearchContactsAsync(chatUserId, searchString)).OrderBy(x => x.UserName)
            };

        public async Task<ContactUsersDto> GetGroupContactsSearch(int chatUserId, Guid groupId)
        {
            var groupDetails = await _groupRepository.GetAsync(groupId);
            if (groupDetails == null)
            {
                throw new InvalidRequestException($"{ErrorConstants.InvalidInputMessage} - {groupId}");
            }
            var partnerListIds = await _contactRepository.GetPartnerIdsAsync(chatUserId);
            var chatUsers = await _contactRepository.GetContactsAsync(chatUserId, partnerListIds, groupDetails.Members);
            return new ContactUsersDto
            {
                UserId = chatUserId,
                ContactUsers = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ContactUser>>(chatUsers).OrderBy(x => x.UserName)
            };
        }

        public async Task<ContactUsersDto> GetMyContactsAsync(int chatUserId)
        {
            var myChatContactListIds = await _contactRepository.GetMyChatContactIdsAsync(chatUserId);
            if (myChatContactListIds == null || myChatContactListIds.Count == 0)
            {
                return null;
            }

            var chatUsers = await _contactRepository.GetMyChatContactsAsync(myChatContactListIds);
            return new ContactUsersDto
            {
                UserId = chatUserId,
                ContactUsers = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<ContactUser>>(chatUsers).OrderBy(x=>x.UserName)
            };
        }
    }
}