using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;

namespace CBH.Chat.Business
{
    public class TeamManager : ITeamManager
    {
        private readonly ITeamRepository _teamRepository;

        private readonly IPartnerRepository _partnerRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public TeamManager(ITeamRepository teamRepository, IPartnerRepository partnerRepository, IContactRepository contactRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _partnerRepository = partnerRepository;
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<TeamResponseModel> FetchByIdAsync(Guid teamId) => _mapper.Map<TeamEntity, TeamResponseModel>(await _teamRepository.GetAsync(teamId));

        public async Task<IEnumerable<TeamResponseModel>> GetTeams(int userId)
        {
            var teamEntities = new List<TeamEntity>();
            var partnerId = _partnerRepository.GetPartnerDetail().PartnerId;
            var userRights = new[] { UserConfigurationManager.AllTeams };
            var userRightsFor = _partnerRepository.GetPermissions(userId, userRights);
            var teams = _partnerRepository.GetTeams(userId, userRightsFor.Contains(UserConfigurationManager.AllTeams));

            foreach (var team in teams)
            {
                var teamEntity = await _teamRepository.GetAsync(team.Id, partnerId) ?? await _teamRepository.CreateAsync(new TeamEntity { PartnerId = partnerId, TeamId = team.Id, Name = team.Name });
                teamEntities.Add(teamEntity);
            }
            return _mapper.Map<IEnumerable<TeamEntity>, IEnumerable<TeamResponseModel>>(teamEntities);
        }

        public async Task<IEnumerable<TeamMemberDto>> GetTeamMembersAsync(int teamId)
        {
            var userIds = _partnerRepository.GetTeamMembers(teamId);
            var chatUsers = await _contactRepository.GetChatUsersAsync(userIds, _partnerRepository.GetPartnerDetail().PartnerId);
            return _mapper.Map<ICollection<ChatUser>, ICollection<TeamMemberDto>>(chatUsers).OrderBy(x => x.UserName);
        }
    }
}