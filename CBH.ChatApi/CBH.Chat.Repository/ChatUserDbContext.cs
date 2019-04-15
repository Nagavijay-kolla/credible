using Microsoft.EntityFrameworkCore;
using CBH.Chat.Interfaces.Repository;
using CBH.Chat.Domain.Core;
using CBH.Chat.Repository.ChatUserDbConfiguration;

namespace CBH.Chat.Repository
{
    public class ChatUserDbContext : DbContext, IChatUserDbContext
    {
        public ChatUserDbContext(DbContextOptions<ChatUserDbContext> options) : base(options) { }

        public DbSet<UserPartner> UserPartners { get ; set ; }
        public DbSet<ChatUser> ChatUsers { get ; set ; }
        public DbSet<UserContact> UserContacts { get ; set ; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UserPartnerMapping("dbo");
            modelBuilder.ChatUserMapping("dbo");
            modelBuilder.UserContactMapping("dbo");
        }

    }
}
