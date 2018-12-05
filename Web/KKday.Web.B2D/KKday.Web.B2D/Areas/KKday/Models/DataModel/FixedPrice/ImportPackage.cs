using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.BE.Areas.KKday.Models.DataModel.FixedPrice
{
    public class ImportPackage
    {
        public Int64 PROD_XID { get; set; }
        public DateTime S_DATE { get; set; }
        public DateTime E_DATE { get; set; }

        public List<PackageItem> packages { get; set; }
    }
 
    public class PackageItem
    {
        public string PKG_NO { get; set; }
        public string PKG_NAME { get; set; }
        public List<PackagePriceItem> prices { get; set; }
    }

    public class PackagePriceItem
    {
        public string COND { get; set; } // Price Condition
        public double PRICE { get; set; }
    }
}
