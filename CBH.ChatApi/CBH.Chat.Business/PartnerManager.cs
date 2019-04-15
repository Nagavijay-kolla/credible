using System;
using System.Linq;
using CBH.Chat.Domain;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace CBH.Chat.Business
{
    public class PartnerManager : IPartnerManager
    {
        private readonly IBaseDbContext _baseDbContext;

        public PartnerManager(IBaseDbContext baseDbContext)
        {
            _baseDbContext = baseDbContext ?? throw new ArgumentNullException(nameof(baseDbContext));
        }

        public Partner GetPartnerByDomainName(string domainName)
        {
            return _baseDbContext.PartnerDbSet.SingleOrDefault(x => x.PartnerDomain == domainName);
        }

        public Partner GetPartnerById(int partnerId)
        {
            return _baseDbContext.PartnerDbSet.SingleOrDefault(x => x.PartnerId == partnerId);
        }

        public Guid GetPartnerServiceKey(int partnerId)
        {
            var serviceKey =
            _baseDbContext.ApplicationServiceAccountDbSet
                    .Include(asa => asa.ServiceAccount)
                    .Include(asa => asa.Application)
                    .Where(asa => !asa.ServiceAccount.DateInactivatedUTC.HasValue &&
                                  asa.Application.Code == "CORE" &&
                                  asa.ServiceAccount.PartnerId == partnerId)
                    .Select(asa => asa.ServiceAccount.ServiceKey)
                    .FirstOrDefault();
            Guid.TryParse(serviceKey, out var parsedGuid);
            return parsedGuid;
        }
    }
}
