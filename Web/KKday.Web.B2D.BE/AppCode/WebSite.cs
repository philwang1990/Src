using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KKday.Web.B2D.BE.App_Code
{
    public sealed class Website
    {
        public static readonly Website Instance = new Website();

        // PostgreSql DB 連線方式
        public string SqlConnectionString { get; private set; }

        public string WebRootPath { get; private set; } // 專案底下的 ~/wwwroot/

        public string ContenRootPath { get; private set; }  // 專案目錄所在 ~/

        public IConfiguration Configuration { get; private set; }

        private Website()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void Init(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Configuration = configuration;

            this.WebRootPath = env.WebRootPath;
            this.ContenRootPath = env.ContentRootPath;

            // 建立資料庫連線
            this.SqlConnectionString = configuration["NPGSQL_Connection"];

        }
    }
}
