using System;
namespace KKday.Web.B2D.EC.Models.Model.Product
{
    public class DiscountRuleModel
    {
        public bool isRule { get; set; }   // 是否有中規則
        public string disc_xid { get; set; }// b2d_discount_mst.xid
        public string disc_name { get; set; }//規則名稱
        public string disc_type { get; set; }//規則類型
        public double? disc_percent { get; set; }//折扣%數
        public string currency { get; set; } // 幣別
        public double? amt { get; set; } // 金額
        public string disc_dtl_xid { get; set; } // dtl_xid


    }
}
