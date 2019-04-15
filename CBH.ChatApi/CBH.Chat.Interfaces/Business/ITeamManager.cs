using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core.Dtos;

namespace CBH.Chat.Interfaces.Business
{
    public interface ITeamManager
    {
        Task<TeamResponseModel> FetchByIdAsync(Guid teamId);
        Task<IEnumerable<TeamResponseModel>> GetTeams(int userId);
        Task<IEnumerable<TeamMemberDto>> GetTeamMembersAsync(int teamId);
    }
}