using CBH.Template.Domain;
using System;

namespace CBH.ChatSignalR.Domain
{
    public class ApplicationServiceAccount
    {
        public int ApplicationServiceAccountId { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceAccountId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreatedUTC { get; set; }
        public DateTime DateUpdatedUTC { get; set; }

        public virtual Application Application { get; set; }
        public virtual ServiceAccount ServiceAccount { get; set; }
    }
}
