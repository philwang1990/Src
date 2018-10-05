using System;
namespace KKday.API.WMS.Models.DataModel.Product
{
   
    public class KKdayApiProdRQModel
    {
        public string apiKey { get; set; }
        public string userOid { get; set; }
        public string ver { get; set; }
        public string locale { get; set; }
        public string currency { get; set; }
        public string ipaddress { get; set; }
        public Json json { get; set; }
    }

    public class Json
    {
        public string infoType { get; set; }
        public string cleanCache { get; set; }
        public string multipricePlatform { get; set; }

        //套餐用
        public string pkgStatus { get; set; }
        public string pkgOid { get; set; }

        //旅規用
        public string deviceId { get; set; }
        public string tokenKey { get; set; }
        public string[] moduleTypes { get; set; }

    }

}
