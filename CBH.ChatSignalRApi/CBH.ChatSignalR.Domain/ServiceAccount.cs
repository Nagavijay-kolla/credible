using System;
using System.Collections.Generic;

namespace CBH.ChatSignalR.Domain
{
    public class ServiceAccount
    {
        public ServiceAccount()
        {
            ApplicationServiceAccounts = new HashSet<ApplicationServiceAccount>();
        }
        public int ServiceAccountId { get; set; }
        public string ServiceKey { get; set; }
        public string AccountName { get; set; }
        public int PartnerId { get; set; }
        public DateTime? DateInactivatedUTC { get; set; }

        public virtual ICollection<ApplicationServiceAccount> ApplicationServiceAccounts { get; set; }
    }
}
