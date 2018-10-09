using System;
namespace KKday.API.WMS.Models.DataModel.Product
{
    public class QueryProductModel
    {
        public string b2d_xid { set; get; }
        public string locale_lang { get; set; }
        public string current_currency { get; set; }
        public string prod_no { get; set; }
        public string pkg_no { get; set; }//套餐用

    }
}



