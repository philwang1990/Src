using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.BE.Models.DataModel
{
    public class OrderRQModel
    {
        public Int64 COMP_XID { get; set; }             //分銷商流水號
        public Int64 CHANNEL_OID { get; set; }          //KK分銷商流水號
        public string LOCALE_LANG { get; set; }         //語系
        public string CURRENT_CURRENCY { get; set; }    //幣別
        public string STATE { get; set; }               //國家
        public OrderOptionModel OPTION { get; set; }    //搜尋條件
    }

    public class OrderOptionModel
    {
        public int PAGE_SIZE { get; set; }
        public int CURRENT_PAGE { get; set; }
        public string TIME_ZONE { get; set; }
        public string PROD_SDATE { get; set; }
        public string PROD_EDATE { get; set; }
        public string ORDER_SDATE { get; set; }
        public string ORDER_EDATE { get; set; }
        public string[] ORDERS { get; set; }
    }
}
