using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using KKday.API.WMS.AppCode;
using Microsoft.AspNetCore.Http;
using KKday.API.WMS.Models.Repository.Product;
using KKday.API.WMS.Models.Repository.Booking;
using KKday.API.WMS.Models.Repository;

namespace KKday.API.WMS {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
            Website.Instance.Init(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //超強烈推薦使用語法！！！
            //model不給值JSON不會吐出
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
              // options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            });

            services.AddSingleton<IRedisHelper, RedisHelper>();
            services.AddSingleton<SearchRepository, SearchRepository>();//註冊搜尋服務
            services.AddSingleton<OrderRepository, OrderRepository>();//註冊查訂單服務
            services.AddSingleton<ProductRepository, ProductRepository>();//註冊搜尋單一商品服務
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
