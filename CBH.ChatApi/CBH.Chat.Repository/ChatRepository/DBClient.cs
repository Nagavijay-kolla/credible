using CBH.Chat.Infrastructure.Chat.Constants;
using CBH.Chat.Interfaces.Repository;
using MongoDB.Driver;

namespace CBH.Chat.Repository.ChatRepository
{
    // ReSharper disable once InconsistentNaming
    public class DBClient : IChatDbClient
    {
        public IMongoDatabase Database { get; set; }

        public DBClient(string connectionString)
        {
            var client = new MongoClient(connectionString);

            Database = client.GetDatabase(DbConstants.DatabaseName);
        }
    }
}