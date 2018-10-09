using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Product;
using KKday.API.WMS.Models.DataModel.Search;
using KKday.API.WMS.Models.Search;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.Models.Repository {
    public class SearchRepository {
        /// <summary>
        /// Gets the prod list.
        /// </summary>
        /// <returns>The prod list.</returns>
        /// <param name="rq">Rq.</param>
        //1.取得商品列表
        public static SearchProductModel GetProdList(SearchRQModel rq) 
        {

            SearchProductModel prod = new SearchProductModel();
            List<ProductBaseModel> pLst = new List<ProductBaseModel>();

            try {

                JObject obj = SearchProxy.GetProdList(rq);

                Metadata md = new Metadata();

                #region --1.取回傳資料是否成功的訊息、統計用資訊--

                md.result = obj["metadata"]["status"].ToString();
                md.result_msg = obj["metadata"]["desc"].ToString();

                prod.metadata = md;
                #endregion

                //如果狀態為0000  表示搜尋沒問題  才繼續滿足商品欄位
                if (md.result == "0000") {

                    //頁數、筆數統計
                    md.total_count = (int)obj["metadata"]["pagination"]["total_count"];//商品總筆數
                    md.start = (int)obj["metadata"]["pagination"]["start"];//從第？筆開始
                    md.count = (int)obj["metadata"]["pagination"]["count"];//一頁？筆商品

                    prod.metadata = md;

                    //stats 金額統計
                    if (rq.stats != null) {
                        Stats s = new Stats();
                        s.price = new Price() {

                            min = (int)obj["data"]["stats"]["price"]["min"],
                            max = (int)obj["data"]["stats"]["price"]["max"],
                            count = (int)obj["data"]["stats"]["price"]["count"],
                            currency = obj["data"]["stats"]["price"]["currency"].ToString()

                        };
                        prod.stats = s;

                      }

                    //facets
                    if (rq.facets != null) {
                        Facets f = new Facets();
                        List<CatMain> cm =
                                 ((JArray)obj["data"]["facets"]["cat_main"])
                                     .Select(x => new CatMain {

                                         id = (string)x["id"],
                                         name = (string)x["name"],
                                         sort = (string)x["sort"],
                                         count = (int)x["count"]

                                     }).ToList();

                        f.cat_main = cm;

                        List<TotalTime> tt =
                                 ((JArray)obj["data"]["facets"]["total_time"])
                                     .Select(x => new TotalTime {

                                         time = (int)x["time"],
                                         count = (int)x["count"]

                                     }).ToList();

                        f.total_time = tt;

                        List<GuideLang> gl =
                                 ((JArray)obj["data"]["facets"]["guide_lang"])
                                     .Select(x => new GuideLang {

                                         id = (string)x["id"],
                                         name = (string)x["name"],
                                         count = (int)x["count"]

                                     }).ToList();

                        f.guide_lang = gl;

                        List<Cat> ca =
                                ((JArray)obj["data"]["facets"]["cat"])
                                    .Select(x => new Cat {

                                        id = (string)x["id"],
                                        name = (string)x["name"],
                                        sort = (string)x["sort"],
                                        count = (int)x["count"]

                                    }).ToList();

                        f.cat = ca;

                        List<SaleDt> sd =
                                ((JArray)obj["data"]["facets"]["sale_dt"])
                                    .Select(x => new SaleDt {

                                        id = (int)x["id"],
                                        count = (int)x["count"]

                                    }).ToList();

                        f.sale_dt = sd;

                        prod.facets = f;
                    }

                    #region --2.從傑森物件取『商品列表』--
                    JArray jsonPlst = (JArray)obj["data"]["prods"];

                    for (int i = 0; i < jsonPlst.Count; i++) {

                        var model = new ProductBaseModel();

                        model.prod_no = (int)jsonPlst[i]["id"];
                        model.prod_name = jsonPlst[i]["name"].ToString();
                        model.b2d_price = (double)jsonPlst[i]["price"];
                        model.display_ref_price = jsonPlst[i]["display_price"].ToString();
                        model.sale_price = (double)jsonPlst[i]["sale_price"];
                        model.display_sale_price = jsonPlst[i]["display_sale_price"].ToString();
                        model.is_display_price = jsonPlst[i]["is_display_price"].ToString();
                        model.prod_currency = jsonPlst[i]["currency"].ToString();
                        model.prod_img_url = jsonPlst[i]["img_url"].ToString();
                        model.rating_count = (int)jsonPlst[i]["rating_count"];
                        model.avg_rating_star = (double)jsonPlst[i]["rating_star"];
                        model.instant_booking = (bool)jsonPlst[i]["instant_booking"];
                        model.order_count = (int)jsonPlst[i]["order_count"];
                        model.days = (int)jsonPlst[i]["days"];
                        model.hours = (int)jsonPlst[i]["hours"];
                        model.introduction = jsonPlst[i]["introduction"].ToString();
                        model.duration = (int)jsonPlst[i]["duration"];
                        //model.display_price_usd = jsonPlst[i]["display_price_usd"].ToString();
                        //model.price_usd = (double)jsonPlst[i]["price_usd"];
                        model.prod_type = jsonPlst[i]["main_cat_key"].ToString();
                        model.tag = jsonPlst[i]["cat_key"].ToObject<string[]>();//把傑森物件轉成字串陣列

                        //取國家,城市
                        List<Country> country =
                            ((JArray)jsonPlst[i]["countries"])
                                .Select(x => new Country {

                                    id = (string)x["id"],
                                    name = (string)x["name"],
                                    cities = x["cities"].ToObject<List<City>>()

                                }).ToList();

                        model.countries = country;

                        pLst.Add(model);
                    }

                    prod.prods = pLst;
                    #endregion

                }

                } catch (Exception ex) {

                Website.Instance.logger.FatalFormat($"getProdLst  Error :{ex.Message},{ex.StackTrace}");

                throw ex;
            
            }

            return prod;
        }

    }
}
