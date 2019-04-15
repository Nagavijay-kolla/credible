using AutoMapper;
using CBH.Chat.Business;
using CBH.Chat.Business.Mappings;
using CBH.Chat.DependencyInjection;
using CBH.Chat.Domain;
using CBH.Chat.Domain.ChatDomains.Models.RequestModels;
using CBH.Chat.Infrastructure.Chat.Validators;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.DependencyInjection;
using CBH.Chat.Interfaces.Repository;
using CBH.Chat.Repository;
using CBH.Chat.Repository.ChatRepository;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CBH.Chat.Web.Services.DependencyConfig
{
    public class ContextModule : BaseModule<ConnectionStringsConfiguration>
    {
        public ContextModule(ConnectionStringsConfiguration configuration) : base(configuration) { }
        public override void Add(IServiceCollection services)
        {
            services.AddDbContext<BaseDbContext>(optionBuilder => optionBuilder.UseSqlServer(Configuration.BaseDbContext));
            services.AddScoped<IBaseDbContext>(provider => provider.GetService<BaseDbContext>());

            services.AddDbContext<ChatUserDbContext>(optionBuilder => optionBuilder.UseSqlServer(Configuration.ChatUserDbContext));
            services.AddScoped<IChatUserDbContext>(provider => provider.GetService<ChatUserDbContext>());

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
                {
                    return null;
                }

                var optionsBuilder = new DbContextOptionsBuilder<PartnerContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return new PartnerContext(optionsBuilder.Options, resolver.GetPartnerId());
            });

            services.AddSingleton<IChatDbClient>(provider => new DBClient(Configuration.ChatDbContext));

            #region Chat
            services.AddSingleton<IChatDbClient>(provider => new DBClient(Configuration.ChatDbContext));
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITeamManager, TeamManager>();
            services.AddTransient<IGroupManager, GroupManager>();
            services.AddTransient<ITeamRepository, TeamRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IThreadManager, ThreadManager>();
            services.AddTransient<IThreadRepository, ThreadRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();

            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(config =>
            {
                config.AddProfile<GroupProfile>();
                config.AddProfile<TeamProfile>();
                config.AddProfile<ThreadProfile>();
            })));

            services.AddTransient<IValidator<NewMessageRequestModel>, NewMessageValidator>();
            services.AddTransient<IValidator<NewBroadcastMessageRequestModel>, NewBroadcastMessageValidator>();
            services.AddTransient<IValidator<NewGroupRequestModel>, NewGroupValidator>();
            services.AddTransient<IValidator<NewUserRequestModel>, NewUserValidator>();
            #endregion
        }
    }
}