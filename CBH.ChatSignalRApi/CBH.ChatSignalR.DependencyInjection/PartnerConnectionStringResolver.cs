using CBH.ChatSignalR.Domain;
using CBH.ChatSignalR.Interfaces.Business;
using CBH.ChatSignalR.Interfaces.DependencyInjection;

namespace CBH.ChatSignalR.DependencyInjection
{
    public class PartnerConnectionStringResolver : IPartnerConnectionStringResolver
    {
        //Fields
        private readonly IPartnerManager _partnerManager;
        private readonly string _unresolvedPartnerConnectionString;
        private readonly int _partnerId;
        private readonly string _partnerDomain;

        //Constructors
        public PartnerConnectionStringResolver(IPartnerManager partnerManager, string unresolvedPartnerConnectionString, int partnerId)
        {
            _partnerManager = partnerManager;
            _unresolvedPartnerConnectionString = unresolvedPartnerConnectionString;
            _partnerId = partnerId;
        }

        public PartnerConnectionStringResolver(IPartnerManager partnerManager, string unresolvedPartnerConnectionString, string partnerDomain)
        {
            _partnerManager = partnerManager;
            _unresolvedPartnerConnectionString = unresolvedPartnerConnectionString;
            _partnerDomain = partnerDomain;
        }

        public string GetPartnerConnectionString()
        {
            Partner partner;
            if (_partnerId != 0)
            {
                partner = _partnerManager.GetPartnerById(_partnerId);
            }
            else
            {
                partner = _partnerManager.GetPartnerByDomainName(_partnerDomain);
            }

            if (partner == null)
            {
                return null;
            }

            return string.Format(_unresolvedPartnerConnectionString, partner.PartnerDb, partner.DatabaseName, partner.PartnerId);
        }

        public int GetPartnerId()
        {
            var partner = GetPartner();
            return partner.PartnerId;
        }

        private Partner GetPartner()
        {
            Partner partner;
            if (_partnerId != 0)
            {
                partner = _partnerManager.GetPartnerById(_partnerId);
            }
            else if (!string.IsNullOrEmpty(_partnerDomain))
            {
                partner = _partnerManager.GetPartnerByDomainName(_partnerDomain);
            }
            else
            {
                partner = null;
            }
            return partner;
        }
    }
}
