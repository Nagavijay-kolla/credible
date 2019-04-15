using CBH.Chat.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository.PartnerDbConfiguration
{
    public static class UserConfiguration
    {
        public static void UserMapping(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Employees", schema);
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id).HasColumnName("emp_id").IsRequired();
                entity.Property(u => u.FirstName).HasColumnName("first_name");
                entity.Property(u => u.LastName).HasColumnName("last_name");
                entity.Property(u => u.UserRole).HasColumnName("title");
                entity.Property(u => u.IsEnableChat).HasColumnName("emp_chat");
                entity.Property(u => u.IsHighImportanceEnable).HasColumnName("emp_highimportance");
                entity.Property(u => u.IsEmployee).HasColumnName("is_not_emp");
                entity.Property(u => u.Status).HasColumnName("emp_status");
                entity.Property(u => u.Deleted).HasColumnName("deleted");
                entity.Property(u => u.ProfileCode).HasColumnName("Profile_Code");

            });
        }
    }
}
