namespace CBH.ChatSignalR.Domain
{
    public abstract class Configuration
    {
    }

    public class ConnectionStringsConfiguration : Configuration
    {
        public string PartnerDbContext { get; set; }
        public string BaseDbContext { get; set; }
    }
    public class RedisCacheConfiguration : Configuration
    {
        public string ServerAddress { get; set; }
        public string ApplicationName { get; set; }
        public string IsStickySessionEnabled { get; set; }
    }
}
