namespace CBH.Chat.Domain.Core
{
    public class UserContact
    {
        public int UserContactId { get; set; }
        public int UserId { get; set; }
        public int ContactUserId { get; set; }
    }
}
