﻿using System;
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

        //套餐可售日期用
        public string prodOid { get; set; }  
        public string rtnMonth { get; set; }

        //套餐場次用
        public string begDt { get; set; }
        public string endDt { get; set; }


        public string multipricePlatform { get; set; }

        //套餐用
        public string pkgStatus { get; set; }
        public string pkgOid { get; set; }

        //旅規用
        public string deviceId { get; set; }
        public string tokenKey { get; set; }
        public string[] moduleTypes { get; set; }

        public string state { get; set; }
        //分銷商的國家 會用在商品是否顯示 
        //例：商品設定為TW,CN 那只有來自台灣、大陸的分銷商才會看到該商品


    }

}
