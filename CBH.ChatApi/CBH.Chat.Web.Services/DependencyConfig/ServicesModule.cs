using AutoMapper;
using CBH.Chat.Business;
using CBH.Chat.Business.Mappings;
using CBH.Chat.DependencyInjection;
using CBH.Chat.Domain;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.Repository;
using CBH.Chat.Repository.ChatRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CBH.Chat.Web.Services.DependencyConfig
{
    public class ServicesModule : BaseModule
    {
        public override void Add(IServiceCollection services)
        {
            #region Chat
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITeamManager, TeamManager>();
            services.AddTransient<IGroupManager, GroupManager>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IThreadManager, ThreadManager>();
            services.AddTransient<IThreadRepository, ThreadRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<IPartnerRepository>(provider =>
            {
                var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var dbcontext = provider.GetRequiredService<IPartnerContext>();
                var user = (ApiUserLogin)context.Items["CurrentUser"];
                var partnerDomain = user.Domain;
                return new PartnerRepository(dbcontext, partnerDomain);
            });
            services.AddTransient<IUserConfigurationManager, UserConfigurationManager>();
            services.AddTransient<IContactManager, ContactManager>();


            services.AddTransient<IMessageRepository>(provider =>
            {
                var archivalDays = provider.GetRequiredService<IPartnerRepository>().GetPartnerArchivalDays();
                var dbClient = provider.GetRequiredService<IChatDbClient>();
                return new MessageRepository(dbClient, archivalDays <= 0 ? 90 : archivalDays);
            });
            services.AddTransient<ILogManager, LogManager>();



            services.AddTransient<IMessageRepository>(provider =>
            {
                var archivalDays = provider.GetRequiredService<IPartnerRepository>().GetPartnerArchivalDays();
                var dbClient = provider.GetRequiredService<IChatDbClient>();
                return new MessageRepository(dbClient, archivalDays <= 0 ? 90 : archivalDays);
            });


            services.AddSingleton<IMapper>(provider =>
            {
                return new Mapper(new MapperConfiguration(config =>
                {
                    config.AddProfile<GroupProfile>();
                    config.AddProfile<TeamProfile>();
                    config.AddProfile<ThreadProfile>();
                    config.AddProfile<MessageProfile>();
                    config.AddProfile<ContactProfile>();
                    config.AddProfile(new LogProfile(provider.GetService<IContactRepository>(), provider.GetService<ITeamRepository>()
                    , provider.GetService<IGroupRepository>()));
                }));
            });


            #endregion
        }
    }
}
