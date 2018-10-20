using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using log4net;
using log4net.Config;
using Resources;
using KKday.Web.B2D.BE.Models.Model.Common;
using System.Globalization;

namespace KKday.Web.B2D.BE.App_Code
{
    public sealed class Website
    {
        public static readonly Website Instance = new Website();

        public ILog logger { get; private set; }

        // PostgreSql DB 連線方式
        public string SqlConnectionString { get; private set; }
        // 專案底下的 ~/wwwroot/
        public string WebRootPath { get; private set; }

        public string ContenRootPath { get; private set; }  // 專案目錄所在 ~/

        public IConfiguration Configuration { get; private set; }

        public string AesCryptKey { get; private set; }

        public readonly ILog _log = LogManager.GetLogger(typeof(Website));

        public ILocalizer _localizer { get;  private set; }
         

        ////////////////////////////////

        private Website()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void Init(IConfiguration configuration, IHostingEnvironment env)
        {
            this.Configuration = configuration;
            //準備相關工作目錄
            this.WebRootPath = env.WebRootPath;
            this.ContenRootPath = env.ContentRootPath;

            //載入 log4net
            LoadLog4netConfig();

            _log.Debug("StartUp....");

            // 建立資料庫連線
            this.SqlConnectionString = configuration["NPGSQL_Connection"];
            // AES加解密專用Key
            this.AesCryptKey = configuration["AesCryptKey"]; 

        }

        /// <summary>
        /// 載入log4net與Ext.Json
        /// </summary>
        private void LoadLog4netConfig()
        {
            var repository = LogManager.CreateRepository(
                    Assembly.GetEntryAssembly(),
                    typeof(log4net.Repository.Hierarchy.Hierarchy)
                );

            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            log4net.GlobalContext.Properties["hostname"] = Environment.MachineName;
        } 
    }
}
