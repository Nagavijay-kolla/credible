namespace CBH.ChatSignalR.Interfaces.DependencyInjection
{
    public interface IPartnerConnectionStringResolver
    {
        string GetPartnerConnectionString();
        int GetPartnerId();
    }
}
