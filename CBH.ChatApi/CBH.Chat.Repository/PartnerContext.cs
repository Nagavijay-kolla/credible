using CBH.Chat.Domain.Core.Entities;
using CBH.Chat.Interfaces.Repository;
using CBH.Chat.Repository.PartnerDbConfiguration;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository
{
    public class PartnerContext : DbContext, IPartnerContext
    {
        private readonly int _partner;

        public int GetPartnerId()
        {
            return _partner;
        }

        public DbSet<User> ChatUsers { get; set; }
        public DbSet<ProfileSecurity> ProfileSecurities { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamEmployee> TeamEmployees { get; set; }
        public DbSet<PartnerConfig> PartnerConfigs { get; set; }

        public PartnerContext(DbContextOptions<PartnerContext> options, int partnerId) : base(options)
        {
            _partner = partnerId;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UserMapping("dbo");
            modelBuilder.TeamMapping("dbo");
            modelBuilder.PartnerConfigMapping("dbo");
        }
    }
}