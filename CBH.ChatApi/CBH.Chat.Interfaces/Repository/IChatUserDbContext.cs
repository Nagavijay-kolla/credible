using Microsoft.EntityFrameworkCore;
using CBH.Chat.Domain.Core;


namespace CBH.Chat.Interfaces.Repository
{
    public interface IChatUserDbContext
    {
        DbSet<UserPartner> UserPartners { get; set; }
        DbSet<ChatUser> ChatUsers { get; set; }
        DbSet<UserContact> UserContacts { get; set; }
        int SaveChanges();
    }
}
