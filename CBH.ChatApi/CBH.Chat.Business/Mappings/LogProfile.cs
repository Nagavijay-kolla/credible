using CBH.Chat.Domain.ChatDomains.Entity;
using CBH.Chat.Domain.ChatDomains.Models.ResponseModels;
using CBH.Chat.Interfaces.Repository;

namespace CBH.Chat.Business.Mappings
{
    public class LogProfile : AutoMapper.Profile
    {
        private readonly IContactRepository _contactRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IGroupRepository _groupRepository;

        public LogProfile(IContactRepository contactRepository, ITeamRepository teamRepository, IGroupRepository groupRepository)
        {
            _contactRepository = contactRepository;
            _teamRepository = teamRepository;
            _groupRepository = groupRepository;

            SetupChatLogMap();
            SetupSendMessageReportMap();
        }

        private void SetupChatLogMap()
        {
            CreateMap<LogEntity, ChatLogResponse>()
                .ForMember(x => x.ArchivedAt, y => y.MapFrom(z => z.ArchivedAt))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => z.CreatedAt))
                .ForMember(x => x.IsArchived, y => y.MapFrom(z => z.IsArchived))
                .ForMember(x => x.IsImportant, y => y.MapFrom(z => z.IsImportant))
                .ForMember(x => x.IsRead, y => y.MapFrom(z => z.IsRead))
                .ForMember(x => x.Message, y => y.MapFrom(z => z.Message))
                .ForMember(x => x.MessageId, y => y.MapFrom(z => z.MessageId))
                .ForMember(x => x.ReadAt, y => y.MapFrom(z => z.ReadAt))
                .ForMember(x => x.TeamId, y => y.MapFrom(z => z.TeamId))
                .ForMember(x => x.ThreadId, y => y.MapFrom(z => z.ThreadId))
                .ForMember(x => x.Type, y => y.MapFrom(z => z.Type.ToString()))
                .ForMember(x => x.SenderName, y => y.MapFrom(z => z.SenderName))
                .ForMember(x => x.RecipientName, y => y.MapFrom(z => z.RecepientName));
        }

        private void SetupSendMessageReportMap()
        {
            CreateMap<LogEntity, SentMessageReportResponseModel>()
                .ForMember(x => x.MessageTime, y => y.MapFrom(z => z.CreatedAt))
                .ForMember(x => x.RecipientName, y => y.MapFrom(z => z.RecepientName))
                .ForMember(x => x.SenderName, y => y.MapFrom(z => z.SenderName))
                .ForMember(x => x.IsImportant, y => y.MapFrom(z => z.IsImportant))
                .ForMember(x => x.IsRead, y => y.MapFrom(z => z.IsRead))
                .ForMember(x => x.DaysUnread, y => y.MapFrom(z => (System.DateTime.UtcNow - z.CreatedAt).Days));
        }

    }
}