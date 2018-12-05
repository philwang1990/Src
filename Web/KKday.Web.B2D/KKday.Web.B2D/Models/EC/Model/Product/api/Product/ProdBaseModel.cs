using System;
using System.Collections.Generic;
namespace KKday.Web.B2D.EC.Models.Model.Product
{
     

    public class ProductBaseModel
    {

        public int prod_no { get; set; }  //id
        public string prod_name { get; set; }

        public double b2c_price { get; set; }//直客價
        public double b2d_price { get; set; }//同業價（套價）....特價的價錢 (計算用) ＝ ref_price (跟LULU共用）牌價
        public string display_ref_price { get; set; } //特價的價錢 (顯示用)
        public double sale_price { get; set; } //市價  商品原售價(計算用) (跟LULU共用) 
        public string display_sale_price { get; set; }  //沒特價的價錢(顯示用) 

        public string is_display_price { get; set; }
        public string prod_currency { get; set; }
        public string prod_img_url { get; set; }
        public int rating_count { get; set; }//評價數
        public double avg_rating_star { get; set; }//評價星等 (跟LULU共用) 
        public Boolean instant_booking { get; set; }//即時預訂 
        public int order_count { get; set; } //訂單數量
        public int days { get; set; }//所需天數  (跟LULU共用)
        public int hours { get; set; } //所需時數  (跟LULU共用)
        public string introduction { get; set; }//(跟LULU共用)
        public int duration { get; set; }//所需時間 (分鐘)
        public string display_price_usd { get; set; }//美金價錢(顯示用) 多幣別後不使用
        public double price_usd { get; set; }//美金價錢 多幣別後不使用
        public string prod_type { get; set; }//主分類key(跟LULU共用)
        public string[] tag { get; set; }//小分類 key

        public List<Country> countries { get; set; }

        //無用欄位？  promo_tag_keys

        //actual_price 為牌價，sale_price 為直客價

    }

    public class Country
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<City> cities { get; set; }
    }

    public class City
    {
        public string id { get; set; }
        public string name { get; set; }
    }


}