using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CBH.Chat.Business.Mappings;
using CBH.Chat.Business.Tests.DataHelpers;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Infrastructure.Chat.Exception;
using CBH.Chat.Interfaces.Repository;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Xunit;

namespace CBH.Chat.Business.Tests
{
    public class GroupManagerTest
    {
        private readonly Random _rnd = new Random();

        #region AddUserToGroupAsync

        [Fact]
        public async Task AddUserToGroupAsyncTest_WhenGroupIdDoesntExists_ThrowsInvalidRequestException()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();

            var groupManager = new GroupManager(groupMockRepository.Object,  mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => groupManager.AddUsersToGroupAsync(Guid.NewGuid(), new List<int> { _rnd.Next(111, 1000) }));

            Assert.IsType<InvalidRequestException>(exception);
            Assert.StartsWith(ErrorConstants.InvalidInputMessage, exception.Message);

            groupMockRepository.VerifyAll();
        }

        [Fact]
        public async Task AddUserToGroupAsyncTest_WhenGroupIdAndUserIdInMembersExists_ReturnsGroupResponseModel()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();
            var groupEntity = EntityModellers.CreateGroupEntity();

            var userId = _rnd.Next(111, 1000);
            groupEntity.Members.Add(userId);
            groupMockRepository.Setup(x => x.GetAsync(groupEntity.Id)).ReturnsAsync(groupEntity);
            groupMockRepository.Setup(x => x.AppendUsersAsync(groupEntity.Id, It.Is<IList<int>>(y => true))).ReturnsAsync(groupEntity);
            var expected = mapper.Map<GroupEntity, GroupResponseModel>(groupEntity);

            var groupManager = new GroupManager(groupMockRepository.Object, mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.AddUsersToGroupAsync(groupEntity.Id, new List<int> { userId });

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupResponseModel>());
            Assert.Contains(userId, actual.Members);

