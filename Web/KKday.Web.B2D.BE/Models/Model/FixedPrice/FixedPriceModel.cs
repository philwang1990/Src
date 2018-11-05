using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.BE.Models.Model.FixedPrice
{
    public class FixedProduct
    {
        public Int64 XID { get; set; }
        public Int64 COMPANY_XID { get; set; }
        public string COMPANY_NAME { get; set; }
        public string PROD_NO { get; set; }
        public string PROD_NAME { get; set; }
    }

    public class FixedProductPackage
    { 
        public Int64 XID { get; set; }
        public Int64 PROD_XID { get; set; }
        public string PACKAGE_NO { get; set; }
        public string PACKAGE_NAME { get; set; } 
    }

    public class FixedProductPackageDtl
    {
        public Int64 XID { get; set; }
        public Int64 PROD_XID { get; set; }
        public string PACKAGE_NO { get; set; }
        public string PACKAGE_NAME { get; set; }

        public List<FixedProductPackagePrice> Prices;
    }

    public class FixedProductPackagePrice
    {
        public Int64 XID { get; set; }
        public Int64 PKG_XID { get; set; }
        public string PROD_COND { get; set; }
        public double PRICE { get; set; } 
    }
}
