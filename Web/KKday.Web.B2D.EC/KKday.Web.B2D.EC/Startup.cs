using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace KKday.Web.B2D.EC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Website.Instance.Init(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();



            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                // name: "about",
                // template: "about",
                // defaults: new { controller = "Home", action = "Index" }
                //);


                routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                  name: "Product",
                     template: "Product/{id?}",
                     defaults: new { controller = "Product", action = "Index" },
                     constraints: new { id = @"\d+" }
                );

                routes.MapRoute(
                  name: "Booking",
                     template: "Booking/{guid?}",
                     defaults: new { controller = "Booking", action = "Index" }
                );

            });

            //app.Run(ctx =>
            //{
            //    ctx.Response.Redirect("/Product/17379"); //可以支持虚拟路径或者index.html这类起始页.
            //    return Task.FromResult(0);
            //});

        }
    }
}
