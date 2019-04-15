using System;
using System.Linq;
using System.Threading.Tasks;
using CBH.Chat.Business.Tests.DataHelpers;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Interfaces.Repository;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Xunit;

namespace CBH.Chat.Business.Tests
{
    public class UserConfigurationManagerTest
    {
        private readonly Random _rnd = new Random();

        [Fact]
        public async Task GetUserChatConfigurationAsyncTest()
        {
            var mockPartnerRepository = new Mock<IPartnerRepository>();
            var mockContactRepository = new Mock<IContactRepository>();

            var userId = _rnd.Next(111, 1000);

            var user = EntityModellers.CreateUserEntity(userId);
            var partner = EntityModellers.CreatePartnerEntity();
            var chatUser = EntityModellers.CreateChatUserEntity();
            var userRights = EntityModellers.GetUserRights();
            var userRightsFor = new[] { "SendBroadcast" };

            mockPartnerRepository.Setup(x => x.GetUser(userId)).Returns(user);
            mockPartnerRepository.Setup(x => x.GetPartnerDetail()).Returns(partner);
            mockContactRepository.Setup(x => x.GetChatUserAsync(user.Id, partner.PartnerId)).ReturnsAsync(chatUser);
            mockPartnerRepository.Setup(x => x.GetPermissions(userId, userRights)).Returns(userRightsFor);

            var userConfiguration = new UserConfigurationDto
            {
                LoggedUserDetail = new LoggedUserDetailDto
                {
                    UserId = user.Id,
                    ChatUserId = chatUser.ChatUserId,
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserRole = user.UserRole,
                    IsEnableChat = user.IsEnableChat,
                    IsBroadcastEnable = userRightsFor.Contains("SendBroadcast"),
                    IsAdmin = user.ProfileCode.Equals("ADMIN"),
                    IsHighImportanceEnable = user.IsHighImportanceEnable,
                    IsChatAllTeams = userRightsFor.Contains("ChatAllTeams"),
                    IsEmployeeMessageLogViewAll = userRightsFor.Contains(""),
                    IsAppointmentArrivalMessage = userRightsFor.Contains(""),
                    IsSendEmpMessage = userRightsFor.Contains(""),
                    UserStatus = chatUser.Status ?? UserStatus.Available.ToString()
                },
                Partner = partner
            };

            var expected = userConfiguration;
            var chatServiceManager = new UserConfigurationManager(mockPartnerRepository.Object, mockContactRepository.Object);
            var actual = await chatServiceManager.GetUserChatConfigurationAsync(userId);

            Assert.Equal(expected, actual, new LogicEqualityComparer<UserConfigurationDto>());
        }
    }
}
