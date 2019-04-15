using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CBH.Chat.Business.Mappings;
using CBH.Chat.Domain.Core;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Interfaces.Repository;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Xunit;

namespace CBH.Chat.Business.Tests
{
    public class ContactManagerTest
    {
        private readonly Random _rnd = new Random();

        #region AddContactAsync

        [Fact]
        public async Task AddContactAsyncTest_WhenUserIdAndContactUserIdExists_ReturnsTrue()
        {
            var mockContactRepository = new Mock<IContactRepository>();
            var mockGroupRepository = new Mock<IGroupRepository>();
            var mapper = GetMapperForContactProfile();

            var userId = _rnd.Next(111, 1000);
            var contactUserId = _rnd.Next(111, 1000);
            mockContactRepository.Setup(x => x.AddChatContactAsync(userId, contactUserId)).ReturnsAsync(true);

            var expected = true;
            var contactManager = new ContactManager(mockContactRepository.Object, mockGroupRepository.Object, mapper);
            var actual = await contactManager.AddContactAsync(userId, contactUserId);

            Assert.Equal(expected, actual);
            mockContactRepository.VerifyAll();
        }

        [Fact]
        public async Task AddContactAsyncTest_WhenUserIdDoesntExistsAndContactUserIdExists_ReturnsFalse()
        {
            var mockContactRepository = new Mock<IContactRepository>();
            var mockGroupRepository = new Mock<IGroupRepository>();
            var mapper = GetMapperForContactProfile();

            var userId = _rnd.Next(111, 1000);
            var contactUserId = _rnd.Next(111, 1000);
            mockContactRepository.Setup(x => x.AddChatContactAsync(userId, contactUserId)).ReturnsAsync(false);

            var expected = false;
            var contactManager = new ContactManager(mockContactRepository.Object, mockGroupRepository.Object, mapper);
            var actual = await contactManager.AddContactAsync(userId, contactUserId);

            Assert.Equal(expected, actual);
            mockContactRepository.VerifyAll();
        }

        #endregion

        #region DeleteContactAsync

        [Fact]
        public async Task DeleteContactAsyncTest_WhenUserIdAndContactUserIdExists_ReturnsTrue()
        {
            var mockContactRepository = new Mock<IContactRepository>();
            var mockGroupRepository = new Mock<IGroupRepository>();
            var mapper = GetMapperForContactProfile();

            var userId = _rnd.Next(111, 1000);
            var contactUserId = _rnd.Next(111, 1000);
            mockContactRepository.Setup(x => x.DeleteContactAsync(userId, contactUserId)).ReturnsAsync(true);

            var expected = true;
            var contactManager = new ContactManager(mockContactRepository.Object, mockGroupRepository.Object, mapper);
            var actual = await contactManager.DeleteContactAsync(userId, contactUserId);

            Assert.Equal(expected, actual);
            mockContactRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteContactAsyncTest_WhenUserIdDoesntExistsAndContactUserIdExists_ReturnsFalse()
        {
            var mockContactRepository = new Mock<IContactRepository>();
            var mockGroupRepository = new Mock<IGroupRepository>();
            var mapper = GetMapperForContactProfile();

            var userId = _rnd.Next(111, 1000);
            var contactUserId = _rnd.Next(111, 1000);
            mockContactRepository.Setup(x => x.DeleteContactAsync(userId, contactUserId)).ReturnsAsync(false);

            var expected = false;
            var contactManager = new ContactManager(mockContactRepository.Object, mockGroupRepository.Object, mapper);
            var actual = await contactManager.DeleteContactAsync(userId, contactUserId);

            Assert.Equal(expected, actual);
            mockContactRepository.VerifyAll();
        }

        #endregion

        #region GetContactsAsync

        [Fact]
        public async Task GetContactsAsyncTest_WhenUserIdExistsAndContactUserIdExists_ReturnsContactUserResponse()
        {
            var mockContactRepository = new Mock<IContactRepository>();
            var mockGroupRepository = new Mock<IGroupRepository>();
            var mapper = GetMapperForContactProfile();

            var userId = _rnd.Next(111, 1000);
            var contactUserId = _rnd.Next(111, 1000);

            var partnerListIds = new List<int> { userId };
            var myChatContactListIds = new List<int> { contactUserId };
            var chatUserList = new ChatUser { UserId = userId, ChatUserId = contactUserId };
            var chatUserEntityList = new List<ChatUser> { chatUserList };

            mockContactRepository.Setup(x => x.GetPartnerIdsAsync(userId)).ReturnsAsync(partnerListIds);
            mockContactRepository.Setup(x => x.GetMyChatContactIdsAsync(userId)).ReturnsAsync(myChatContactListIds);
            mockContactRepository.Setup(x => x.GetContactsAsync(userId, partnerListIds, myChatContactListIds)).ReturnsAsync(chatUserEntityList);

            var contactUsers = mapper.Map<IEnumerable<ChatUser>, IEnumerable<ContactUser>>(chatUserEntityList).OrderBy(x => x.UserName);

            var expected = new ContactUsersDto { UserId = userId, ContactUsers = contactUsers };
            var contactManager = new ContactManager(mockContactRepository.Object, mockGroupRepository.Object, mapper);
            var actual = await contactManager.GetContactsAsync(userId);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ContactUsersDto>());
            mockContactRepository.VerifyAll();
        }

