using System;
using System.Collections.Generic;
namespace KKday.Web.B2D.EC.Models.Model.Booking.api
{
    public class B2dOrderModel
    {
        //public string order_no { get; set; }//訂單編號
        public string kkday_order_oid { get; set; }//KK訂單編號流水號
        public string kkday_order_mid { get; set; }//KK訂單編號編號
        public string company_xid { get; set; }//分銷商xid
        public string channel_oid { get; set; }//KKday old
        public string booking_type { get; set; }//訂單來源(api/web)
        public DateTime order_date { get; set; }//訂購日期
        public string order_type { get; set; }//訂單類型(b2b)
        public string order_status { get; set; }//訂單狀態
        public double order_amt { get; set; }//總金額
        public double order_b2c_amt { get; set; }//直客總金額
        public string connect_name { get; set; }//訂購人姓名
        public string connect_tel { get; set; }//訂購人聯絡電話
        public string connect_mail { get; set; }//訂購人聯絡信箱
        public string order_note { get; set; }//訂單備註
        public DateTime crt_dateime { get; set; }//成立時間

        public OrderDiscountRule order_discount_rule { get; set; }//order_discount_rule_mst



    }

    //public class Source
    //{
    //    //public string order_no { get; set; }//訂單編號
    //    public string booking_type { get; set; }//訂單來源(api/web)
    //    public int company_xid { get; set; }//分銷商xid
    //    public int channel_oid { get; set; }//KKday old
    //    public string connect_name { get; set; }//訂購人姓名
    //    public string connect_tel { get; set; }//訂購人聯絡電話
    //    public string connect_mail { get; set; }//訂購人聯絡信箱
    //    public string order_note { get; set; }//訂單備註
    //    public string client_ip { get; set; }//IP位置
    //    public string source_pk1 { get; set; }//
    //    public string source_pk2 { get; set; }//
    //    public string source_pk3 { get; set; }//
    //    public string source_pk4 { get; set; }//
    //    public DateTime crt_time { get; set; }//成立時間

    //}

    //public class OrderCus
    //{

    //    //public string order_no { get; set; }//訂單編號
    //    //public int cus_seqno { get; set; }//旅客細項編號
    //    //public int lst_seqno { get; set; }//細項編號
    //    public string cus_type { get; set; }//旅客類別(01成02孩03嬰04老)
    //    public string cus_name_e_last { get; set; }//旅客護照姓
    //    public string cus_name_e_first { get; set; }//旅客護照名
    //    public string cus_sex { get; set; }//旅客性別
    //    public string cus_tel { get; set; }//旅客聯絡電話
    //    public string cus_mail { get; set; }//旅客聯絡信箱


    //}

    //public class OrderLst
    //{
    //    //public string order_no { get; set; }//訂單編號
    //    //public int lst_seqno { get; set; }//細項編號
    //    //public int cus_seqno { get; set; }//旅客細項編號
    //    public string prod_no { get; set; }//商品編號
    //    public string prod_name { get; set; }//商品名稱
    //    public double prod_amt { get; set; }//商品金額
    //    public double prod_b2c_amt { get; set; }//直客商品金額
    //    public string prod_currency { get; set; }//商品金額幣別
    //    //public int discount_xid { get; set; }//折扣xid
    //    public string prod_cond1 { get; set; }//人(price1,price2)
    //    public string prod_cond2 { get; set; }//其他(ex.車)(price1)
    //    public string pkg_no { get; set; }//套餐編號
    //    public string pkg_name { get; set; }//套餐名稱
    //    public string pkg_date { get; set; }//套餐日期
    //    public string events { get; set; }//套餐場次
    //    public string op_status { get; set; }//OP狀態
    //    public string sc_status { get; set; }//SC狀態
    //    public string fa_status { get; set; }//財務狀態
    //    public int prod_qty { get; set; }//商品數量

    //    public OrderDiscountRule order_discount_rule { get; set; }//order_discount_rule_mst


    //}

    public class OrderDiscountRule
    {
        //public int xid { get; set; }//流水號
        //public int lst_seqno { get; set; }//細項編號
        public string disc_name { get; set; }//折扣名稱
        public double disc_amt { get; set; }//折扣金額
        public string disc_currency { get; set; }//折扣幣別
        public string disc_note { get; set; }//折扣備註
        public string order_no { get; set; }//訂單編號

        //public OrderDiscountRuleDtl order_discount_rule_dtl { get; set; }//order_dsicount_rule_dtl

    }

    //public class OrderDiscountRuleDtl
    //{
    //    public int xid { get; set; }//流水號
    //    public int mst_xid { get; set; }//主檔流水號
    //    //public int lst_seqno { get; set; }//細項編號
    //    //public string order_no { get; set; }//訂單編號

    //}


    public class insB2dOrderResult
    {
        public string result { get; set; }
        public string result_msg { get; set; }
        public string order_no { get; set; }
    }


    public class UpdateB2dOrderModel
    {

        public string order_no { get; set; }
        public string order_oid { get; set; }
        public string order_mid { get; set; }
        public string company_xid { get; set; }

    }

    public class updB2dOrderResult
    {
        public string result { get; set; }
        public string result_msg { get; set; }
        public string count { get; set; }
    }

}
