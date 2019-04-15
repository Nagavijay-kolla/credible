using CBH.Chat.Domain;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.DependencyInjection;

namespace CBH.Chat.DependencyInjection
{
    public class PartnerConnectionStringResolver : IPartnerConnectionStringResolver
    {
        private readonly IPartnerManager _partnerManager;
        private readonly string _unresolvedPartnerConnectionString;
        private readonly int _partnerId;
        private readonly string _partnerDomain;

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
            return GetPartner().PartnerId;
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
