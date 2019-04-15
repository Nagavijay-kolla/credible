using CBH.Chat.Domain;
using CBH.Chat.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Interfaces.Repository
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