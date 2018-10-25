using System;
using System.Collections.Generic;
using KKday.Web.B2D.BE.Models.Model.Discount;

namespace KKday.Web.B2D.BE.Areas.KKday.Models.DataModel.Promotion
{
    [Serializable]
    public class B2dDiscountMstUpdModel
    {
        public Int64 XID { get; set; }
        public string DISC_NO { get; set; } //規則編碼
        public string DISC_NAME { get; set; } //規則名稱
        public string DISC_TYPE { get; set; } //規則類型(01 NET/02 COMM)
        public double DISC_PERCENT { get; set; } //折扣%數
        public DateTime? S_DATE { get; set; } //可使用起日
        public DateTime? E_DATE { get; set; }//可使用迄日
        public string STATUS { get; set; } //規則上下架狀態(00下架/01上架)
        public string RULE_STATUS { get; set; } //引用規則狀態(00不限/01限定)
    }

    [Serializable]
    public class B2dDiscountDtlUpdModel
    {
        public Int64 XID { get; set; } // 折扣明細序號
        public string DISC_TYPE { get; set; } //折扣類型(type1=商品編號/type2=商品分類[M01])
        public string DISC_LIST { get; set; } //折扣類型內容
        public string DISC_LIST_NAME { get; set; } // 折扣類型內容描述
        public string WHITELIST { get; set; } //黑白標記 (0黑/1白)
    }

    [Serializable]
    public class B2dDiscountCurrAmtUpdModel
    {
        public Int64 XID { get; set; } // 外幣金額折扣序號
        public string CURRENCY { get; set; } // 幣別
        public double AMOUNT { get; set; } // 金額
    }
}
