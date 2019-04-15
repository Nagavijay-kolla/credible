using System;

namespace CBH.Chat.Domain
{
    public class Partner
    {
        public int PartnerId { get; set; }

        public string PartnerName { get; set; }

        public string PartnerDesc { get; set; }

        public string PartnerDomain { get; set; }

        public string PartnerDb { get; set; }

        public string ExtendedUrl { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsSmallBusiness { get; set; }

        public byte BbNum { get; set; }

        public string SubDomain { get; set; }

        public byte WebNum { get; set; }

        public string BaseServer { get; set; }

        public string DbLinkName { get; set; }

        public int? VisitCountMonth { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool IsProduction { get; set; }

        public string DatabaseName { get; set; }

        public bool UseReportServer { get; set; }

        public Guid PartnerGuid { get; set; }

        public string DirectEmailName { get; set; }

        public bool IsInternal { get; set; }

    }
}
