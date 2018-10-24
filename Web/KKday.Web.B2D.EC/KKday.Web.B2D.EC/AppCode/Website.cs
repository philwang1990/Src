using System;
using System.Data;
using System.Configuration;
using System.Web;
using log4net;
using System.Net;
using System.Xml;
using System.IO;
using System.Collections;
using Microsoft.Extensions.Configuration;
using log4net.Config;
using System.Reflection;
using Npgsql;


public sealed class Website
{
    public static readonly Website Instance = new Website();

    // Postgresql ERP DB 連線方式
    private string _ERP_DB = "";
    public string ERP_DB
    {
        get { return _ERP_DB; }
    }

    // DI for Configuration (appsettings.cs)
    public IConfiguration Configuration { get; private set; }
    public IServiceProvider ServiceProvider { get; private set; }
    public readonly ILog logger = LogManager.GetLogger(typeof(Website));

    // 主機站台識別
    private string _stationID;
    public string StationID
    {
        get { return _stationID; }
    }

    private Website()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void Init(IConfiguration config)
    {
        this.Configuration = config;
        this._ERP_DB = Configuration["ConnectionStrings:PostgreSQL"];

        _stationID = Dns.GetHostName();

        NpgsqlConnection npg_conn = new NpgsqlConnection(_ERP_DB);
       
        LoadLog4netConfig();

        logger.Debug("StartUp....!");
    }

    private void LoadLog4netConfig()
    {
        string logPath = Configuration["log4netPath:path"];

        var repository = LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                 typeof(log4net.Repository.Hierarchy.Hierarchy)
             );

        XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        log4net.GlobalContext.Properties["hostname"] = Environment.MachineName;


        // var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
        Array.ForEach(repository.GetAppenders(), appender =>
        {
            // Check appsetting.json => log4net.Appender.Name is "RollingFile"
            if (appender.Name.Equals("RollingFile") &&
                 appender.GetType() == typeof(log4net.Appender.RollingFileAppender))
            {
                Console.WriteLine(appender.Name);
                ((log4net.Appender.RollingFileAppender)appender).File = logPath;
                ((log4net.Appender.RollingFileAppender)appender).ActivateOptions();
            }
        });
    }
}

