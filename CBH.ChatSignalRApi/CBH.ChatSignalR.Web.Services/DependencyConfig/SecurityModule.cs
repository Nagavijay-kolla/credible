using System;
using System.Collections.Generic;
using CBH.Common.Security.Business;
using CBH.Common.Security.Domain;
using CBH.Common.Security.Domain.DTO;
using CBH.Common.Security.Interfaces.Business;
using CBH.Common.Security.Interfaces.Repository;
using CBH.Common.Security.Repository;
using CBH.ChatSignalR.Business;
using CBH.ChatSignalR.DependencyInjection;
using CBH.ChatSignalR.Domain;
using CBH.ChatSignalR.Infrastructure;
using CBH.ChatSignalR.Interfaces.Business;
using CBH.ChatSignalR.Interfaces.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CBH.ChatSignalR.Web.Services.DependencyConfig
{
    public class SecurityModule : BaseModule
    {
        private readonly IConfigurationRoot configuration;

        public SecurityModule(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public override void Add(IServiceCollection services)
        {
            services.AddTransient<IAuthenticationManager, ApiAuthenticationManager>();
            services.AddScoped<ITokenManager>(provider =>
            {
                var secretsHolder = new SecretKeysHolder
                {
                    SecretKey = configuration.GetValue<string>("SecretKeysHolder:SecretKey"),
                    PreviousSecretKey = configuration.GetValue<string>("SecretKeysHolder:PreviousSecretKey"),
                    PreviousSecretKeyExpiryDateUtc = configuration.GetValue<DateTime>("SecretKeysHolder:PreviousSecretKeyExpiryDateUtc")
                };
                return new JwtTokenManager(secretsHolder);
            });

            services.Configure<AuthenticationOptions>(o =>
            {
                o.ExcludedRequestPaths = new List<string>
                    {
                        @"/healthcheck",
                        @"/swagger/",
                        @"/help/CredibleAPITermsofUse121817.pdf",
                        @"/swagger",
                        @"/CBHChatHub"//TODO: IF AUTHENTICATION REQUIRED PLEASE REMOVE THIS LINE FROM HERE
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
