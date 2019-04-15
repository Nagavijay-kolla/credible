using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Entity;

namespace CBH.Chat.Interfaces.Repository
{
    public interface ITeamRepository
    {
        Task<IEnumerable<TeamEntity>> GetAllAsync();
        Task<TeamEntity> GetAsync(Guid teamId);
        Task<TeamEntity> GetAsync(int teamId, int partnerid);
        Task<TeamEntity> CreateAsync(TeamEntity newTeamEntity);
        Task<TeamEntity> UpdateAsync(TeamEntity team);
    }
}