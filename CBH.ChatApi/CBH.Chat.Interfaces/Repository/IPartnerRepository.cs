using System.Collections.Generic;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Domain.Core.Entities;

namespace CBH.Chat.Interfaces.Repository
{
    public interface IPartnerRepository
    {
        IList<string> GetPermissions(int userId, ICollection<string> rightsList);
        User GetUser(int userId);
        PartnerDto GetPartnerDetail();
        ICollection<Team> GetTeams(int userId, bool isAllTeams);
        ICollection<int> GetTeamMembers(int teamId);
        int GetPartnerArchivalDays();
    }
}