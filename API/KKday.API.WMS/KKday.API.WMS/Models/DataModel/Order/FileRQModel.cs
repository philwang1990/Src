using System;
namespace KKday.API.WMS.Models.DataModel.Order {
    public class FileRQModel {
     
        public string apiKey { get; set; }
        public string locale { get; set; }
        public string userOid { get; set; }
        public string ver { get; set; }
        public string ipaddress { get; set; }
        public Json json { get; set; }
    }
    //public class Json {
        //public string orderMid { get; set; }
        //public string orderFileOid { get; set; }

        //public string memberUuid { get; set; }
        //public string deviceId { get; set; }
        //public string tokenKey { get; set; }

        //}
}
