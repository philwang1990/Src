using System;

using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Discount
{


    public class DiscountModel
    {
        public string black_prod_no { get; set; }//黑名單的產品陣列
        public string is_black { get; set; }//

        public List<Rule> rules { get; set; }
    }

    public class Rule
    {
        public string mst_xid { get; set; }
        public double disc_percent { get; set; }
        public double amt { get; set; }
        public double disc_price { get; set; }

    }
}
