namespace CBH.Chat.Domain
{
    public abstract class Configuration
    {
    }

    public class ConnectionStringsConfiguration : Configuration
    {
        public string PartnerDbContext { get; set; }
        public string BaseDbContext { get; set; }
        public string ChatDbContext { get; set; }
        public string ChatUserDbContext { get; set; }
    }
    public class RedisCacheConfiguration : Configuration
    {
        public string ServerAddress { get; set; }
        public string ApplicationName { get; set; }
    }
}
