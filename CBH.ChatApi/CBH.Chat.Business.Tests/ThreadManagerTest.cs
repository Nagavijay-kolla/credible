using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CBH.Chat.Business.Mappings;
using CBH.Chat.Business.Tests.DataHelpers;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Enumerations;
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
    public class ThreadManagerTest
    {
        private readonly Random _rnd = new Random();

        #region GetThreadByUserId

        [Fact]
        public async Task GetThreadByUserIdTest_WhenUserIdExists_ReturnsUserThreadResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var mapper = GetMapperForThreadProfile();
            var threadEntity = EntityModellers.CreateThreadEntity();
            var userId = _rnd.Next(111, 1000);
            var groupId = Guid.NewGuid();
            const int defaultLimit = -1;
            threadEntity.Participants.Add(userId);
            threadEntity.GroupId = groupId;
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            chatUserEntity.ChatUserId = userId;
            var chatUserEntityList = new List<ChatUser> { chatUserEntity };

            contactMockRepository.Setup(x => x.GetChatUserDetailsAsync(new List<int> { userId })).ReturnsAsync(chatUserEntityList);
            var participantDetails = mapper.Map<IEnumerable<IEnumerable<ChatUser>>, IEnumerable<IEnumerable<UserContactResponseModel>>>(new List<List<ChatUser>> { chatUserEntityList });
            var expected = mapper.Map<IEnumerable<ThreadEntity>, IEnumerable<ThreadWithContactsResponseModel>>(new List<ThreadEntity> { threadEntity }).Zip(participantDetails, ZipContactsWithThread);
            threadMockRepository.Setup(x => x.SearchByParticipantIdAsync(userId)).ReturnsAsync(new List<ThreadEntity> { threadEntity });


            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.GetThreadsByUserId(userId, defaultLimit, FetchThreadType.All);

            Assert.Equal(expected.Count(), actual.Count());
            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
            teamMockRepository.VerifyAll();
        }

        private static ThreadWithContactsResponseModel ZipContactsWithThread(ThreadWithContactsResponseModel thread, IEnumerable<UserContactResponseModel> contacts)
        {
            thread.Participants = contacts.ToList();
            return thread;
        }

        [Fact]
        public async Task GetThreadByUserIdTest_WhenUserIdDoesntExists_ReturnsEmptyThreadResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();

            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var mapper = GetMapperForThreadProfile();
            var threadEntity = EntityModellers.CreateThreadEntity();
            const int defaultLimit = -1;
            var userId = _rnd.Next(111, 1000);
            var groupId = Guid.NewGuid();

            threadEntity.GroupId = groupId;


            var expected = mapper.Map<IEnumerable<ThreadEntity>, IEnumerable<ThreadWithContactsResponseModel>>(new List<ThreadEntity>());
            threadMockRepository.Setup(x => x.SearchByParticipantIdAsync(userId)).ReturnsAsync((IEnumerable<ThreadEntity>)null);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.GetThreadsByUserId(userId, defaultLimit, FetchThreadType.UnArchived);

            Assert.Equal(expected, actual, new LogicEqualityComparer<IEnumerable<ThreadWithContactsResponseModel>>());

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
        }

        #endregion

        #region ArchiveThreadyByIdAsync

        [Fact]
        public async Task ArchiveThreadyByIdAsyncTest_WhenThreadIdExists_Returns()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var mapper = GetMapperForThreadProfile();
            var threadEntity = EntityModellers.CreateThreadEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();

            threadMockRepository.Setup(x => x.GetAsync(threadEntity.Id)).ReturnsAsync(threadEntity);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            await threadManager.ArchiveThreadyByIdAsync(chatUserEntity.ChatUserId, threadEntity.Id, true);

            threadMockRepository.Verify(x => x.UpdateAsync(threadEntity), Times.Once);

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
        }

        [Fact]
        public async Task ArchiveThreadyByIdAsyncTest_WhenThreadIdDoesntExists_ThrowsInvalidRequestException()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var mapper = GetMapperForThreadProfile();
            var threadEntity = EntityModellers.CreateThreadEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();

            threadMockRepository.Setup(x => x.GetAsync(threadEntity.Id)).ReturnsAsync((ThreadEntity)null);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => threadManager.ArchiveThreadyByIdAsync(chatUserEntity.ChatUserId, threadEntity.Id, true));

            Assert.IsType<InvalidRequestException>(exception);

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
        }

        #endregion

        #region SearchOrCreateThreadAsync

        [Fact]
        public async Task SearchOrCreateThreadAsyncTest_WhenThreadTypeIsUserAndParticipantGuidExist_ReturnsThreadWithMessagesResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var userId = _rnd.Next(111, 1000);
            var participantGuid = _rnd.Next(111, 1000);
            const ThreadType type = ThreadType.User;

            var mapper = GetMapperForThreadProfile();
            var threadEntity = EntityModellers.CreateThreadEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var messageEntity = EntityModellers.CreateMessageEntity();
            var chatUserEntityList = new List<ChatUser> { chatUserEntity };

            contactMockRepository.Setup(x => x.GetChatUserDetailAsync(participantGuid)).ReturnsAsync(chatUserEntity);
            contactMockRepository.Setup(x => x.GetChatUserDetailsAsync(threadEntity.Participants)).ReturnsAsync(chatUserEntityList);

            threadEntity.Participants.Add(participantGuid);
            threadMockRepository.Setup(x => x.SearchByParticipantIdsAsync(userId, participantGuid)).ReturnsAsync(threadEntity);
            messageMockRepository.Setup(x => x.GetByThreadIdAsync(threadEntity.Id)).ReturnsAsync(new List<MessageEntity> { messageEntity });

            var participantResponse = mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(new List<ChatUser> { chatUserEntity });
            var messagesResponse = mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(new List<MessageEntity> { messageEntity });
            var threadResponse = mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(threadEntity);
            var threadWithMessages = mapper.Map(messagesResponse, threadResponse);
            var expected = mapper.Map(participantResponse, threadWithMessages);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.SearchOrCreateThreadAsync(userId, participantGuid.ToString(), type);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ThreadWithMessagesResponseModel>());
            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            teamMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
        }

        [Fact]
        public async Task SearchOrCreateThreadAsyncTest_WhenThreadTypeIsUserAndParticipantGuidDoesntExist_ThrowsInvalidRequestException()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var mapper = GetMapperForThreadProfile();

            var userId = _rnd.Next(111, 1000);
            var participantGuid = _rnd.Next(111, 1000);
            const ThreadType type = ThreadType.User;

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => threadManager.SearchOrCreateThreadAsync(userId, participantGuid.ToString(), type));

            Assert.IsType<InvalidRequestException>(exception);

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
        }

        [Fact]
        public async Task SearchOrCreateThreadAsyncTest_WhenThreadTypeIsGroupAndParticipantGuidExist_ThreadWithMessagesResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var userId = _rnd.Next(111, 1000);
            var participantGuid = Guid.NewGuid();
            const ThreadType type = ThreadType.Group;

            var mapper = GetMapperForThreadProfile();
            var threadEntity = EntityModellers.CreateThreadEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            chatUserEntity.ChatUserId = userId;
            var messageEntity = EntityModellers.CreateMessageEntity();
            var groupEntity = EntityModellers.CreateGroupEntity();
            groupEntity.Id = participantGuid;
            groupEntity.Members.Add(userId);
            threadEntity.Participants = new List<int> { userId };
            var chatUserEntityList = new List<ChatUser> { chatUserEntity };

            groupMockRepository.Setup(x => x.GetAsync(groupEntity.Id)).ReturnsAsync(groupEntity);
            threadMockRepository.Setup(x => x.SearchByGroupIdAsync(groupEntity.Id)).ReturnsAsync(threadEntity);
            messageMockRepository.Setup(x => x.GetByThreadIdAsync(threadEntity.Id)).ReturnsAsync(new List<MessageEntity> { messageEntity });
            contactMockRepository.Setup(x => x.GetChatUserDetailsAsync(new List<int> { userId })).ReturnsAsync(chatUserEntityList);

            var participantResponse = mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(new List<ChatUser> { chatUserEntity });
            var messagesResponse = mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(new List<MessageEntity> { messageEntity });
            var threadResponse = mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(threadEntity);
            var threadWithMessages = mapper.Map(messagesResponse, threadResponse);
            var expected = mapper.Map(participantResponse, threadWithMessages);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.SearchOrCreateThreadAsync(userId, participantGuid.ToString(), type);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ThreadWithMessagesResponseModel>());
            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
        }

        [Fact]
        public async Task SearchOrCreateThreadAsyncTest_WhenThreadTypeIsGroupAndGroupDoesNotExist_ThrowsInvalidRequestException()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var mapper = GetMapperForThreadProfile();

            var userId = _rnd.Next(111, 1000);
            var participantGuid = Guid.NewGuid();
            var type = ThreadType.Group;

            groupMockRepository.Setup(x => x.GetAsync(participantGuid)).ReturnsAsync((GroupEntity)null);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => threadManager.SearchOrCreateThreadAsync(userId, participantGuid.ToString(), type));

            Assert.IsType<InvalidRequestException>(exception);
            Assert.StartsWith(ErrorConstants.InvalidParticipantId, exception.Message);

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
        }

        [Fact]
        public async Task SearchOrCreateThreadAsyncTest_WhenThreadTypeIsTeamAndParticipantGuidExist_ThreadWithMessagesResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var mapper = GetMapperForThreadProfile();
            var threadEntity = EntityModellers.CreateThreadEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var messageEntity = EntityModellers.CreateMessageEntity();
            var groupEntity = EntityModellers.CreateGroupEntity();
            var teamEntity = EntityModellers.CreateTeamEntity();

            var userId = _rnd.Next(111, 1000);
            var participantGuid = Guid.NewGuid();
            const ThreadType type = ThreadType.Team;
            var teamMembers = new List<int> { teamEntity.TeamId };
            var chatUsers = new List<ChatUser> { chatUserEntity };
            teamEntity.Id = participantGuid;
            groupEntity.Id = participantGuid;

            threadEntity.Participants = new List<int> { userId };

            teamMockRepository.Setup(x => x.GetAsync(teamEntity.Id)).ReturnsAsync(teamEntity);
            partnerMockRepository.Setup(x => x.GetTeamMembers(teamEntity.TeamId)).Returns(teamMembers);
            threadMockRepository.Setup(x => x.GetAsync(participantGuid)).ReturnsAsync(threadEntity);
            messageMockRepository.Setup(x => x.GetByThreadIdAsync(threadEntity.Id)).ReturnsAsync(new List<MessageEntity> { messageEntity });
            contactMockRepository.Setup(x => x.GetChatUserDetailsFromUserIdsAsync(teamMembers)).ReturnsAsync(chatUsers);
            contactMockRepository.Setup(x => x.GetChatUserDetailsAsync(It.Is<IEnumerable<int>>(l => new CompareLogic().Compare(l, threadEntity.Participants).AreEqual)))
                .ReturnsAsync(chatUsers);

            var participantResponse = mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(new List<ChatUser> { chatUserEntity });
            var messagesResponse = mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(new List<MessageEntity> { messageEntity });
            var threadResponse = mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(threadEntity);
            var threadWithMessages = mapper.Map(messagesResponse, threadResponse);
            var expected = mapper.Map(participantResponse, threadWithMessages);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.SearchOrCreateThreadAsync(userId, participantGuid.ToString(), type);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ThreadWithMessagesResponseModel>());

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
        }

        [Fact]
        public async Task SearchOrCreateThreadAsyncTest_WhenThreadTypeIsTeamAndParticipantGuidExist_ThrowsInvalidRequestException()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var mapper = GetMapperForThreadProfile();

            var userId = _rnd.Next(111, 1000);
            var participantGuid = Guid.NewGuid();
            const ThreadType type = ThreadType.Team;

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => threadManager.SearchOrCreateThreadAsync(userId, participantGuid.ToString(), type));

            Assert.IsType<InvalidRequestException>(exception);

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
        }

        #endregion

        #region AddNewMessageAsync

        [Fact]
        public async Task AddNewMessageAsyncTest_WhenUserThreadIdExists_ReturnsThreadWithMessagesResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();
            var mapper = GetMapperForThreadProfile();

            var threadEntity = EntityModellers.CreateThreadEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var newMessageRequestModel = EntityModellers.CreateNewMessageRequestModel();

            var tempMessageEntity = new MessageEntity
            {
                Id = Guid.NewGuid(),
                ThreadId = threadEntity.Id,
                Content = newMessageRequestModel.Content,
                FromUserId = newMessageRequestModel.FromUserId,
                FromUserName = newMessageRequestModel.FromUserName,
                CreatedAt = DateTime.UtcNow
            };

            var tempMessages = new List<MessageEntity>
            {
                tempMessageEntity,
                new MessageEntity
                {
                    Id = Guid.NewGuid(),
                    ThreadId = threadEntity.Id,
                    Content = "content",
                    FromUserId = newMessageRequestModel.FromUserId,
                    FromUserName = newMessageRequestModel.FromUserName,
                    CreatedAt = DateTime.UtcNow
                }
            };

            var lstUserEntity = new List<ChatUser> { chatUserEntity };

            threadMockRepository.Setup(x => x.GetAsync(newMessageRequestModel.ThreadId)).ReturnsAsync(threadEntity);
            messageMockRepository.Setup(x => x.AddAndGetAsync(It.IsAny<MessageEntity>())).ReturnsAsync(tempMessages);
            contactMockRepository.Setup(x => x.GetChatUserDetailsAsync(threadEntity.Participants)).ReturnsAsync(lstUserEntity);
            var messagesResponse = mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(tempMessages);

            var participantResponse = mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(lstUserEntity);
            var threadResponse = mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(threadEntity);
            var threadWithMessages = mapper.Map(messagesResponse, threadResponse);
            var expected = mapper.Map(participantResponse, threadWithMessages);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.AddNewMessageAsync(newMessageRequestModel);
            expected.ModifiedAt = actual.ModifiedAt;

            Assert.Equal(expected, actual, new LogicEqualityComparer<ThreadWithMessagesResponseModel>());

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
            teamMockRepository.VerifyAll();
        }

        #endregion

        #region GetThreadById

        [Fact]
        public async Task GetThreadByIdTest_WhenThreadIdExists_ReturnsThreadWithMessagesResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();
            var mapper = GetMapperForThreadProfile();

            var threadEntity = EntityModellers.CreateThreadEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var lstUserEntity = new List<ChatUser> { chatUserEntity };
            var messageEntity = EntityModellers.CreateMessageEntity();
            var lstMessageEntity = new List<MessageEntity> { messageEntity };

            threadMockRepository.Setup(x => x.GetAsync(threadEntity.Id)).ReturnsAsync(threadEntity);
            contactMockRepository.Setup(x => x.GetChatUserDetailsAsync(threadEntity.Participants)).ReturnsAsync(new List<ChatUser> { chatUserEntity });
            messageMockRepository.Setup(x => x.GetByThreadIdAsync(threadEntity.Id)).ReturnsAsync(lstMessageEntity);

            var participantDetails = mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(lstUserEntity);
            var messages = mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(lstMessageEntity);
            var threads = mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(threadEntity);
            var expected = mapper.Map(participantDetails, mapper.Map(messages, threads));
            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.GetThreadById(threadEntity.Id);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ThreadWithMessagesResponseModel>());

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetThreadByIdTest_WhenThreadIdDoesntExists_ThrowsInvalidRequestException()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();
            var mapper = GetMapperForThreadProfile();

            var threadId = Guid.NewGuid();
            threadMockRepository.Setup(x => x.GetAsync(threadId)).ReturnsAsync((ThreadEntity)null);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => threadManager.GetThreadById(threadId));

            Assert.IsType<InvalidRequestException>(exception);

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetThreadByIdTest_WhenThreadIdAndUserDoesntExists_ReturnsThreadWithMessagesResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();
            var mapper = GetMapperForThreadProfile();

            var threadEntity = EntityModellers.CreateThreadEntity();
            var messageEntity = EntityModellers.CreateMessageEntity();
            var lstMessageEntity = new List<MessageEntity> { messageEntity };

            messageMockRepository.Setup(x => x.GetByThreadIdAsync(threadEntity.Id)).ReturnsAsync(lstMessageEntity);

            var participantDetails = mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(new List<ChatUser>());
            var messages = mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(lstMessageEntity);
            var threads = mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(threadEntity);
            threadMockRepository.Setup(x => x.GetAsync(threadEntity.Id)).ReturnsAsync(threadEntity);
            var expected = mapper.Map(participantDetails, mapper.Map(messages, threads));

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.GetThreadById(threadEntity.Id);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ThreadWithMessagesResponseModel>());

            messageMockRepository.Verify(x => x.GetByThreadIdAsync(threadEntity.Id), Times.Once);

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
        }

        #endregion

        #region SearchOrCreateBroadcastThreadAsync
        [Fact]
        public async Task SearchOrCreateBroadcastThreadAsyncTest_WhenThreadDoesntExists_ReturnsThreadWithMessagesResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();
            var mapper = GetMapperForThreadProfile();

            const ThreadType type = ThreadType.Broadcast;
            var threadEntity = new ThreadEntity();

            var participantDetails = mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(new List<ChatUser>());
            var messages = mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(new List<MessageEntity>());
            var threadWithMessages = mapper.Map(messages, mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(threadEntity));
            var expected = mapper.Map(participantDetails, threadWithMessages);

            threadMockRepository.Setup(x => x.GetByTypeAsync(type)).ReturnsAsync(new ThreadEntity());
            messageMockRepository.Setup(x => x.GetByThreadIdAsync(threadEntity.Id)).ReturnsAsync(new List<MessageEntity>());
            contactMockRepository.Setup(x => x.GetChatUserDetailsAsync(threadEntity.Participants)).ReturnsAsync(new List<ChatUser>());

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.SearchOrCreateBroadcastThreadAsync();

            Assert.Equal(expected, actual, new LogicEqualityComparer<ThreadWithMessagesResponseModel>());

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
            teamMockRepository.VerifyAll();
        }

        [Fact]
        public async Task SearchOrCreateBroadcastThreadAsyncTest_WhenThreadExists_ReturnsThreadWithMessagesResponseModel()
        {
            var threadMockRepository = new Mock<IThreadRepository>();
            var userMockRepository = new Mock<IUserRepository>();
            var messageMockRepository = new Mock<IMessageRepository>();
            var groupMockRepository = new Mock<IGroupRepository>();
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var contactMockRepository = new Mock<IContactRepository>();
            var logMockRepository = new Mock<ILogRepository>();
            var teamMockRepository = new Mock<ITeamRepository>();

            var mapper = GetMapperForThreadProfile();

            var userId = _rnd.Next(111, 1000);
            var type = ThreadType.Broadcast;
            var threadEntity = EntityModellers.CreateThreadEntity();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var lstUserEntity = new List<ChatUser> { chatUserEntity };
            var messageEntity = EntityModellers.CreateMessageEntity();
            var lstMessageEntity = new List<MessageEntity> { messageEntity };
            threadEntity.Participants.Add(userId);

            threadMockRepository.Setup(x => x.GetByTypeAsync(type)).ReturnsAsync(threadEntity);
            contactMockRepository.Setup(x => x.GetChatUserDetailsAsync(threadEntity.Participants)).ReturnsAsync(lstUserEntity);
            messageMockRepository.Setup(x => x.GetByThreadIdAsync(threadEntity.Id)).ReturnsAsync(lstMessageEntity);
            var participantDetails = mapper.Map<IEnumerable<ChatUser>, IEnumerable<UserContactResponseModel>>(lstUserEntity);
            var messages = mapper.Map<IEnumerable<MessageEntity>, IEnumerable<MessageResponseModel>>(lstMessageEntity);
            var threadWithMessages = mapper.Map(messages, mapper.Map<ThreadEntity, ThreadWithMessagesResponseModel>(threadEntity));
            var expected = mapper.Map(participantDetails, threadWithMessages);

            var threadManager = new ThreadManager(partnerMockRepository.Object, threadMockRepository.Object, messageMockRepository.Object, groupMockRepository.Object, mapper, contactMockRepository.Object, teamMockRepository.Object, logMockRepository.Object);
            var actual = await threadManager.SearchOrCreateBroadcastThreadAsync();

            Assert.Equal(expected, actual, new LogicEqualityComparer<ThreadWithMessagesResponseModel>());

            threadMockRepository.VerifyAll();
            userMockRepository.VerifyAll();
            messageMockRepository.VerifyAll();
            groupMockRepository.VerifyAll();
            partnerMockRepository.VerifyAll();
            teamMockRepository.VerifyAll();
        }

        #endregion

        #region MapperConfiguration

        private static IMapper GetMapperForThreadProfile() => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ThreadProfile());
            cfg.AddProfile(new MessageProfile());
        }).CreateMapper();

        #endregion
    }
}