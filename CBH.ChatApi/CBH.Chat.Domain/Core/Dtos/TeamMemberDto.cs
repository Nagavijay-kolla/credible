namespace CBH.Chat.Domain.Core.Dtos
{
    public class TeamMemberDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string Status { get; set; }
    }
}