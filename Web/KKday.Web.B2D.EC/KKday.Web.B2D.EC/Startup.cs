﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Repostory.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            // 新增 Cookie 驗證服務
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = ".B2D.User.SharedCookie";
                    options.LoginPath = "/Login/";
                    // options.Cookie.Domain = "kkday.com";

                    options.Events.OnValidatePrincipal = (context) =>
                    {
                        return Task.CompletedTask;
                    };

                });

            // 指定Cookie授權政策區分不同身分者
            services.AddAuthorization(options =>
            {
                options.AddPolicy("KKdayOnly", policy => policy.RequireClaim("UserType", "KKDAY"));
                options.AddPolicy("UserOnly", policy => policy.RequireClaim("UserType", "USER", "ADMIN"));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRedisHelper, RedisHelper>();
            services.AddSingleton<AccountRepository>();  //

            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddSessionStateTempDataProvider();

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
            app.UseSession();

            // 啟用 Cookie 使用者驗證
            app.UseAuthentication();

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
