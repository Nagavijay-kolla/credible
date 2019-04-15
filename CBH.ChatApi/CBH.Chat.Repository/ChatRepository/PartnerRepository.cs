using System;
using System.Collections.Generic;
using System.Linq;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Domain.Core.Entities;
using CBH.Chat.Interfaces.Repository;

namespace CBH.Chat.Repository.ChatRepository
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly IPartnerContext _partnerDbContext;
        private readonly string _partnerDomainName;
        public const string ArchivalStorageDays = "chatStorage";

        public PartnerRepository(IPartnerContext partnerDbContext, string partnerDomainName)
        {
            _partnerDbContext = partnerDbContext;
            _partnerDomainName = partnerDomainName;
        }

        public IList<string> GetPermissions(int userId, ICollection<string> rightsList)
        {
            if (rightsList == null || rightsList.Count <= 0)
            {
                return new List<string>();
            }

            var rightsQuery = from ps in _partnerDbContext.ProfileSecurities
                              join s in _partnerDbContext.Securities on ps.Security_Id equals s.Security_Id
                              join p in _partnerDbContext.Profiles on ps.Profile_Id equals p.Profile_Id
                              join u in _partnerDbContext.ChatUsers on p.Profile_Code equals u.ProfileCode
                              where u.Id == userId && rightsList.Contains(s.Action)
                              select s.Action;
            return rightsQuery.ToList();
        }

        public User GetUser(int userId)
        {
            return (from user in _partnerDbContext.ChatUsers
                    where user.Id == userId && user.Deleted == false && user.IsEnableChat
                    select user)
                .SingleOrDefault();
        }

        public PartnerDto GetPartnerDetail()
        {
            return new PartnerDto
            {
                PartnerId = _partnerDbContext.GetPartnerId(),
                PartnerName = _partnerDomainName
            };
        }

        public ICollection<Team> GetTeams(int userId, bool isAllTeams)
        {
            IQueryable<Team> query;
            if (isAllTeams)
            {
                query = from team in _partnerDbContext.Teams
                        orderby team.Name
                        select team;
            }
            else
            {
                query = from team in _partnerDbContext.Teams
                        join teamEmployee in _partnerDbContext.TeamEmployees on team.Id equals teamEmployee.Team_Id
                        where teamEmployee.Emp_Id == userId
                        select team;
            }
            return query.ToList();
        }

        public ICollection<int> GetTeamMembers(int teamId)
        {
            return (from u in _partnerDbContext.ChatUsers
                    join te in _partnerDbContext.TeamEmployees on u.Id equals te.Emp_Id
                    join t in _partnerDbContext.Teams on te.Team_Id equals t.Id
                    where t.Id == teamId && u.IsEnableChat
                    select (int)u.Id)
                   .ToList();
        }

        public int GetPartnerArchivalDays()
        {
            var parameterValue = (from p in _partnerDbContext.PartnerConfigs where p.Parameter == ArchivalStorageDays select p.Value).SingleOrDefault();
            return string.IsNullOrEmpty(parameterValue) ? 90 : Convert.ToInt32(parameterValue);
        }
    }
}