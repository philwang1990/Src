using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Models.Repository;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Repostory.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KKday.Web.B2D.BE.App_Code;
using Resources;

namespace KKday.Web.B2D.EC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //Website.Instance.Init(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion Dependency Injection Regisgter-- end

            //使用多國語系
            services.AddSingleton<ILocalizer, Localizer>();

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
            //當用戶輸入的網址找不到時↓
            //app.UseStatusCodePagesWithRedirects("~/404.html"); //或直接給http開頭的絕對URL

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
                   name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // 指定預設Area頁面
                routes.MapAreaRoute(
                    name: "defaultArea",
                    areaName: "User",
                    template: "{controller=Home}/{action=Index}/{id?}");

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
