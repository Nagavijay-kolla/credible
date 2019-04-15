using CBH.Chat.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository.ChatUserDbConfiguration
{
    public static class ChatUserConfiguration
    {
        public static void ChatUserMapping(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<ChatUser>(entity =>
            {
                entity.ToTable("ChatUser");
                entity.HasKey(cu => cu.ChatUserId);

                entity.Property(cu => cu.ChatUserId).HasColumnName("ChatUser_Id").IsRequired();
                entity.Property(cu => cu.UserId).HasColumnName("User_Id");
                entity.Property(cu => cu.PartnerId).HasColumnName("Partner_Id");
                entity.Property(cu => cu.FirstName).HasColumnName("User_FirstName");
                entity.Property(cu => cu.LastName).HasColumnName("User_LastName");
                entity.Property(cu => cu.UserRole).HasColumnName("User_Role");
                entity.Property(cu => cu.Status).HasColumnName("User_Chat_Status");
            });
        }
    }
}
