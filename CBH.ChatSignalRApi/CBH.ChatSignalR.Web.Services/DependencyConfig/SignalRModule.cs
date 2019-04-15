using CBH.ChatSignalR.DependencyInjection;
using CBH.ChatSignalR.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CBH.ChatSignalR.Web.Services.DependencyConfig
{
    public class SignalRModule : BaseModule<RedisCacheConfiguration>
    {
        public SignalRModule(RedisCacheConfiguration configuration) : base(configuration) { }

        public override void Add(IServiceCollection services)
        {
            if(Convert.ToBoolean(Configuration.IsStickySessionEnabled))
            {

                //TODO: ADD required config here,
                services.AddSignalR(c => c.EnableDetailedErrors = true).AddStackExchangeRedis(Configuration.ServerAddress,
                    options => {
                        options.Configuration.ClientName = Configuration.ApplicationName;
                    }
                    );
            }
            else
            {
                services.AddSignalR(c => c.EnableDetailedErrors = true);
            }
        }
    }
}
