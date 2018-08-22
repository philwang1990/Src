// using log4net;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.IO;
using Npgsql;
using System;

//using log4net;

/// <summary>
/// Summary description for Website
/// </summary>
public sealed class Website {
    public static readonly Website Instance = new Website();

    private IConfiguration _Configuration;
    public IConfiguration Configuration {
        get { return _Configuration; }
    }

    // Postgresql IS4 DB 連線方式
    private string _IS4_DB = "";
    public string IS4_DB {
        get { return _IS4_DB; }
    }

    public log4net.ILog logger {
        get;
        private set;
    }

    // 主機站台識別
    private string _stationID;
    public string StationID {
        get { return _stationID; }
    }

    /// <summary>
    ///   Constructor
    /// </summary>
    /// 
    private Website() {
        //
        // TODO: Add constructor logic here
        //
    }

    public void Init(IConfiguration config) {

        try {

            this._Configuration = config;

            Console.WriteLine($"pg連線字串：{Configuration["ConnectionStrings:PostgreSQL"]}");
            this._IS4_DB = Configuration["ConnectionStrings:PostgreSQL"];

            string szLog4NetCfgFile = string.Format("{0}\\log4net.config", Directory.GetCurrentDirectory());

            _stationID = Dns.GetHostName();

            NpgsqlConnection npg_conn = new NpgsqlConnection(_IS4_DB);
            //NpgsqlTransaction npg_tran = null;

            //Initialize log4net;
            //var logRepository = LogManager.GetRepository( System.Reflection.Assembly.GetEntryAssembly());
            //log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo(szLog4NetCfgFile));

            //_logger = LogManager.GetLogger(logRepository.Name, "logger");
            //logger.Info("Webiste.Initialized .....");


            //string dd = Configuration["oConnectionStrings:SqlConnectionString"];
            //string ddd = "select * from users";
            //using (OracleConnection cn = new OracleConnection(dd))
            //{
            //    cn.Open();
            //    DataSet ds = OracleHelper.ExecuteDataset(cn, CommandType.Text, ddd);
            //}

        } catch (Exception ex) {

            Console.WriteLine($"{ex.Message} {ex.StackTrace} {Website.Instance.IS4_DB}");

        }

    }
}