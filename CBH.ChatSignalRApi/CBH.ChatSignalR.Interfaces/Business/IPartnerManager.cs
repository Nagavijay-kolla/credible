using System;
using CBH.ChatSignalR.Domain;

namespace CBH.ChatSignalR.Interfaces.Business
{
    public interface IPartnerManager
    {
        Partner GetPartnerByDomainName(string domainName);
        Partner GetPartnerById(int partnerId);
        Guid GetPartnerServiceKey(int partnerId);
    }
}
