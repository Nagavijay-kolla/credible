using CBH.ChatSignalR.Business;
using CBH.ChatSignalR.DependencyInjection;
using CBH.ChatSignalR.Domain;
using CBH.ChatSignalR.Interfaces.Business;
using CBH.ChatSignalR.Interfaces.DependencyInjection;
using CBH.ChatSignalR.Interfaces.Repository;
using CBH.ChatSignalR.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CBH.ChatSignalR.Web.Services.DependencyConfig
{
    public class ContextModule : BaseModule<ConnectionStringsConfiguration>
    {
        public ContextModule(ConnectionStringsConfiguration configuration) : base(configuration) { }
        public override void Add(IServiceCollection services)
        {
            services.AddDbContext<BaseDbContext>(optionBuilder => optionBuilder.UseSqlServer(Configuration.BaseDbContext));
            services.AddScoped<IBaseDbContext>(provider => provider.GetService<BaseDbContext>());

            services.AddScoped<IPartnerManager, PartnerManager>();
            services.AddScoped<IPartnerConnectionStringResolver>(provider =>
            {
                var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var user = (ApiUserLogin)context.Items["CurrentUser"];
                var partnerDomain = user.Domain;
                var partnerManager = provider.GetService<IPartnerManager>();
                return new PartnerConnectionStringResolver(partnerManager, Configuration.PartnerDbContext, partnerDomain);
            });

            services.AddScoped<IPartnerContext>(provider =>
            {
                var resolver = provider.GetService<IPartnerConnectionStringResolver>();
                var connectionString = resolver.GetPartnerConnectionString();
                if (string.IsNullOrEmpty(connectionString))
                    return null;
                else
                {
                    var optionsBuilder = new DbContextOptionsBuilder<PartnerContext>();
                    optionsBuilder.UseSqlServer(connectionString);
                    return new PartnerContext(optionsBuilder.Options, resolver.GetPartnerId());
                }
            });
        }
    }
}
