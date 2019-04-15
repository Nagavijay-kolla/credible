using System;
using System.Threading.Tasks;
using AutoMapper;
using CBH.Chat.Business.Tests.DataHelpers;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Interfaces.Repository;
using Moq;
using Xunit;

namespace CBH.Chat.Business.Tests
{
    public class UserManagerTest
    {
        private readonly Random _rnd = new Random();

        #region UpdateUserStatusAsync

        [Fact]
        public async Task UpdateUserStatusAsyncTest_WhenUserIdExistsAndStatusIsAvailable_ReturnsUserResponseModelWithUpdatedStatus()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var mapper = new Mock<IMapper>();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var status = UserStatus.Available;
            var userId = _rnd.Next(111, 1000);
            mockUserRepository.Setup(x => x.UpdateChatStatusAsync(userId, status.ToString())).ReturnsAsync(true);
            var expected = true;

            //Action
            var userManager = new UserManager(mockUserRepository.Object, mockThreadRepository.Object, mapper.Object);
            var actual = await userManager.UpdateUserStatusAsync(userId, status);

            //Assert
            Assert.Equal(expected, actual);
            mockUserRepository.VerifyAll();
        }

        [Fact]
        public async Task UpdateUserStatusAsyncTest_WhenUserIdDoesntExistsAndStatusIsAvailable_ReturnsUserResponseModelWithUpdatedStatus()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            var mockThreadRepository = new Mock<IThreadRepository>();
            var mapper = new Mock<IMapper>();
            var chatUserEntity = EntityModellers.CreateChatUserEntity();
            var status = UserStatus.Available;
            var userId = _rnd.Next(111, 1000);
            mockUserRepository.Setup(x => x.UpdateChatStatusAsync(userId, status.ToString())).ReturnsAsync(false);
            var expected = false;

            //Action
            var userManager = new UserManager(mockUserRepository.Object, mockThreadRepository.Object, mapper.Object);
            var actual = await userManager.UpdateUserStatusAsync(userId, status);

            //Assert
            Assert.Equal(expected, actual);
            mockUserRepository.VerifyAll();
        }       

        #endregion
    }
}
