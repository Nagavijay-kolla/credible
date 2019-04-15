namespace CBH.Chat.Domain.Core.Dtos
{
    public class ContactDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public bool IsActive { get; set; }
    }
}