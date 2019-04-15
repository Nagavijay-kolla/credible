namespace CBH.Chat.Domain.Core.Dtos
{
    public class ContactUser
    {
        public int UserId { get; set; }
        public int PartnerId { get; set; }
        public int ChatUserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string Status { get; set; }
        
    }
}