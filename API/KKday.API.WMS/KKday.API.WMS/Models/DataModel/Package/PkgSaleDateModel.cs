using System;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Package {

    public class PkgPriceModel {

        public string currency { get; set; }
        public List<pkgs> pkgs { get; set; }
    
    }

    public class pkgs
    {
        public string pkg_no { get; set; }
        public double? price1 { get; set; }
        public double? price1_b2c { get; set; }
        public double? price2 { get; set; }
        public double? price2_b2c { get; set; }
        public double? price3 { get; set; }
        public double? price3_b2c { get; set; }
        public double? price4 { get; set; }
        public double? price4_b2c { get; set; }
    }
}
