using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Infrastructure.Chat.Exception;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;

namespace CBH.Chat.Business
{
    public class GroupManager : IGroupManager
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IThreadRepository _threadRepository;
        private readonly IMapper _mapper;
        private readonly IContactRepository _contactRepository;

        public GroupManager(IGroupRepository groupRepository, IThreadRepository threadRepository, IMapper mapper, IContactRepository contactRepository)
        {
            _groupRepository = groupRepository;
            _threadRepository = threadRepository;
            _mapper = mapper;
            _contactRepository = contactRepository;
        }

        public async Task<GroupResponseModel> AddUsersToGroupAsync(Guid groupId, IList<int> userIds)
        {
            var group = await _groupRepository.GetAsync(groupId);

            if (group == null)
            {
                throw new InvalidRequestException($"{ErrorConstants.InvalidInputMessage} - {groupId}");
            }

            var nonExistingUsers = userIds.Except(group.Members).ToList();
            var modifiedGroup = await _groupRepository.AppendUsersAsync(group.Id, nonExistingUsers);

            // add to its thread
            var groupThread = await _threadRepository.SearchByGroupIdAsync(group.Id);
            if (groupThread == null)
            {
                return _mapper.Map<GroupEntity, GroupResponseModel>(modifiedGroup);
            }

            groupThread.Participants = groupThread.Participants.Concat(nonExistingUsers).ToList();
            await _threadRepository.UpdateAsync(groupThread);
            return _mapper.Map<GroupEntity, GroupResponseModel>(modifiedGroup);
        }

        public async Task<GroupResponseModel> CreateNewGroupAsync(NewGroupRequestModel newGroup)
        {
            var newGroupEntity = _mapper.Map<NewGroupRequestModel, GroupEntity>(newGroup);
            newGroupEntity.Members = new List<int>
            {
                newGroup.CreatedUserId
            };
            return _mapper.Map<GroupEntity, GroupResponseModel>(await _groupRepository.CreateAsync(newGroupEntity));
        }

        public async Task<GroupResponseModel> DeleteGroupByIdAsync(Guid groupId)
        {
            var deletedGroup = await _groupRepository.DeleteAsync(groupId);

            // delete corresponding thread
            await _threadRepository.DeleteByMultiIdAsync(deletedGroup.Id);
            return _mapper.Map<GroupEntity, GroupResponseModel>(deletedGroup);
        }

        public async Task<GroupResponseModel> DeleteUsersFromGroupAsync(Guid groupId, IList<int> userIds)
        {
            var group = await _groupRepository.GetAsync(groupId);
            if (group == null)
            {
                throw new InvalidRequestException($"{ErrorConstants.InvalidInputMessage} - {groupId}");
            }

            var existingUsers = group.Members.Intersect(userIds).ToList();
            var modifiedGroup = await _groupRepository.RemoveUsersAsync(group.Id, existingUsers);

            // remove from its thread
            var groupThread = await _threadRepository.SearchByGroupIdAsync(group.Id);
            if (groupThread == null)
            {
                return _mapper.Map<GroupEntity, GroupResponseModel>(modifiedGroup);
            }

            groupThread.Participants = groupThread.Participants.Except(existingUsers).ToList();
            await _threadRepository.UpdateAsync(groupThread);
            return _mapper.Map<GroupEntity, GroupResponseModel>(modifiedGroup);
        }

        public async Task<GroupWithUsersResponseModel> FetchByIdAsync(Guid groupId)
        {
            var groupDetails = await _groupRepository.GetAsync(groupId);

            if (groupDetails == null)
            {
                throw new InvalidRequestException($"{ErrorConstants.InvalidInputMessage} - {groupId}");
            }

            var usersResponse = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(await _contactRepository.GetChatUserDetailsAsync(groupDetails.Members));
            var groupResponse = _mapper.Map<GroupEntity, GroupWithUsersResponseModel>(groupDetails);
            return _mapper.Map(usersResponse, groupResponse);
        }

        public async Task<IEnumerable<UserContactResponseModel>> FetchGroupUserByIdAsync(Guid groupId)
        {
            var groupDetails = await _groupRepository.GetAsync(groupId);

            if (groupDetails == null)
            {
                throw new InvalidRequestException($"{ErrorConstants.InvalidInputMessage} - {groupId}");
            }

            var usersResponse = _mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(await _contactRepository.GetChatUserDetailsAsync(groupDetails.Members));

            return usersResponse;
        }

        public async Task<IEnumerable<GroupResponseModel>> FetchByUserIdAsync(int userId)
        {
            return _mapper.Map<IEnumerable<GroupEntity>, IEnumerable<GroupResponseModel>>(await _groupRepository.GetByUserIdAsync(userId));
        }
    }
}