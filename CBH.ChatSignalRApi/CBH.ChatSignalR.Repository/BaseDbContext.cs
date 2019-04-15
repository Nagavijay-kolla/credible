using CBH.Template.Domain;
using CBH.ChatSignalR.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using CBH.ChatSignalR.Domain;

namespace CBH.ChatSignalR.Repository
{
    public class BaseDbContext : DbContext, IBaseDbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options) { }

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
