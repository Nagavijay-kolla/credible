using CBH.Chat.Domain;
using CBH.Chat.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Repository
{
    public class BaseDbContext : DbContext, IBaseDbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

        public virtual DbSet<Partner> PartnerDbSet { get; set; }
        public DbSet<Application> ApplicationDbSet { get; set; }
        public DbSet<ApplicationServiceAccount> ApplicationServiceAccountDbSet { get; set; }
        public DbSet<ServiceAccount> ServiceAccountDbSet { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.PartnerAddConfig("dbo");
            modelBuilder.ApplicationAddConfig("dbo");
            modelBuilder.ApplicationServiceAccountAddConfig("dbo");
            modelBuilder.ServiceAccountAddConfig("dbo");
        }
    }
}
