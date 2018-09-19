using System;
namespace KKday.API.WMS.Models.DataModel.Search {
    public class ProdListModel {

        public string id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string display_price { get; set; }
        public string sale_price { get; set; }
        public string display_sale_price { get; set; }
        public string is_display_price { get; set; }
        public string currency { get; set; }
        public string img_url { get; set; }
        public string rating_count { get; set; }
        public string rating_star { get; set; }
        public string instant_booking { get; set; }
        public string order_count { get; set; }
        public string days { get; set; }
        public string hours { get; set; }
        public string introduction { get; set; }
        public string duration { get; set; }
        public string display_price_usd { get; set; }
        public string price_usd { get; set; }
        public string main_cat_key { get; set; }

        public string countryId { get; set; }
        public string countryName { get; set; }

        public string[] cities { get; set; }//吐出範例"id:A01-003-00004,name:Sapporo"

        public string[] cat_key { get; set; }


    }

 
}
