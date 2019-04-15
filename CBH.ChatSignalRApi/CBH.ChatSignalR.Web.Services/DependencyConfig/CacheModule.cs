using CBH.Common.RedisCache;
using CBH.Common.RedisCache.Configuration;
using CBH.ChatSignalR.DependencyInjection;
using CBH.ChatSignalR.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CBH.ChatSignalR.Web.Services.DependencyConfig
{
    public class CacheModule : BaseModule<RedisCacheConfiguration>
    {
        public CacheModule(RedisCacheConfiguration configuration) : base(configuration) { }
        public override void Add(IServiceCollection services)
        {
            services.AddSingleton<ICacheConnection>(p => new RedisConnectionManager(new RedisSettings
            {
                ApplicationName = Configuration.ApplicationName,
                RedisAddress = Configuration.ServerAddress
            }));
            services.AddSingleton<ICacheService>(p => new RedisCacheService(p.GetService<ICacheConnection>()));
        }
    }
}
