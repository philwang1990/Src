using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resources;

namespace KKday.Web.B2D.BE
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
             
            // 允用 Local Cache
            services.AddMemoryCache();

            // 指定Cookie授權政策區分不同身分者
            services.AddAuthorization(options =>
            {
                options.AddPolicy("KKdayOnly", policy => policy.RequireClaim("UserType", "KKDAY"));
                options.AddPolicy("UserOnly", policy => policy.RequireClaim("UserType", "USER"));
            });

            #region Dependency Injection Regisgter -- begin

            services.AddSingleton<AccountRepository>();
            services.AddSingleton<IB2dAccountRepository, B2dAccountRepository>();
            services.AddSingleton<IB2dAccountRepository, B2dApiAccountRepository>();
            services.AddSingleton<ListPriceRepository>();
            services.AddSingleton<CompanyRepository>();
            services.AddSingleton<PriceSettingRepository>();
            services.AddSingleton<CommonRepository>();
            services.AddSingleton<VouchAddonRepository>();
            services.AddSingleton<FixedPriceRepository>();

            #endregion Dependency Injection Regisgter-- end

            //使用多國語系
            services.AddSingleton<ILocalizer, Localizer>();

            //使用 Session
            services.AddSession();

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddSessionStateTempDataProvider();
           
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
             
            // 初始化-網站主控台
            Website.Instance.Init(this.Configuration, env);
             
            // 異常頁面處理, 走 RazorPage 模式(目錄=>"\Pages\Errors\")
            app.UseStatusCodePages(context => {
                // var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;
                response.Redirect("/Errors/" + response.StatusCode);

                return Task.CompletedTask;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            // 啟用 Cookie 使用者驗證
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //name: "default",
                //template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                   name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // 指定預設Area頁面
                routes.MapAreaRoute(
                    name: "defaultArea",
                    areaName: "User",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
