namespace CBH.ChatSignalR.Interfaces.Repository
{
    public interface IPartnerContext
    {
        int GetPartnerId();
        int SaveChanges();
    }
}
