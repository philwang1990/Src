using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IS4.API.DEMO.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IS4.API.DEMO {
    public class Startup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        //获取应用程序的当前工作目录。  
        string LocaPath = Directory.GetCurrentDirectory();  

        public void ConfigureServices(IServiceCollection services) {
            services.AddIdentityServer()
                    // .AddDeveloperSigningCredential()
                //.AddSigningCredential(new X509Certificate2(@"/etc/nginx/ssl/socialnetwork.pfx", "2wsx3edc"))

               .AddSigningCredential(new X509Certificate2(LocaPath + "/Pfx/socialnetwork.pfx", "2wsx3edc"))
               .AddTestUsers(InMemoryConfiguration.Users().ToList())
               .AddInMemoryClients(InMemoryConfiguration.Clients())
               .AddInMemoryApiResources(InMemoryConfiguration.ApiResources());


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseDeveloperExceptionPage();
            app.UseIdentityServer();
        }
    }
}
