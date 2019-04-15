using CBH.Chat.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Interfaces.Repository
{
    public interface IPartnerContext
    {
        int GetPartnerId();
        DbSet<User> ChatUsers { get; set; }
        DbSet<ProfileSecurity> ProfileSecurities { get; set; }
        DbSet<Profile> Profiles { get; set; }
        DbSet<Security> Securities { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<TeamEmployee> TeamEmployees { get; set; }
        DbSet<PartnerConfig> PartnerConfigs { get; set; }
    }
}