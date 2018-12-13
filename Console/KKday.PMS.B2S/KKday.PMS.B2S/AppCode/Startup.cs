using System;
using System.IO;
using KKday.PMS.B2S.Models.Shared.Enum;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace KKday.PMS.B2S
{
    public sealed class Startup
    {
        public static readonly Startup Instance = new Startup();
        public IConfigurationBuilder builder;
        public IConfigurationRoot configuration;
        public NpgsqlConnection npg_conn;

        private Startup()
        {
            //
        }

        //StartUp
        public void Initial()
        {
            //appsetting
            builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            this.configuration = builder.Build();

            LoadB2SDBConfig();
        }

        // Postgresql B2S DB 連線方式
        private string _B2S_DB = "";
        public string B2S_DB
        {
            get { return _B2S_DB; }
        }

        // 主機站台識別
        private string _stationID;
        public string StationID
        {
            get { return _stationID; }
        }

        private void LoadB2SDBConfig()
        {
            Console.WriteLine($"pg連線字串：{this.configuration["ConnectionStrings:PostgreSQL"]}");
            this._B2S_DB = configuration["ConnectionStrings:PostgreSQL"];
            this.npg_conn = new NpgsqlConnection(_B2S_DB);
        }

        public string GetParameter(PMSSourse pms, ParameterType parameterType)
        {
            string head = "";

            switch (pms)
            {
                case PMSSourse.Rezdy:
                    head = "PMS_Url";
                    switch (parameterType)
                    {
                        case ParameterType.ApiKey:
                            return this.configuration[$"{head}:Rezdy:apikey"];
                        case ParameterType.ProductSearch:
                            return this.configuration[$"{head}:Rezdy:ProductSearch"];
                        case ParameterType.Product:
                            return this.configuration[$"{head}:Rezdy:Product"];
                        case ParameterType.Pickups:
                            return this.configuration[$"{head}:Rezdy:Pickups"];
                        case ParameterType.Availability:
                            return this.configuration[$"{head}:Rezdy:Availability"];
                        default:
                            return string.Empty;
                    }
                case PMSSourse.KKday:
                    head = "KKdayApi_Url";
                    switch (parameterType)
                    {
                        case ParameterType.KKdayApi_supplierlogin:
                            return this.configuration[$"{head}:supplierlogin"];
                        case ParameterType.KKdayApi_productnew:
                            return this.configuration[$"{head}:productnew"];
                        case ParameterType.KKdayApi_area:
                            return this.configuration[$"{head}:area"];
                        case ParameterType.KKdayApi_countrymodify:
                            return this.configuration[$"{head}:countrymodify"];
                        case ParameterType.KKdayApi_timezone:
                            return this.configuration[$"{head}:timezone"];
                        case ParameterType.KKdayApi_setCostMethod:
                            return this.configuration[$"{head}:setCostMethod"];
                        case ParameterType.KKdayApi_productmodify:
                            return this.configuration[$"{head}:productmodify"];
                        case ParameterType.KKdayApi_updateDate:
                            return this.configuration[$"{head}:updateDate"];
                        case ParameterType.KKdayApi_voucherupdate:
                            return this.configuration[$"{head}:voucherupdate"];
                        case ParameterType.KKdayApi_updatepkg:
                            return this.configuration[$"{head}:update_pkg"];
                        case ParameterType.KKdayApi_calendarextend:
                            return this.configuration[$"{head}:calendar_extend"];
                        case ParameterType.KKdayApi_calendarmodify:
                            return this.configuration[$"{head}:calendar_modify"];
                        case ParameterType.KKdayApi_priceupdate:
                            return this.configuration[$"{head}:price_update"];
                        case ParameterType.KKdayApi_newevent:
                            return this.configuration[$"{head}:new_event"];
                        case ParameterType.KKdayApi_eventstatus:
                            return this.configuration[$"{head}:event_status"];
                        case ParameterType.KKdayApi_pkgstatus:
                            return this.configuration[$"{head}:pkg_status"];
                        case ParameterType.KKdayApi_imageUpload:
                            return this.configuration[$"{head}:imageUpload"];
                        case ParameterType.KKdayApi_modifyImg:
                            return this.configuration[$"{head}:modifyImg"];
                        default:
                            return string.Empty;
                    }
                default:
                    return string.Empty;
            }
        }
    }
}
