using MongoDB.Driver;

namespace CBH.Chat.Interfaces.Repository
{
    public interface IChatDbClient
    {
        IMongoDatabase Database { get; set; }
    }
}