using CBH.ChatSignalR.Domain;
using Microsoft.EntityFrameworkCore;

namespace CBH.ChatSignalR.Repository
{
    public static class ServiceAccountConfigExt
    {
        public static void ServiceAccountAddConfig(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<ServiceAccount>(entity =>
            {
                entity.ToTable("ServiceAccount", schema);
                entity.HasKey(sa => sa.ServiceAccountId);
                entity.Property(sa => sa.ServiceAccountId).HasColumnName("serviceaccount_id").IsRequired();
                entity.Property(sa => sa.ServiceKey).HasColumnName("service_key").IsRequired().HasMaxLength(100);
                entity.Property(sa => sa.AccountName).HasColumnName("account_name").IsRequired().HasMaxLength(300);
                entity.Property(sa => sa.PartnerId).HasColumnName("partner_id").IsRequired();
                entity.Property(sa => sa.DateInactivatedUTC).HasColumnName("date_inactivated_utc").IsRequired(false);
            });
        }
    }
}
