using CBH.Chat.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository.ChatUserDbConfiguration
{
    public static class UserPartnerConfiguration
    {
        public static void UserPartnerMapping(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<UserPartner>(entity =>
            {
                entity.ToTable("UserPartner");
                entity.HasKey(up => up.UserPartnerId);

                entity.Property(up => up.UserPartnerId).HasColumnName("UserPartner_Id");
                entity.Property(up => up.UserId).HasColumnName("User_Id");
                entity.Property(up => up.PartnerId).HasColumnName("Partner_Id");

            });
        }
    }
}
