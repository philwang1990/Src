using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotGetway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //ocelot
            services.AddOcelot(new ConfigurationBuilder()
                .AddJsonFile("configuration.json")
                .Build());

            var authenticationProviderKey = "TestKey";
            Action<IdentityServerAuthenticationOptions> options = o => {
                o.Authority = "http://192.168.2.83";
                o.ApiName = "socialnetwork";
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = "secret";
            };

            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, options);







        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            await app.UseOcelot();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
