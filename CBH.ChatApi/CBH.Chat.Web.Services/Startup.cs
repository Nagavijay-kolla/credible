using CBH.Chat.DependencyInjection;
using CBH.Chat.Domain;
using CBH.Chat.Infrastructure;
using CBH.Chat.Web.Services.DependencyConfig;
using CBH.Chat.Web.Services.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using System.Threading.Tasks;

namespace CBH.Chat.Web.Services
{
    public class Startup
    {
        private const string CrosPolicyName = "CorsPolicy";

        private const string BearerAuthResponseHeader = "Authorization";
        private const string BearerAuthResponseHeaderValue = "Bearer";
        public IConfigurationRoot Configuration { get; }
        private IConfigurationRoot CredibleSettingsConfiguration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
            var credibleSettingsPath = Configuration.GetValue<string>("CredibleSettingsPath");
            var credibleSettingsBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(credibleSettingsPath, false, true);
            CredibleSettingsConfiguration = credibleSettingsBuilder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(CrosPolicyName,builder => 
            builder.WithHeaders("content-type", "authorization")
            .WithMethods(Configuration.GetValue<string>("AllowedMethods").Split(','))
            .AllowAnyOrigin()));

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddFluentValidation();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Chat API", Version = "v1" });
                //c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "CBH.Chat.Web.Services.xml"));
            });

            services.AddOptions();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddModule(new SecurityModule(CredibleSettingsConfiguration));
            services.AddModule(new CacheModule(ConfigurationResolver.GetConfiguration<RedisCacheConfiguration>(Configuration)));
            services.AddModule(new ContextModule(ConfigurationResolver.GetConfiguration<ConnectionStringsConfiguration>(Configuration)));
            services.AddModule(new ServicesModule());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IHttpContextAccessor accessor)
        {
            app.UseCors(CrosPolicyName);
            app.UseAuthentication();
            app.Use(async (context, nextMiddleware) =>
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add(BearerAuthResponseHeader, BearerAuthResponseHeaderValue);

                    return Task.FromResult(0);
                });
                await nextMiddleware();
            });

            env.ConfigureNLog("nlog.config");

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Credible Chat API V1");
                // c.RoutePrefix = "help";
            });

            app.UseMvc();


            app.Run(ctx =>
            {
                ctx.Response.Redirect("/help/index.html");
                return Task.FromResult(0);
            });

        }
    }
}
