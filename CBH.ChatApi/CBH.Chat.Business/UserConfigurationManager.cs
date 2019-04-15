using System;
using System.Threading.Tasks;
using CBH.Chat.Domain.ChatDomains.Enumerations;
using CBH.Chat.Domain.Core.Dtos;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;

namespace CBH.Chat.Business
{
    public class UserConfigurationManager : IUserConfigurationManager
    {
        public const string SendBroadcast = "SendBroadcast";
        public const string AllTeams = "ChatAllTeams";
        public const string EmployeeMessageLogViewAll = "EmployeeMessageLogViewAll";
        public const string AppointmentArrivalMessage = "AppointmentArrivalMessage";
        public const string SendEmployeeMessage = "SendEmpMessage";
        public const string ProfileAdmin = "ADMIN";
        private static readonly string[] UserRights = { SendBroadcast, AllTeams, EmployeeMessageLogViewAll, AppointmentArrivalMessage, SendEmployeeMessage };
        private readonly IPartnerRepository _partnerRepository;
        private readonly IContactRepository _contactRepository;

        public UserConfigurationManager(IPartnerRepository partnerRepository, IContactRepository contactRepository)
        {
            _partnerRepository = partnerRepository ?? throw new ArgumentNullException(nameof(partnerRepository));
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        }

        public async Task<UserConfigurationDto> GetUserChatConfigurationAsync(int userId)
        {
            var user = _partnerRepository.GetUser(userId);
            var partner = _partnerRepository.GetPartnerDetail();
            var chatUser = await _contactRepository.GetChatUserAsync(user.Id, partner.PartnerId);
            var userRightsFor = _partnerRepository.GetPermissions(userId, UserRights);
            return new UserConfigurationDto
            {
                LoggedUserDetail = new LoggedUserDetailDto
                {
                    UserId = user.Id,
                    ChatUserId = chatUser.ChatUserId,
                    UserName = $"{user.FirstName} {user.LastName}",
                    UserRole = user.UserRole,
                    IsEnableChat = user.IsEnableChat,
                    IsBroadcastEnable = userRightsFor.Contains(SendBroadcast),
                    IsAdmin = user.ProfileCode.Equals(ProfileAdmin),
                    IsHighImportanceEnable = user.IsHighImportanceEnable,
                    IsChatAllTeams = userRightsFor.Contains(AllTeams),
                    IsEmployeeMessageLogViewAll = userRightsFor.Contains(EmployeeMessageLogViewAll),
                    IsAppointmentArrivalMessage = userRightsFor.Contains(AppointmentArrivalMessage),
                    IsSendEmpMessage = userRightsFor.Contains(SendEmployeeMessage),
                    UserStatus = chatUser.Status ?? UserStatus.Available.ToString()
                },
                Partner = partner
            };
        }
    }
}