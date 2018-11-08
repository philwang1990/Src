using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.BE.Models.Model.FixedPrice
{
    public class FixedPriceProduct
    {
        public Int64 XID { get; set; }
        public Int64 COMPANY_XID { get; set; }
        public string PROD_NO { get; set; }
        public string PROD_NAME { get; set; }
        public string STATE { get; set; }
    }

    public class FixedPriceProductEx : FixedPriceProduct
    {
        public string CURRENCY { get; set; }
        public string COMPANY_NAME { get; set; }
        public string STATE_NAME { get; set; }
    }

    ///////////

    public class FixedPricePackage
    {
        public Int64 XID { get; set; }
        public Int64 PROD_XID { get; set; }
        public string PACKAGE_NO { get; set; }
        public string PACKAGE_NAME { get; set; }
        public DateTime ONLINE_SDATE { get; set; }
        public DateTime ONLINE_EDATE { get; set; }
    }

    public class FixedPricePackageEx : FixedPricePackage
    {
        public int SEQ_NO { get; set; }
        public List<FixedPricePackageDtl> Prices;
    }

    ///////////

    public class FixedPricePackageDtl
    {
        public Int64 XID { get; set; }
        public Int64 PKG_XID { get; set; }
        public string PRICE_COND { get; set; }
        public double PRICE { get; set; }
    }

    public class FixedPricePackageDtlEx : FixedPricePackageDtl
    {
        public int PKG_SEQ_NO { get; set; }
    }
}