            groupMockRepository.VerifyAll();
        }

        [Fact]
        public async Task AddUserToGroupAsyncTest_WhenUserIdExistsAndUserIdInMembersIsEmpty_ReturnsGroupResponseModel()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();

            var userId = _rnd.Next(111, 1000);
            var groupEntity = EntityModellers.CreateGroupEntity();
            groupEntity.Members.Add(userId);

            groupMockRepository.Setup(x => x.GetAsync(groupEntity.Id)).ReturnsAsync(groupEntity);
            groupMockRepository.Setup(x => x.AppendUsersAsync(groupEntity.Id, It.Is<IList<int>>(y => true))).ReturnsAsync(groupEntity);
            var expected = mapper.Map<GroupEntity, GroupResponseModel>(groupEntity);
            var newUsers = new List<int> { userId };
            var groupManager = new GroupManager(groupMockRepository.Object,  mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.AddUsersToGroupAsync(groupEntity.Id, newUsers);

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupResponseModel>());
            Assert.Contains(userId, actual.Members);

            groupMockRepository.Verify(x => x.AppendUsersAsync(groupEntity.Id, newUsers), Times.Never);

            groupMockRepository.VerifyAll();
        }

        #endregion

        #region CreateNewGroupAsync

        [Fact]
        public async Task CreateNewGroupAsyncTest_WhenMemberCreatedExists_ReturnsGroupResponseModel()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();
            var groupEntity = EntityModellers.CreateGroupEntity();
            var groupNewrequestModel = EntityModellers.CreateNewGroupRequestModel();

            var groupResponse = mapper.Map<NewGroupRequestModel, GroupEntity>(groupNewrequestModel);
            groupResponse.Members = new List<int> { groupNewrequestModel.CreatedUserId };

            groupMockRepository.Setup(x => x.CreateAsync(It.Is<GroupEntity>(y => y.Name == groupNewrequestModel.Name
            && y.CreatedUserId == groupNewrequestModel.CreatedUserId
            && y.CreatedUsername == groupNewrequestModel.CreatedUserName))).ReturnsAsync(groupResponse);

            var expected = mapper.Map<GroupEntity, GroupResponseModel>(groupResponse);

            var groupManager = new GroupManager(groupMockRepository.Object, mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.CreateNewGroupAsync(groupNewrequestModel);

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupResponseModel>());
        }

        #endregion

        #region DeleteUserFromGroupAsync

        [Fact]
        public async Task DeleteUserFromGroupAsyncTest_WhenGroupIdDoesntExists_ThrowsInvalidRequestException()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();

            var groupEntity = EntityModellers.CreateGroupEntity();

            groupMockRepository.Setup(x => x.GetAsync(groupEntity.Id)).ReturnsAsync((GroupEntity)null);

            var groupManager = new GroupManager(groupMockRepository.Object, mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => groupManager.DeleteUsersFromGroupAsync(groupEntity.Id, new List<int> { _rnd.Next(111, 1000) }));

            Assert.IsType<InvalidRequestException>(exception);
            Assert.StartsWith(ErrorConstants.InvalidInputMessage, exception.Message);

            groupMockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteUserFromGroupAsyncTest_WhenGroupIdAndUserIdInMembersExists_ReturnsGroupResponseModelWithOutUserIdInMembers()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();

            var userId = _rnd.Next(111, 1000);

            var groupEntity = EntityModellers.CreateGroupEntity();
            groupEntity.Members.Add(userId);

            var groupEntityWithoutUser = EntityModellers.CreateGroupEntity();
            var usersToRemove = new List<int> { userId };
            groupMockRepository.Setup(x => x.GetAsync(groupEntity.Id)).ReturnsAsync(groupEntity);
            groupMockRepository.Setup(x => x.RemoveUsersAsync(groupEntity.Id, usersToRemove)).ReturnsAsync(groupEntityWithoutUser);

            var expected = mapper.Map<GroupEntity, GroupResponseModel>(groupEntityWithoutUser);

            var groupManager = new GroupManager(groupMockRepository.Object, mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.DeleteUsersFromGroupAsync(groupEntity.Id, usersToRemove);

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupResponseModel>());
            Assert.DoesNotContain(userId, actual.Members);

            groupMockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteUserFromGroupAsyncTest_WhenGroupIdExistsAndUserIdInMembersDoesntExists_ReturnsGroupResponseModelWithOutUserIdInMemebers()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();

            var userId = _rnd.Next(111, 1000);
            var groupEntity = EntityModellers.CreateGroupEntity();
            var expected = mapper.Map<GroupEntity, GroupResponseModel>(groupEntity);
            var usersToRemove = new List<int> { userId };
            groupMockRepository.Setup(x => x.GetAsync(groupEntity.Id)).ReturnsAsync(groupEntity);
            groupMockRepository.Setup(x => x.RemoveUsersAsync(groupEntity.Id, It.Is<IList<int>>(y => true))).ReturnsAsync(groupEntity);
            var groupManager = new GroupManager(groupMockRepository.Object, mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.DeleteUsersFromGroupAsync(groupEntity.Id, usersToRemove);

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupResponseModel>());
            Assert.DoesNotContain(userId, actual.Members);

            groupMockRepository.Verify(x => x.RemoveUsersAsync(groupEntity.Id, usersToRemove), Times.Never);

            groupMockRepository.VerifyAll();
        }

        #endregion

        #region FetchByUserIdAsync

        [Fact]
        public async Task FetchByUserIdAsyncTest_WhenUserIdExists_ReturnsGroupResponseModelWithUserId()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();
            var groupEntity = EntityModellers.CreateGroupEntity();

            var userId = _rnd.Next(111,1000);
            groupEntity.Members.Add(userId);
            groupMockRepository.Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync(new List<GroupEntity> { groupEntity });
            var expected = mapper.Map<IEnumerable<GroupEntity>, IEnumerable<GroupResponseModel>>(new List<GroupEntity> { groupEntity });

            var groupManager = new GroupManager(groupMockRepository.Object, mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.FetchByUserIdAsync(userId);

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupResponseModel>());
        }

        [Fact]
        public async Task FetchByUserIdAsyncTest_WhenUserIdDoesntExists__ReturnsEmptyGroupResponseModel()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();
            var groupEntity = EntityModellers.CreateGroupEntity();

            var userId = _rnd.Next(111, 1000);
            var expected = mapper.Map<IEnumerable<GroupEntity>, IEnumerable<GroupResponseModel>>(new List<GroupEntity>());
            groupMockRepository.Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync((IEnumerable<GroupEntity>)null);

            var groupManager = new GroupManager(groupMockRepository.Object, mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.FetchByUserIdAsync(userId);

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupResponseModel>());
            Assert.Empty(actual);
        }

        #endregion

        #region FetchByIdAsync
        
        public async Task FetchByIdAsyncTest_WhenGroupIdExists_ReturnsGroupWithUsersResponseModel()
        {

            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();
            var groupEntity = EntityModellers.CreateGroupEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var chatUserEntityList = new List<ChatUser> { chatUserEntity };

            groupMockRepository.Setup(x => x.GetAsync(groupEntity.Id)).ReturnsAsync(groupEntity);
            contactMockRepository.Setup(x => x.GetChatUserDetailsAsync(groupEntity.Members)).ReturnsAsync(chatUserEntityList);

            var userResponse = mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(chatUserEntityList);
            var groupResponse = mapper.Map<GroupEntity, GroupWithUsersResponseModel>(groupEntity);

            var expected = mapper.Map(userResponse, groupResponse);

            var groupManager = new GroupManager(groupMockRepository.Object, mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.FetchByIdAsync(groupEntity.Id);

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupWithUsersResponseModel>());
            Assert.NotNull(actual);

            groupMockRepository.VerifyAll();
        }

        [Fact]
        public async Task FetchByIdAsyncTest_WhenGroupIdDoesntExists_ThrowsInvalidRequestException()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var mapper = GetMapperForGroupProfile();

            var groupId = Guid.NewGuid();

            groupMockRepository.Setup(x => x.GetAsync(groupId)).ReturnsAsync((GroupEntity)null);
            var groupManager = new GroupManager(groupMockRepository.Object,  mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => groupManager.FetchByIdAsync(groupId));

            Assert.IsType<InvalidRequestException>(exception);
            Assert.StartsWith(ErrorConstants.InvalidInputMessage, exception.Message);

            groupMockRepository.VerifyAll();
        }

        #endregion

        #region DeleteGroupByIdAsync

        [Fact]
        public async Task DeleteGroupByIdAsync_WhenGroupIdExists_ReturnsGroupResponseWithGroupDeleted()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();

            var mapper = GetMapperForGroupProfile();
            
            var groupEntity = EntityModellers.CreateGroupEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var threadEntity = EntityModellers.CreateThreadEntity();

            groupMockRepository.Setup(x => x.DeleteAsync(groupEntity.Id)).ReturnsAsync(groupEntity);            
            var expected = mapper.Map<GroupEntity, GroupResponseModel>(groupEntity);
            var groupManager = new GroupManager(groupMockRepository.Object, mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.DeleteGroupByIdAsync(groupEntity.Id);

            mockThreadRepository.Verify(x => x.DeleteByMultiIdAsync(groupEntity.Id), Times.Once);

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupResponseModel>());

            groupMockRepository.VerifyAll();
            mockThreadRepository.VerifyAll();

        }

        [Fact]
        public async Task DeleteGroupByIdAsync_WhenGroupIdDoesntExists_ReturnsGroupResponseWithGroupRemoved()
        {
            var groupMockRepository = new Mock<IGroupRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var contactMockRepository = new Mock<IContactRepository>();

            var mapper = GetMapperForGroupProfile();

            var groupEntity = EntityModellers.CreateGroupEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var threadEntity = EntityModellers.CreateThreadEntity();

            var expected = mapper.Map<GroupEntity, GroupResponseModel>(new GroupEntity());
            groupMockRepository.Setup(x => x.DeleteAsync(groupEntity.Id)).ReturnsAsync(new GroupEntity());
            var groupManager = new GroupManager(groupMockRepository.Object,  mockThreadRepository.Object, mapper, contactMockRepository.Object);
            var actual = await groupManager.DeleteGroupByIdAsync(groupEntity.Id);

            Assert.Equal(expected, actual, new LogicEqualityComparer<GroupResponseModel>());
            mockThreadRepository.Verify(x => x.DeleteByMultiIdAsync(groupEntity.Id), Times.Never);

            groupMockRepository.VerifyAll();
            mockThreadRepository.VerifyAll();
        }

        #endregion

        #region MapperConfiguration

        private static IMapper GetMapperForGroupProfile() => new MapperConfiguration(cfg => cfg.AddProfile(new GroupProfile())).CreateMapper();

        #endregion
    }
}