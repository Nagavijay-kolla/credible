using System;
using System.Collections.Generic;
using CBH.Chat.Business;
using CBH.Chat.DependencyInjection;
using CBH.Chat.Domain;
using CBH.Chat.Infrastructure;
using CBH.Chat.Interfaces.Business;
using CBH.Chat.Interfaces.DependencyInjection;
using CBH.Common.Security.Business;
using CBH.Common.Security.Domain;
using CBH.Common.Security.Domain.DTO;
using CBH.Common.Security.Interfaces.Business;
using CBH.Common.Security.Interfaces.Repository;
using CBH.Common.Security.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CBH.Chat.Web.Services.DependencyConfig
{
    public class SecurityModule : BaseModule
    {
        private readonly IConfigurationRoot _configuration;

        public SecurityModule(IConfigurationRoot configuration)
        {
            this._configuration = configuration;
        }

        public override void Add(IServiceCollection services)
        {
            services.AddTransient<IAuthenticationManager, ApiAuthenticationManager>();
            services.AddScoped<ITokenManager>(provider =>
            {
                var secretsHolder = new SecretKeysHolder
                {
                    SecretKey = _configuration.GetValue<string>("SecretKeysHolder:SecretKey"),
                    PreviousSecretKey = _configuration.GetValue<string>("SecretKeysHolder:PreviousSecretKey"),
                    PreviousSecretKeyExpiryDateUtc = _configuration.GetValue<DateTime>("SecretKeysHolder:PreviousSecretKeyExpiryDateUtc")
                };
                return new JwtTokenManager(secretsHolder);
            });

            services.Configure<AuthenticationOptions>(o =>
            {
                //TODO: Add Api Path to disable Authentication
                o.ExcludedRequestPaths = new List<string>
                    {
                        @"/healthcheck",
                        @"/swagger/",
                        @"/help/CredibleAPITermsofUse121817.pdf",
                        @"/swagger",
                        @"/chat",
                        @"/group",
                        @"/team",
                        @"/user",
                        @"/chatservice"
                    };
                o.CurrentApplication = Applications.IntegratedCare;
            });
            services.AddTransient<IProfileManager, ProfileManager>();
            services.AddScoped<IPartnerManager, PartnerManager>();

            services.AddScoped<ISecurityContext>(provider =>
            {
                var connectionString = provider.GetService<IPartnerConnectionStringResolver>().GetPartnerConnectionString();
                var optionsBuilder = new DbContextOptionsBuilder<SecurityContext>();
                optionsBuilder.UseSqlServer(connectionString);
                var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var user = (ApiUserLogin)context.Items["CurrentUser"];
                var partnerDomain = user.Domain;
                return new SecurityContext(optionsBuilder.Options, partnerDomain, 0);
            });
        }
    }
}