        #endregion

        #region SearchContactsAsync

        [Fact]
        public async Task SearchContactsAsyncTest_WhenUserIdExistsAndContactUserIdExists_ReturnsContactUserResponse()
        {
            var mockContactRepository = new Mock<IContactRepository>();
            var mockGroupRepository = new Mock<IGroupRepository>();
            var mapper = GetMapperForContactProfile();

            var userId = _rnd.Next(111, 1000);
            var contactUserId = _rnd.Next(111, 1000);

            var chatUserList = new ChatUser { UserId = userId, ChatUserId = contactUserId, FirstName = "User1" };
            var chatUserEntityList = new List<ChatUser> { chatUserList };
            const string searchString = "User1";

            mockContactRepository.Setup(x => x.SearchContactsAsync(userId, searchString)).ReturnsAsync(chatUserEntityList);

            var contactUsers = mapper.Map<IEnumerable<ChatUser>, IEnumerable<ContactUser>>(chatUserEntityList).OrderBy(x => x.UserName);

            var expected = new ContactUsersDto { UserId = userId, ContactUsers = contactUsers };
            var contactManager = new ContactManager(mockContactRepository.Object, mockGroupRepository.Object, mapper);
            var actual = await contactManager.SearchContactsAsync(userId, searchString);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ContactUsersDto>());
            mockContactRepository.VerifyAll();
        }

        #endregion

        #region GetMyContactsAsync

        [Fact]
        public async Task GetMyContactsAsyncTest_WhenUserIdExistsAndContactUserIdExists_ReturnsContactUserResponse()
        {
            var mockContactRepository = new Mock<IContactRepository>();
            var mockGroupRepository = new Mock<IGroupRepository>();
            var mapper = GetMapperForContactProfile();

            var userId = _rnd.Next(111, 1000);
            var contactUserId = _rnd.Next(111, 1000);

            var chatUserIdList = new List<int> { userId };
            var chatUserList = new ChatUser { UserId = userId, ChatUserId = contactUserId, FirstName = "User1" };
            var chatUserEntityList = new List<ChatUser> { chatUserList };

            mockContactRepository.Setup(x => x.GetMyChatContactIdsAsync(userId)).ReturnsAsync(chatUserIdList);
            mockContactRepository.Setup(x => x.GetMyChatContactsAsync(chatUserIdList)).ReturnsAsync(chatUserEntityList);

            var contactUsers = mapper.Map<IEnumerable<ChatUser>, IEnumerable<ContactUser>>(chatUserEntityList).OrderBy(x => x.UserName);

            var expected = new ContactUsersDto { UserId = userId, ContactUsers = contactUsers };
            var contactManager = new ContactManager(mockContactRepository.Object, mockGroupRepository.Object, mapper);
            var actual = await contactManager.GetMyContactsAsync(userId);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ContactUsersDto>());
            mockContactRepository.VerifyAll();
        }

        [Fact]
        public async Task GetMyContactsAsyncTest_WhenUserIdDoesntExistsAndContactUserIdExists_ReturnsNull()
        {
            var mockContactRepository = new Mock<IContactRepository>();
            var mockGroupRepository = new Mock<IGroupRepository>();
            var mapper = GetMapperForContactProfile();

            var userId = _rnd.Next(111, 1000);
            var contactUserId = _rnd.Next(111, 1000);

            var chatUserList = new ChatUser { UserId = userId, ChatUserId = contactUserId, FirstName = "User1" };

            mockContactRepository.Setup(x => x.GetMyChatContactIdsAsync(userId)).ReturnsAsync((ICollection<int>)null);

            var expected = (ContactUsersDto)null;
            var contactManager = new ContactManager(mockContactRepository.Object, mockGroupRepository.Object, mapper);
            var actual = await contactManager.GetMyContactsAsync(userId);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ContactUsersDto>());
            mockContactRepository.VerifyAll();
        }

        [Fact]
        public async Task GetMyContactsAsyncTest_WhenUserIdDoesntExistsAndContactUserIdDoesntExists_ReturnsNull()
        {
            var mockContactRepository = new Mock<IContactRepository>();
            var mockGroupRepository = new Mock<IGroupRepository>();
            var mapper = GetMapperForContactProfile();

            var userId = _rnd.Next(111, 1000);

            mockContactRepository.Setup(x => x.GetMyChatContactIdsAsync(userId)).ReturnsAsync(new List<int>());

            var expected = (ContactUsersDto)null;
            var contactManager = new ContactManager(mockContactRepository.Object, mockGroupRepository.Object, mapper);
            var actual = await contactManager.GetMyContactsAsync(userId);

            Assert.Equal(expected, actual, new LogicEqualityComparer<ContactUsersDto>());
            mockContactRepository.VerifyAll();
        }

        #endregion

        #region MapperConfiguration

        private static IMapper GetMapperForContactProfile() => new MapperConfiguration(cfg => cfg.AddProfile(new ContactProfile())).CreateMapper();

        #endregion
    }
}
