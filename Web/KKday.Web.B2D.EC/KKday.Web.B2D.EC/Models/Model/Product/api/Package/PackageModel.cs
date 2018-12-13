using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.EC.Models.Model.Product
{
    public class PackageModel
    {

        public string result { get; set; }
        public string result_msg { get; set; }
        public string cost_calc_type { get; set; }
        //成本計算方式/costCalcMethod ex:NET >> PMS會需要

        public PkgSaleDateModel sale_dates { get; set; } //套餐可售日期
        public List<PkgDetailModel> pkgs { get; set; }
        public DiscountRuleModel discount_rule { get; set; }

        public string guid { get; set; }
    }

    public class PkgDetailModel
    {
        public string pkg_no { get; set; }
        public string pkg_name { get; set; }
        public List<DescItem> desc_items { get; set; }
        public string online_s_date { get; set; }//套餐上架起日
        public string online_e_date { get; set; }//套餐上架迄日
        public string weekDays { get; set; }//可販售星期 逗號區隔

        public string is_unit_pirce { get; set; }//價格別均一價(NORANK,RANK)
        public double price1 { get; set; }//當下幣別價price1
        public double price1_org { get; set; } //商品原幣別
        public double price1_org_net { get; set; }//商品原幣成本價
        public double price1_profit_rate { get; set; }//毛利率
        public double price1_comm_rate { get; set; } //傭金率
        public string price1_age_range { get; set; }//年紀範圍/price1BegOId, price1EndOId 逗號區隔
        public double price1_net { get; set; } //當下幣別-同業價
        public double price1_list { get; set; }//當下幣別-牌價
        public double price1_cost { get; set; } //當下幣別-成本價
        public double price1_b2c { get; set; }//price_sale

        public double price2 { get; set; }
        public double price2_org { get; set; }
        public double price2_org_net { get; set; }
        public double price2_profit_rate { get; set; }
        public double price2_comm_rate { get; set; }
        public string price2_age_range { get; set; }
        public double price2_net { get; set; } //當下幣別價-同業價
        public double price2_list { get; set; }//當下幣別-牌價
        public double price2_cost { get; set; } //當下幣別-成本價
        public double price2_b2c { get; set; }//price_sale

        public double? price3 { get; set; }
        public double? price3_org { get; set; }
        public double? price3_org_net { get; set; }
        public double? price3_profit_rate { get; set; }
        public double? price3_comm_rate { get; set; }
        public string price3_age_range { get; set; }
        public double? price3_net { get; set; } //當下幣別-同業價
        public double? price3_list { get; set; }//當下幣別-牌價
        public double? price3_cost { get; set; } //當下幣別-成本價 
        public double? price3_b2c { get; set; }//price_sale

        public double? price4 { get; set; }
        public double? price4_org { get; set; }
        public double? price4_org_net { get; set; }
        public double? price4_profit_rate { get; set; }
        public double? price4_comm_rate { get; set; }
        public string price4_age_range { get; set; }
        public double? price4_net { get; set; } //當下幣別-同業價
        public double? price4_list { get; set; }//當下幣別-牌價
        public double? price4_cost { get; set; } //當下幣別-成本價 
        public double? price4_b2c { get; set; }//price_sale

        public string status { get; set; } //狀態（上架：Y,下架：N）
        //public int min_book_qty { get; set; }//最少訂購數量
        //public int max_book_qty { get; set; }//最多訂購數量
        public int norank_min_book_qty { get; set; }//最少訂購數量
        public int norank_max_book_qty { get; set; }//最多訂購數量
        public int rank_min_book_qty { get; set; } //minOrderQty
        public int min_overage_qty { get; set; } //minOrderAdultQty
        public string isMultiple { get; set; }//是否最少購買量的倍數/ default = Y // N
        public string book_qty { get; set; }//可訂購數量
        public string unit { get; set; }//套餐單位
        public string unit_txt { get; set; }// 套餐單位翻譯
        public int unit_qty { get; set; }// 套餐單位人數
        public string pickupTp { get; set; } //送方式代碼當商品類別 >>挖字用
        public string pickupTpTxt { get; set; }//接送方式翻譯資料
        public string is_hl { get; set; }//是否候補/isBackUp Y/N
        public string is_event { get; set; }//是否有場次 Y/N

        public ModuleSetting module_setting { get; set; }//商品模組化設定


        //這是為了前瑞顯示而加的--Phil *******
        public string chkDateCanSell { get; set; } //如有日期進來能不能賣 1上架日期可賣 2上架日期不可賣 3己售罄
        public string NoSellTextShow { get; set; } //2 3 用的字眼
        public PkgDateforEcModel pkgDate { get; set; } 

    }

    public class DescItem
    {

        public string desc { get; set; }
        public string id { get; set; }

    }

    public class ModuleSetting
    {
        public FlightInfoType flight_info_type { get; set; }
        public SendInfoType send_info_type { get; set; }
        public VoucherValidInfo voucher_valid_info { get; set; }
    }

    public class FlightInfoType
    {
        public string value { get; set; }
        //00：預設  01：送機(depart)去機場  02：接機(arrive)去市區 
        //03：接機+送機(arrive+ depart)   04:送機+接機(不會有這個商品吧XD)
    }

    public class SendInfoType
    {
        public string value { get; set; }//00:不需寄送  01:寄送到飯店  02:寄送到指定國家
        public string country_code { get; set; }
    }

    public class VoucherValidInfo
    {
        public string valid_period_type { get; set; }
        public string before_specific_date { get; set; }
        public AfterOrderDate after_order_date { get; set; }
    }

    public class AfterOrderDate
    {
        public int qty { get; set; }
        public string unit { get; set; }
    }
}
