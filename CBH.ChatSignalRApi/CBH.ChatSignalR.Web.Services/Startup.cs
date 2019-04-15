using System.Threading.Tasks;
using CBH.ChatSignalR.Business;
using CBH.ChatSignalR.DependencyInjection;
using CBH.ChatSignalR.Domain;
using CBH.ChatSignalR.Web.Services.DependencyConfig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

namespace CBH.ChatSignalR.Web.Services
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private IConfigurationRoot CredibleSettingsConfiguration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
            CredibleSettingsConfiguration = Configuration;
            /*
            var credibleSettingsPath = Configuration.GetValue<string>("CredibleSettingsPath");
            var credibleSettingsBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(credibleSettingsPath, optional: false, reloadOnChange: true);
            CredibleSettingsConfiguration = credibleSettingsBuilder.Build();
            */
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var credinleDomain = Configuration.GetValue<string>("CredibleDomain");
            services.AddLogging();
            if (!string.IsNullOrEmpty(credinleDomain))
            {
                services.AddCors(options =>
                {
                options.AddPolicy("SignalrCors", builder => builder.WithOrigins(credinleDomain).AllowCredentials().AllowAnyHeader().AllowAnyMethod());
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("SignalrCors", builder => builder.AllowCredentials().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
                });
            }

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Template API", Version = "v1" });
            });

            services.AddOptions();

            // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // services.AddModule(new SecurityModule(CredibleSettingsConfiguration));
            //services.AddModule(new CacheModule(ConfigurationResolver.GetConfiguration<RedisCacheConfiguration>(Configuration)));
            services.AddModule(new ContextModule(ConfigurationResolver.GetConfiguration<ConnectionStringsConfiguration>(Configuration)));
            services.AddModule(new SignalRModule(ConfigurationResolver.GetConfiguration<RedisCacheConfiguration>(Configuration)));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IHttpContextAccessor accessor)
        {
            loggerFactory.AddConsole();

            app.UseCors("SignalrCors");
            //MiddlewareExtensions.UseAuthentication(app);

            env.ConfigureNLog("nlog.config");

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/Template/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = "help";
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalRHub>("/CBHChatHub");
            });

            app.UseMvc();

            app.Run(ctx =>
            {
                ctx.Response.Redirect("/Template/help/index.html");
                return Task.FromResult(0);
            });

        }
    }
}