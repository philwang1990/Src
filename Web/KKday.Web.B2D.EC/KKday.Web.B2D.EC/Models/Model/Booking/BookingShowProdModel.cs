using System;
namespace KKday.Web.B2D.EC.Models.Model.Booking
{
    public class BookingShowProdModel
    {
        public string prodOid { get; set; }
        public string prodName { get; set; }
        public string photoUrl { get; set; }

        public string currency { get; set; }
        public string sDate { get; set; }
        public int? price1Qty { get; set; }
        public double? price1 { get; set; }
        public int? price2Qty { get; set; }
        public double? price2 { get; set; }
        public int? price3Qty { get; set; }
        public double? price3 { get; set; }
        public int? price4Qty { get; set; }
        public double? price4 { get; set; }
        public string pkgOid { get; set; }
        public string pkgName { get; set; }
        public Boolean isRank { get; set; }
        public string eventOid { get; set; }
        public string eventTime { get; set; }
        public double? totoalPrice { get; set; }
        public string unitText { get; set; }
        public string confirm_order_time { get; set; }
    }

}
