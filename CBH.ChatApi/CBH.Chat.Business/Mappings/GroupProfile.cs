using System;
using System.Collections.Generic;
using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;

namespace CBH.Chat.Business.Mappings
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            SetupGroupResponseMap();
            SetupNewGroupRequestMap();
            SetupGroupWithContactsResponsMap();
            SetupUsersResponseMap();
        }

        private void SetupNewGroupRequestMap()
        {
            CreateMap<NewGroupRequestModel, GroupEntity>()
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.NewGuid()))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.UtcNow));
        }

        private void SetupGroupResponseMap()
        {
            CreateMap<GroupEntity, GroupResponseModel>();
        }

        private void SetupGroupWithContactsResponsMap()
        {
            CreateMap<GroupEntity, GroupWithUsersResponseModel>()
                .ForMember(x => x.Members, y => y.Ignore());
        }

        private void SetupUsersResponseMap()
        {
            CreateMap<IEnumerable<UserContactResponseModel>, GroupWithUsersResponseModel>()
                .ForMember(x => x.Members, y => y.MapFrom(z => z));
        }
    }
}