using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Interfaces.Repository;
using MongoDB.Driver;

namespace CBH.Chat.Repository.ChatRepository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IMongoCollection<TeamEntity> _teams;
        private static readonly FindOptions<TeamEntity, TeamEntity> OrderByNameFindOptions = new FindOptions<TeamEntity, TeamEntity>
        {
            Sort = Builders<TeamEntity>.Sort.Ascending(x => x.Name)
        };
        private static readonly FindOneAndReplaceOptions<TeamEntity, TeamEntity> DefaultFindAndReplaceOption = new FindOneAndReplaceOptions<TeamEntity, TeamEntity>
        {
            ReturnDocument = ReturnDocument.After
        };

        public TeamRepository(IChatDbClient dbClient, IPartnerContext partnerContext)
        {
            _teams = dbClient.Database.GetCollection<TeamEntity>(DbConstants.TeamCollection);
        }

        public async Task<IEnumerable<TeamEntity>> GetAllAsync()
        {
            return (await _teams.FindAsync(x => true, OrderByNameFindOptions)).ToEnumerable();
        }

        public async Task<TeamEntity> GetAsync(Guid groupId)
        {
            return await (await _teams.FindAsync(x => x.Id == groupId)).SingleOrDefaultAsync();
        }

        public async Task<TeamEntity> GetAsync(int teamId,int partnerid)
        {
            return await(await _teams.FindAsync(x => x.TeamId == teamId && x.PartnerId==partnerid)).SingleOrDefaultAsync();
        }
        public async Task<TeamEntity> CreateAsync(TeamEntity newTeamEntity)
        {
            await _teams.InsertOneAsync(newTeamEntity);
            return newTeamEntity;
        }
        public async Task<TeamEntity> UpdateAsync(TeamEntity team)
        {
            return await _teams.FindOneAndReplaceAsync<TeamEntity>(x => x.Id == team.Id, team, DefaultFindAndReplaceOption);
        }
    }
}