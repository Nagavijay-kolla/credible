using CBH.Chat.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository.PartnerDbConfiguration
{
    public static class TeamConfiguration
    {
        public static void TeamMapping(this ModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Id).HasColumnName("team_id").IsRequired();
                entity.Property(t => t.Name).HasColumnName("team_name");
            });
        }
    }
}
