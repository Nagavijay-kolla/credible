using CBH.ChatSignalR.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace CBH.ChatSignalR.Repository
{
    public class PartnerContext : DbContext, IPartnerContext
    {
        private int _partnerId;

        public int GetPartnerId()
        {
            return _partnerId;
        }

        public PartnerContext(DbContextOptions<PartnerContext> options, int partnerId)
            : base(options)
        {
            _partnerId = partnerId;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
