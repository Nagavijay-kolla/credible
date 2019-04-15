using System;
using System.Collections.Generic;
using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core;
using CBH.Chat.Infrastructure.Chat.Constants;

namespace CBH.Chat.Business.Mappings
{
    public class ThreadProfile : Profile
    {
        public ThreadProfile()
        {
            SetupThreadResponseMap();
            SetupMessagesResponseMap();
            SetupMessageResponseMap();
            SetupNewMessageRequestMap();
            SetupThreadContactResponseMap();
            SetupThreadWithContactsResponseMap();
            SetupThreadChatLogResponseMap();
            SetupThreadChatUserContactResponseMap();
            SetupThreadChatLogMap();
        }

        private void SetupThreadResponseMap()
        {
            CreateMap<ThreadEntity, ThreadWithMessagesResponseModel>()
            .ForMember(x => x.Participants, y => y.Ignore())
            .ForMember(x => x.IsArchived, y => y.MapFrom((src, dest, destMember, context) => context.Items.ContainsKey(MappingConstants.ThreadRequestUserId) && src.ArchivedBy?.Contains((int)context.Items[MappingConstants.ThreadRequestUserId]) == true));
        }

        private void SetupThreadWithContactsResponseMap()
        {
            CreateMap<ThreadEntity, ThreadWithContactsResponseModel>()
            .ForMember(x => x.Participants, y => y.Ignore())
            .ForMember(x => x.IsArchived, y => y.MapFrom((src, dest, destMember, context) => context.Items.ContainsKey(MappingConstants.ThreadRequestUserId) && src.ArchivedBy?.Contains((int)context.Items[MappingConstants.ThreadRequestUserId]) == true));
        }

        private void SetupMessagesResponseMap()
        {
            CreateMap<IEnumerable<MessageResponseModel>, ThreadWithMessagesResponseModel>()
            .ForMember(x => x.Messages, y => y.MapFrom(z => z));
        }

        private void SetupMessageResponseMap()
        {
            CreateMap<MessageEntity, MessageResponseModel>();
        }

        private void SetupNewMessageRequestMap()
        {
            CreateMap<NewMessageRequestModel, MessageEntity>()
            .ForMember(x => x.Id, y => y.MapFrom(z => Guid.NewGuid()))
            .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.UtcNow));
        }

        private void SetupThreadContactResponseMap()
        {
            CreateMap<IEnumerable<UserContactResponseModel>, ThreadWithMessagesResponseModel>()
            .ForMember(x => x.Participants, y => y.MapFrom(z => z));
        }

        private void SetupThreadChatLogResponseMap()
        {
            CreateMap<ThreadWithMessagesResponseModel, ChatLogResponse>();
        }

        private void SetupThreadChatUserContactResponseMap()
        {
            CreateMap<ChatUser, UserContactResponseModel>()
                .ForMember(x => x.IsActive, y => y.Ignore());
        }
        private void SetupThreadChatLogMap()
        {
            CreateMap<ThreadEntity, LogEntity>()
                .ForMember(x => x.TeamId, y => y.MapFrom(z => z.TeamId))
                .ForMember(x => x.GroupId, y => y.MapFrom(z => z.GroupId))
                .ForMember(x => x.ReadAt, y => y.MapFrom(z => DateTime.UtcNow))
                .ForMember(x => x.ArchivedAt, y => y.MapFrom(z => DateTime.UtcNow))
                .ForMember(x => x.Type, y => y.MapFrom(z => z.Type));
        }
    }
}