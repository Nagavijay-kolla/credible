namespace CBH.Chat.Domain.Core.Dtos
{
    public class LoggedUserDetailDto
    {
        public int UserId { get; set; }
        public int ChatUserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public bool IsEnableChat { get; set; }
        public bool IsBroadcastEnable { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsHighImportanceEnable { get; set; }
        public bool IsChatAllTeams { get; set; }
        public bool IsEmployeeMessageLogViewAll { get; set; }
        public bool IsAppointmentArrivalMessage { get; set; }
        public bool IsSendEmpMessage { get; set; }
        public string UserStatus { get; set; }
        public string UnRead { get; set; }
    }
}