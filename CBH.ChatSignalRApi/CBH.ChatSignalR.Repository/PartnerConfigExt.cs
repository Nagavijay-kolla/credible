using CBH.ChatSignalR.Domain;
using Microsoft.EntityFrameworkCore;

namespace CBH.ChatSignalR.Repository
{
    public static class PartnerConfigExt
    {
        public static void PartnerAddConfig(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("Partner", schema);

                entity.HasKey(partner => partner.PartnerId);

                entity.Property(partner => partner.PartnerId).HasColumnName("parter_id").IsRequired();
                entity.Property(partner => partner.PartnerName).HasColumnName("partner_name").IsRequired(false);
                entity.Property(partner => partner.PartnerId).HasColumnName("partner_id").IsRequired();
                entity.Property(partner => partner.PartnerName).HasColumnName("partner_name").IsRequired(false);
                entity.Property(partner => partner.PartnerDesc).HasColumnName("partner_desc").IsRequired(false);
                entity.Property(partner => partner.PartnerDomain).HasColumnName("partner_domain").IsRequired(false);
                entity.Property(partner => partner.PartnerDb).HasColumnName("partner_db").IsRequired(false);
                entity.Property(partner => partner.ExtendedUrl).HasColumnName("extended_url").IsRequired(false);
                entity.Property(partner => partner.IsBlocked).HasColumnName("is_blocked").IsRequired();
                entity.Property(partner => partner.IsSmallBusiness).HasColumnName("is_small_business").IsRequired();
                entity.Property(partner => partner.BbNum).HasColumnName("db_num").IsRequired();
                entity.Property(partner => partner.SubDomain).HasColumnName("sub_domain").IsRequired();
                entity.Property(partner => partner.WebNum).HasColumnName("web_num").IsRequired();
                entity.Property(partner => partner.BaseServer).HasColumnName("base_server").IsRequired(false);
                entity.Property(partner => partner.DbLinkName).HasColumnName("db_link_name").IsRequired(false);
                entity.Property(partner => partner.VisitCountMonth).HasColumnName("visit_count_month").IsRequired(false);
                entity.Property(partner => partner.Deleted).HasColumnName("deleted").IsRequired();
                entity.Property(partner => partner.DateUpdated).HasColumnName("date_updated").IsRequired(false);
                entity.Property(partner => partner.DateCreated).HasColumnName("date_created").IsRequired(false);
                entity.Property(partner => partner.IsProduction).HasColumnName("is_production").IsRequired();
                entity.Property(partner => partner.DatabaseName).HasColumnName("database_name").IsRequired(false);
                entity.Property(partner => partner.UseReportServer).HasColumnName("use_report_server").IsRequired();
                entity.Property(partner => partner.PartnerGuid).HasColumnName("partner_guid").IsRequired();
                entity.Property(partner => partner.DirectEmailName).HasColumnName("direct_email_name").IsRequired(false);
                entity.Property(partner => partner.IsInternal).HasColumnName("is_internal").IsRequired();
            });
        }
    }
}
