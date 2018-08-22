using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IS4.API.DEMO.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using IdentityServer4;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using KKday.API.IS4.Server.Models.Repository;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using KKday.API.IS4.Server;
using System.IdentityModel.Tokens.Jwt;
using IdentityServer4.AccessTokenValidation;

namespace IS4.API.DEMO {
    public class Startup {

        //获取应用程序的当前工作目录。  
        string LocaPath = Directory.GetCurrentDirectory();

        public Startup(IConfiguration configuration) {

            //var builder = new ConfigurationBuilder()
            //.SetBasePath(env.ContentRootPath)
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            //.AddEnvironmentVariables();

            //Configuration = builder.Build();
            Configuration = configuration;
            Website.Instance.Init(configuration);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {

            //my user repository
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddIdentityServer()
               .AddDeveloperSigningCredential()
              // .AddSigningCredential(new X509Certificate2(@"/etc/nginx/ssl/socialnetwork.pfx", "2wsx3edc"))
              //.AddSigningCredential(new X509Certificate2(@"/Users/jiangzhimin/socialnetwork.pfx", "2wsx3edc"))
              //   .AddSigningCredential(new X509Certificate2(LocaPath + "/Pfx/socialnetwork.pfx", "2wsx3edc"))
              // .AddTestUsers(InMemoryConfiguration.Users().ToList())
              .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources())
              .AddInMemoryClients(InMemoryConfiguration.Clients())
              .AddInMemoryApiResources(InMemoryConfiguration.ApiResources())
              .AddProfileService<ProfileService>();


            //Inject the classes we just created
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();//再做一次 像new

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {


            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

            }
            // app.UseDeveloperExceptionPage();
            app.UseIdentityServer();

            // JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //IdentityServerAuthenticationOptions identityServerValidationOptions = new IdentityServerAuthenticationOptions {
            //    //move host url into appsettings.json
            //    Authority = "http://localhost:50000/",
            //    ApiSecret = "secret",
            //    ApiName = "my.api.resource",
            //    AutomaticAuthenticate = true,
            //    SupportedTokens = SupportedTokens.Both,

            //    // required if you want to return a 403 and not a 401 for forbidden responses
            //    AutomaticChallenge = true,

            //    //change this to true for SLL
            //    RequireHttpsMetadata = false
            //};
            //app.UseIdentityServerAuthentication(identityServerValidationOptions);



        }
    }
}
