using System;
using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Infrastructure.Chat.Constants;

namespace CBH.Chat.Business.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            SetupNewMessageMap();
            SetupNewBroadcastMessageMap();
            SetupMessageResponseModelChatLogMap();
        }

        private void SetupNewMessageMap()
        {
            CreateMap<NewMessageRequestModel, MessageEntity>()
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.NewGuid()))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.UtcNow));
        }

        private void SetupNewBroadcastMessageMap()
        {
            CreateMap<NewBroadcastMessageRequestModel, MessageEntity>()
                .ForMember(x => x.Id, y => y.MapFrom(z => Guid.NewGuid()))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.UtcNow));
        }
        private void SetupMessageResponseModelChatLogMap()
        {
            CreateMap<MessageResponseModel, LogEntity>()
                .ForMember(x => x.ThreadId, y => y.MapFrom(z => z.ThreadId))
                .ForMember(x => x.MessageId, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.SenderId, y => y.MapFrom(z => z.FromUserId))
                .ForMember(x => x.Message, y => y.MapFrom(z => z.Content))
                .ForMember(x => x.IsImportant, y => y.MapFrom(z => z.IsImportant))
                .ForMember(x => x.RecipientId,y=>y.MapFrom((src, dest, destMember, context) =>(int)context.Items[MappingConstants.RecipientUserId]))
                .ForAllOtherMembers(x => x.Ignore());
          
        }
    }
}