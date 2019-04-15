using AutoMapper;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Domain.Core;
using CBH.Chat.Domain.Core.Dtos;

namespace CBH.Chat.Business.Mappings
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            SetupContactUserMap();
            SetupChatUserResponseModelMap();
        }

        private void SetupContactUserMap()
        {
            CreateMap<ChatUser, ContactUser>()
                .ForMember(x => x.UserId, y => y.MapFrom(z => z.UserId))
                .ForMember(x => x.PartnerId, y => y.MapFrom(z => z.PartnerId))
                .ForMember(x => x.ChatUserId, y => y.MapFrom(z => z.ChatUserId))
                .ForMember(x => x.UserName, y => y.MapFrom(z => $"{z.FirstName} {z.LastName}"))
                .ForMember(x => x.UserRole, y => y.MapFrom(z => z.UserRole))
                .ForMember(x => x.Status, y => y.MapFrom(z => z.Status ?? UserStatus.Available.ToString()));
        }
        private void SetupChatUserResponseModelMap()
        {
            CreateMap<ChatUser, UserContactResponseModel>()
                .ForMember(x => x.ChatUserId, y => y.MapFrom(z => z.ChatUserId))
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.Status, y => y.MapFrom(z => z.Status));
        }
    }
}
