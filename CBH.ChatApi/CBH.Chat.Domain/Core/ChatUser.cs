namespace CBH.Chat.Domain.Core
{
    public class ChatUser
    {
        public int ChatUserId { get; set; }
        public int UserId { get; set; }
        public int PartnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserRole { get; set; }
        public string Status { get; set; }
        public string Name => $"{FirstName} {LastName}";  
    }
}
