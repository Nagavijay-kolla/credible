using System;
using System.Linq;
using CBH.ChatSignalR.Domain;
using CBH.ChatSignalR.Interfaces.Business;
using CBH.ChatSignalR.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace CBH.ChatSignalR.Business
{
    public class PartnerManager : IPartnerManager
    {
        protected IBaseDbContext BaseDbContext { get; private set; }

        public PartnerManager(IBaseDbContext baseDbContext)
        {
            if (baseDbContext == null)
            {
                throw new ArgumentNullException("baseDbContext");
            }

            BaseDbContext = baseDbContext;
        }

        public Partner GetPartnerByDomainName(string domainName)
        {
            return BaseDbContext.PartnerDbSet.SingleOrDefault(p => p.PartnerDomain == domainName);
        }

        public Partner GetPartnerById(int partnerId)
        {
            return BaseDbContext.PartnerDbSet.SingleOrDefault(p => p.PartnerId == partnerId);
        }

        public Guid GetPartnerServiceKey(int partnerId)
        {
            var serviceKey =
            BaseDbContext.ApplicationServiceAccountDbSet
                    .Include(asa => asa.ServiceAccount)
                    .Include(asa => asa.Application)
                    .Where(asa => !asa.ServiceAccount.DateInactivatedUTC.HasValue &&
                                  asa.Application.Code == "CORE" &&
                                  asa.ServiceAccount.PartnerId == partnerId)
                    .Select(asa => asa.ServiceAccount.ServiceKey)
                    .FirstOrDefault();
            Guid parsedGuid;
            Guid.TryParse(serviceKey, out parsedGuid);
            return parsedGuid;
        }
    }
}
