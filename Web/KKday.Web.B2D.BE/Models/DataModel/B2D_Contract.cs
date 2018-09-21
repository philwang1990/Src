using System;
namespace KKday.Web.B2D.BE.Models.DataModel
{
    public class B2D_Contract
    {
        public int XID { get; set; }          //合約流水號
        public int COMPANY_XID { get; set; }  //所屬公司流水號
        public int STATUS { get; set; }       //審核狀態(00待審中/01已核准/02審核未通過/03待補件)

        public DateTime S_DATE { get; set; }  //合約起始日
        public DateTime E_DATE { get; set; }  //合約終止日
        public bool IS_RENEW { get; set; }    //是否自動展延(Y/N)

        public float EST_AMT { get; set; }    //可用額度
        public float REAL_AMT { get; set; }   //剩餘可用額度

        public int BILL_TYPE { get; set; }    //結帳方式(0010十天結/0015半月結/0030月結/0045四十五天結)
        public int PAY_DAY { get; set; }      //帳單付款日(0010每月十號/0015每月十五號/0025每月二十五號)

    }
}
