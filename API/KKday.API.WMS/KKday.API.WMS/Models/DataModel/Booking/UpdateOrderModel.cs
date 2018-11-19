using System;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Booking
{


    public class UpdateOrderModel
    {

        public string order_no { get; set; }
        public string order_oid { get; set; }
        public string order_mid { get; set; }
        public int company_xid { get; set; }

    }
}
