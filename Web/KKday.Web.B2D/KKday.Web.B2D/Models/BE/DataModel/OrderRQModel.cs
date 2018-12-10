using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.BE.Models.DataModel
{
    public class OrderRQModel
    {
        public Int64 company_xid { get; set; }          //分銷商流水號
        public Int64 channel_oid { get; set; }          //KK分銷商流水號
        public string locale_lang { get; set; }         //語系
        public string current_currency { get; set; }    //幣別
        public string state { get; set; }               //國家
        public OrderOptionModel option { get; set; }    //搜尋條件
    }

    public class OrderOptionModel
    {
        public int page_size { get; set; }
        public int current_page { get; set; }
        public string time_zone { get; set; }
        public string prod_Sdate { get; set; }
        public string prod_Edate { get; set; }
        public string order_Sdate { get; set; }
        public string order_Edate { get; set; }
        public string[] orders { get; set; }
    }
}
