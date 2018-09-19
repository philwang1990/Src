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
        public string multipricePlatform { get; set; }
    }

}
