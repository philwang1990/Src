using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Search;
using KKday.API.WMS.Models.Search;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.Models.Repository {
    public class ProdRepository {

        //1.取得商品列表
        public static List<ProdListModel> GetProdList(SearchRQModel rq) 
        {
            List<ProdListModel> pLst = new List<ProdListModel>();

            try {

                JObject obj = new JObject();

                obj = SearchProxy.GetProdList(rq);

                //從傑森物件取『商品列表』
                JArray jsonPlst = (JArray)obj["data"]["prods"];
              
                for (int i = 0; i < jsonPlst.Count; i++) {

                    var model = new ProdListModel();

                    model.id = jsonPlst[i]["id"].ToString();
                    model.name = jsonPlst[i]["name"].ToString();
                    model.price = jsonPlst[i]["price"].ToString();
                    model.display_price = jsonPlst[i]["display_price"].ToString();
                    model.sale_price = jsonPlst[i]["sale_price"].ToString();
                    model.display_sale_price = jsonPlst[i]["display_sale_price"].ToString();
                    model.is_display_price = jsonPlst[i]["is_display_price"].ToString();
                    model.currency = jsonPlst[i]["currency"].ToString();
                    model.img_url = jsonPlst[i]["img_url"].ToString();
                    model.rating_count = jsonPlst[i]["rating_count"].ToString();
                    model.rating_star = jsonPlst[i]["rating_star"].ToString();
                    model.instant_booking = jsonPlst[i]["instant_booking"].ToString();
                    model.order_count = jsonPlst[i]["order_count"].ToString();
                    model.days = jsonPlst[i]["days"].ToString();
                    model.hours = jsonPlst[i]["hours"].ToString();
                    model.introduction = jsonPlst[i]["introduction"].ToString();
                    model.duration = jsonPlst[i]["duration"].ToString();
                    model.display_price_usd = jsonPlst[i]["display_price_usd"].ToString();
                    model.price_usd = jsonPlst[i]["price_usd"].ToString();
                    model.main_cat_key = jsonPlst[i]["main_cat_key"].ToString();

                    model.cat_key = jsonPlst[i]["cat_key"].ToObject<string[]>();//把傑森物件轉成字串陣列
                      
                    model.countryId = jsonPlst[i]["countries"][0]["id"].ToString();
                    model.countryName = jsonPlst[i]["countries"][0]["name"].ToString();

                    JArray arrCities = (JArray)jsonPlst[i]["countries"][0]["cities"];                            
                    List<String> list = new List<String>();
                    //把結森陣列放進list
                    for (int c = 0; c < arrCities.Count; c++) {
                    
                        list.Add("id:"+ arrCities[c]["id"].ToString()+
                                 ",name:"+ arrCities[c]["name"].ToString());
                        }

                    model.cities = list.ToArray();
                
                    pLst.Add(model);
                }
           
            } catch (Exception ex) {

                throw ex;
                //  Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }

            return pLst;
        }

    }
}
