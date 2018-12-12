using System;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Order
{
    public class QueryOrderModel
    {
        public Int64 company_xid { set; get; }
        public Int64 channel_oid { set; get; }
        public string locale_lang { get; set; }
        public string current_currency { get; set; }
        public string state { get; set; }

        public Option option { get; set; }

    }
    public class Option
    {
        public int page_size { get; set; }
        public int current_page { get; set; }
        public string time_zone { get; set; }
        public string prod_Sdate { get; set; }
        public string prod_Edate { get; set; }
        public string order_Sdate { get; set; }
        public string order_Edate { get; set; }
        public string[] kkday_orders { get; set; }
        public List<string>  orders{ get; set; }
    }
}
