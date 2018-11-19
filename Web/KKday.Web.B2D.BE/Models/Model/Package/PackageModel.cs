using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.BE.Models.Model.Package
{
    // KKday 商品套餐(Package) 

    public class KKdayPackage
    {

        public string pkg_no { get; set; }
        public string pkg_name { get; set; }
        public List<DescItem> desc_items { get; set; }
        public string online_s_date { get; set; }
        public string online_e_date { get; set; }
        public string weekDays { get; set; }
        public string is_unit_pirce { get; set; }
        public int price1 { get; set; }
        public double price1_org { get; set; }
        public int price1_org_net { get; set; }
        public int price1_profit_rate { get; set; }
        public int price1_comm_rate { get; set; }
        public string price1_age_range { get; set; }
        public int price1_b2c { get; set; }
        public int price2 { get; set; }
        public double price2_org { get; set; }
        public int price2_org_net { get; set; }
        public double price2_profit_rate { get; set; }
        public int price2_comm_rate { get; set; }
        public string price2_age_range { get; set; }
        public int price2_b2c { get; set; }
        public int price3 { get; set; }
        public int price3_org { get; set; }
        public int price3_org_net { get; set; }
        public int price3_profit_rate { get; set; }
        public int price3_comm_rate { get; set; }
        public string price3_age_range { get; set; }
        public int price3_b2c { get; set; }
        public int price4 { get; set; }
        public int price4_org { get; set; }
        public int price4_org_net { get; set; }
        public int price4_profit_rate { get; set; }
        public int price4_comm_rate { get; set; }
        public string price4_age_range { get; set; }
        public int price4_b2c { get; set; }
        public string status { get; set; }
        public int norank_min_book_qty { get; set; }
        public int norank_max_book_qty { get; set; }
        public int rank_min_book_qty { get; set; }
        public int min_orverage_qty { get; set; }
        public string isMultiple { get; set; }
        public string book_qty { get; set; }
        public string unit { get; set; }
        public string unit_txt { get; set; }
        public int unit_qty { get; set; }
        public string pickupTp { get; set; }
        public string pickupTpTxt { get; set; }
        public string is_hl { get; set; }
        public string is_event { get; set; }
        public ModuleSetting module_setting { get; set; } 
    }

    public class DescItem
    {
        public string desc { get; set; }
        public string id { get; set; }
    }

    public class FlightInfoType
    {
        public string value { get; set; }
    }

    public class SendInfoType
    {
        public string value { get; set; }
        public string country_code { get; set; }
    }

    public class AfterOrderDate
    {
        public string unit { get; set; }
    }

    public class VoucherValidInfo
    {
        public string valid_period_type { get; set; }
        public string before_specific_date { get; set; }
        public AfterOrderDate after_order_date { get; set; }
    }

    public class ModuleSetting
    {
        public FlightInfoType flight_info_type { get; set; }
        public SendInfoType send_info_type { get; set; }
        public VoucherValidInfo voucher_valid_info { get; set; }
    }
}
