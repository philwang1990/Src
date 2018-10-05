using System;
namespace KKday.API.WMS.Models.DataModel.Product
{
   
    public class KKdayApiProdRequestModel
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

        //套餐用
        public string pkgStatus { get; set; }
        public string pkgOid { get; set; }

        //套餐可售日期用
        public string prodOid { get; set; }  
        public string rtnMonth { get; set; }

        //套餐場次用
        public string begDt { get; set; }
        public string endDt { get; set; }


        public string multipricePlatform { get; set; }
    }

}
