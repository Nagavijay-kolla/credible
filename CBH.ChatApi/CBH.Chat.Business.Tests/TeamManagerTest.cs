using System;
using System.Collections.Generic;
using AutoMapper;
using CBH.Chat.Business.Mappings;
using CBH.Chat.Business.Tests.DataHelpers;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.Core;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Interfaces.Repository;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Xunit;

namespace CBH.Chat.Business.Tests
{
    public class TeamManagerTest
    {
        private readonly Random _rnd = new Random();

        [Fact]
        public async void GetTeamMembersAsyncTest_WhenTeamIdExists_ReturnsTeamMemberDto()
        {
            var mockPartnerRepository = new Mock<IPartnerRepository>();
            var mockContactRepository = new Mock<IContactRepository>();
            var mockTeamRepository = new Mock<ITeamRepository>();
            var mapper = GetMapperForTeamProfile();

            var teamId = _rnd.Next(111, 1000);            
            var userIdList = new List<int> { teamId };
            var partner = EntityModellers.CreatePartnerEntity();

            var chatUser = EntityModellers.CreateChatUserEntity();

            var team = EntityModellers.CreateTeamEntity();
            var teamEntityList = new List<TeamEntity> { team }; 

            var chatUserList = new List<ChatUser> { chatUser };

            mockPartnerRepository.Setup(x => x.GetTeamMembers(teamId)).Returns(userIdList);
            mockPartnerRepository.Setup(x => x.GetPartnerDetail()).Returns(partner);
            mockContactRepository.Setup(x => x.GetChatUsersAsync(userIdList, partner.PartnerId)).ReturnsAsync(chatUserList);
            mockTeamRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(teamEntityList);

            var expected = mapper.Map<ICollection<ChatUser>, ICollection<TeamMemberDto>>(chatUserList);

            var teamManager = new TeamManager(mockTeamRepository.Object, mockPartnerRepository.Object, mockContactRepository.Object, mapper);
            var actual = await teamManager.GetTeamMembersAsync(teamId);

            Assert.Equal(expected, actual, new LogicEqualityComparer<TeamMemberDto>());

            mockPartnerRepository.VerifyAll();
            mockContactRepository.VerifyAll();
        }

        #region MapperConfiguration

        private static IMapper GetMapperForTeamProfile() => new MapperConfiguration(cfg => cfg.AddProfile(new TeamProfile())).CreateMapper();

        #endregion
    }
}