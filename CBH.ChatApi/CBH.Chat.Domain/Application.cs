using System;
using System.Collections.Generic;

namespace CBH.Chat.Domain
{
    public class Application
    {
        public Application()
        {
            ApplicationServiceAccounts = new HashSet<ApplicationServiceAccount>();
        }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime DateCreatedUTC { get; set; }

        public virtual ICollection<ApplicationServiceAccount> ApplicationServiceAccounts { get; set; }
    }
}
