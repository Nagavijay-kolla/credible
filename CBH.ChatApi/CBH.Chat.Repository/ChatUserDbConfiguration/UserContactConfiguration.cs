using CBH.Chat.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository.ChatUserDbConfiguration
{
    public static class UserContactConfiguration
    {
        public static void UserContactMapping(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<UserContact>(entity =>
            {
                entity.ToTable("UserContacts");
                entity.HasKey(uc => uc.UserContactId);

                entity.Property(uc => uc.UserContactId).HasColumnName("Id");
                entity.Property(uc => uc.UserId).HasColumnName("User_Id");
                entity.Property(uc => uc.ContactUserId).HasColumnName("ContactUser_Id");
            });
        }
    }
}
