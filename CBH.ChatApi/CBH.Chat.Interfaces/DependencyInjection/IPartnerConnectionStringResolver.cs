﻿namespace CBH.Chat.Interfaces.DependencyInjection
{
    public interface IPartnerConnectionStringResolver
    {
        string GetPartnerConnectionString();
        int GetPartnerId();
    }
}