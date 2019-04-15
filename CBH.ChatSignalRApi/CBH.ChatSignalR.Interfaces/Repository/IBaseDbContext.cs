using CBH.ChatSignalR.Domain;
using CBH.Template.Domain;
using Microsoft.EntityFrameworkCore;

namespace CBH.ChatSignalR.Interfaces.Repository
{
    public interface IBaseDbContext
    {
        DbSet<Partner> PartnerDbSet { get; set; }
        DbSet<Application> ApplicationDbSet { get; set; }
        DbSet<ApplicationServiceAccount> ApplicationServiceAccountDbSet { get; set; }
        DbSet<ServiceAccount> ServiceAccountDbSet { get; set; }
        int SaveChanges();
    }
}
