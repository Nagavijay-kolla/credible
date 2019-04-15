using CBH.Chat.Domain;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository
{
    public static class ApplicationServiceAccountConfigExt
    {
        public static void ApplicationServiceAccountAddConfig(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<ApplicationServiceAccount>(entity =>
            {
                entity.ToTable("ApplicationServiceAccount", schema);
                entity.HasKey(sa => sa.ApplicationServiceAccountId);
                entity.Property(sa => sa.ApplicationServiceAccountId).HasColumnName("application_service_account_id").IsRequired();
                entity.Property(sa => sa.ApplicationId).HasColumnName("application_id").IsRequired();
                entity.Property(sa => sa.ServiceAccountId).HasColumnName("serviceaccount_id").IsRequired();
                entity.Property(sa => sa.IsActive).HasColumnName("is_active").IsRequired();
                entity.Property(sa => sa.DateCreatedUTC).HasColumnName("date_created_utc").IsRequired();
                entity.Property(sa => sa.DateUpdatedUTC).HasColumnName("date_updated_utc").IsRequired();

                entity.HasOne(sa => sa.Application)
                    .WithMany(a => a.ApplicationServiceAccounts)
                    .HasForeignKey(sa => sa.ApplicationId);
                entity.HasOne(sa => sa.ServiceAccount)
                    .WithMany(a => a.ApplicationServiceAccounts)
                    .HasForeignKey(sa => sa.ServiceAccountId);
            });
        }
    }
}
