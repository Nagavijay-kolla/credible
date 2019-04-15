using System.Collections.Generic;

namespace CBH.Chat.Domain.Core.Dtos
{
    public class ContactUsersDto
    {
        public int UserId { get; set; }
        public IEnumerable<ContactUser> ContactUsers { get; set; }
    }
}