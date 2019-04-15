using CBH.ChatSignalR.Domain;
using CBH.Template.Domain;
using Microsoft.EntityFrameworkCore;

namespace CBH.ChatSignalR.Repository
{
    public static class ApplicationConfigExt
    {
        public static void ApplicationAddConfig(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application", schema);
                entity.HasKey(a => a.ApplicationId);
                entity.Property(a => a.ApplicationId).HasColumnName("application_id").IsRequired();
                entity.Property(a => a.Name).HasColumnName("name").IsRequired(false).HasMaxLength(50);
                entity.Property(a => a.Code).HasColumnName("code").IsRequired(false).HasMaxLength(10);
                entity.Property(a => a.DateCreatedUTC).HasColumnName("date_created_utc").IsRequired();
            });
        }
    }
}
