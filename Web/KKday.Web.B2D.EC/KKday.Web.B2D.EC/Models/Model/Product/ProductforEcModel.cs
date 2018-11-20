using System;
using System.Collections.Generic;
using KKday.Web.B2D.EC.Models.Model.Pmch;

namespace KKday.Web.B2D.EC.Models.Model.Product
{
    public class ProductforEcModel : ProductModel
    {
        public string guidNo { get; set; } //guid
        public string durationStr { get; set; }

        public string locationCountry { get; set; }
        public string locationCity { get; set; }
        public string confirmInfo { get; set; } //imm_confir onfirm_hr_48 confirm_hr_24 confirm_hr_0 m
        public string policyInfo { get; set; } //common_free_cancellation common_no_refund cancellation-policy (如果是3可能要錨點）

        public List<CancelPolicy> cancelPolicyList { get; set; } //取消政策
    }

    //套餐日期
    public class PkgDateforEcModel
    {
        public string pkgOid { get; set; }
        public string day { get; set; }  //日期以逗號區隔
    }


    //ajax查套餐用
    public class prodQury
    {
        public string prodOid { get; set; }
        public string selDate { get; set; }
    }

    //ajax confirm 
    public class confirmPkgInfo
    {
        public string prodOid { get; set; }
        public string selDate { get; set; }
        public string pkgOid { get; set; }
        public int? price1Qty { get; set; }
        public int? price2Qty { get; set; }
        public int? price3Qty { get; set; }
        public int? price4Qty { get; set; }
        public string pkgEvent { get; set; }
        public string guid { get; set; }
    }

    public class prodEvent
    {
        public string companyXid { get; set; }
        public string lang { get; set; }
        public string prodno { get; set; }
        public string pkgno { get; set; }
        public string DateSelected { get; set; }
    }

    public class CusAgeRange
    {
        public int? price1Qty { get; set; }
        public int price1sAge { get; set; } //年齡起
        public int price1eAge { get; set; } //年齡迄

        public int? price2Qty { get; set; }
        public int price2sAge { get; set; } //年齡起
        public int price2eAge { get; set; } //年齡迄

        public int? price3Qty { get; set; }
        public int price3sAge { get; set; } //年齡起
        public int price3eAge { get; set; } //年齡迄

        public int? price4Qty { get; set; }
        public int price4sAge { get; set; } //年齡起
        public int price4eAge { get; set; } //年齡迄

    }

    public class returnStatus
    {
        public string status { get; set; }
        public string jsonStr { get; set; }
        public string msgErr { get; set; }
        public string url { get; set; }
        //public PmchSslRequest pmchSslRequest { get; set; }
        public PmchSslRequest3 pmchSslRequest { get; set; }

    }



}
