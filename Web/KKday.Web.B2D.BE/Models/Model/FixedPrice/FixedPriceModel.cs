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
        public string STATE_NAME { get; set; }
    }

    ///////////

    public class FixedPricePackage
    { 
        public Int64 XID { get; set; }
        public Int64 PROD_XID { get; set; }
        public string PACKAGE_NO { get; set; }
        public string PACKAGE_NAME { get; set; } 
    }

    public class FixedPricePackageEx : FixedPricePackage
    { 
        public List<FixedPricePackageDtl> Prices;
    }

    ///////////

    public class FixedPricePackageDtl
    {
        public Int64 XID { get; set; }
        public Int64 PKG_XID { get; set; }
        public string PROD_COND { get; set; }
        public double PRICE { get; set; } 
    }
}
