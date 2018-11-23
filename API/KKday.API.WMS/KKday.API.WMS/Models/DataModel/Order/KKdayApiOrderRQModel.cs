using System;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Order
{
    public class KKdayApiOrderRQModel
    {
        public string apiKey { get; set; }
        public string locale { get; set; }
        public string userOid { get; set; }
        public string ver { get; set; }
        public string ipaddress { get; set; }
        public Json json { get; set; }
    }
    public class Json
    {
        public int pageSize { get; set; }
        public int currentPage { get; set; }
        public string memberUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }


        //b2d查詢條件
        public Int64 channelOid { get; set; }
        public string timeZone { get; set; }
        public string begLstGoDt { get; set; }
        public string endLstGoDt { get; set; }
        public string begCrtDt { get; set; }
        public string endCrtDt { get; set; }
        public string[] orderMids { get; set; }
    }
}
