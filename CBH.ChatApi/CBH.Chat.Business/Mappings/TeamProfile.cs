using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core;
using CBH.Chat.Domain.Core.Dtos;

namespace CBH.Chat.Business.Mappings
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            SetupTeamMemberMap();
            SetupTeamResponseModelMap();
        }

        private void SetupTeamMemberMap() => CreateMap<ChatUser, TeamMemberDto>()
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.ChatUserId))
                .ForMember(x => x.UserName, y => y.MapFrom(z => $"{z.FirstName} {z.LastName}"))
                .ForMember(x => x.UserRole, y => y.MapFrom(z => z.UserRole))
                .ForMember(x => x.Status, y => y.MapFrom(z => z.Status?? UserStatus.Available.ToString()));


        private void SetupTeamResponseModelMap() => CreateMap<TeamEntity, TeamResponseModel>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.TeamId))
                .ForMember(x => x.TeamId, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.Members, y => y.MapFrom(z => z.Members));
    }
}