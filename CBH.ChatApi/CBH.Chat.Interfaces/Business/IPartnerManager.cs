using System;
using CBH.Chat.Domain;

namespace CBH.Chat.Interfaces.Business
{
    public interface IPartnerManager
    {
        Partner GetPartnerByDomainName(string domainName);
        Partner GetPartnerById(int partnerId);
        Guid GetPartnerServiceKey(int partnerId);
    }
}