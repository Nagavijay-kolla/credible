using CBH.Chat.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository.PartnerDbConfiguration
{
    public static class PartnerConfigConfiguration
    {
        public static void PartnerConfigMapping(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<PartnerConfig>(entity =>
            {
                entity.ToTable("PartnerConfig");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasColumnName("partnerconfig_id").IsRequired();
                entity.Property(p => p.Parameter).HasColumnName("parameter");
                entity.Property(p => p.Value).HasColumnName("paramvalue");
            });
        }
    }
}
