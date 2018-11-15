using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Product;
using KKday.API.WMS.Models.DataModel.Search;
using KKday.API.WMS.Models.Repository.Discount;
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

                DataModel.Discount.DiscountRuleModel disc = null;

                #region --1.取回傳資料是否成功的訊息、統計用資訊--

                md.result = obj["metadata"]["status"].ToString();
                md.result_msg = obj["metadata"]["desc"].ToString();

                prod.metadata = md;
                #endregion

                //如果狀態為0000  表示搜尋沒問題  才繼續滿足商品欄位
                if (md.result == "0000") {

                   #region --2.從傑森物件取『商品列表』--
                    JArray jsonPlst = (JArray)obj["data"]["prods"];

                    int countBlackProd = 0;//計算黑名單筆數

                    for (int i = 0; i < jsonPlst.Count; i++)
                    {

                        var model = new ProductBaseModel();

                        string prod_no = jsonPlst[i]["id"].ToString();

                        //抓商品是否為黑名單
                        bool isBlack = DiscountRepository.GetProdBlackWhite(prod_no);

                        //表示該商品為白名單 需要綁入列表中 （黑名單的就不綁了）
                        if (isBlack != true)
                        {

                            model.prod_no = Convert.ToInt32(prod_no);
                            model.prod_name = jsonPlst[i]["name"].ToString();
                            model.b2d_price = DiscountRepository.GetCompanyDiscPrice(Int64.Parse(rq.company_xid), (double)jsonPlst[i]["price"], prod_no, jsonPlst[i]["main_cat_key"].ToString(), ref disc);//分銷價
                            model.b2c_price = (double)jsonPlst[i]["sale_price"];//直客價
                            model.display_ref_price = jsonPlst[i]["display_price"].ToString();
                          
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

                        } else {

                            countBlackProd ++;
                        }

                        //頁數、筆數統計
                        md.total_count = (int)obj["metadata"]["pagination"]["total_count"] - countBlackProd;//商品總筆數 扣掉黑名單筆數
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

                        //facets 大分類 小分類 ...的統計
                        if (rq.facets != null) {

                            Facets f = new Facets();

                            //大分
                            if (rq.facets.Where(x => x.Equals("cat_main")).Count() == 1) {

                                List<CatMain> cm =
                                         ((JArray)obj["data"]["facets"]["cat_main"])
                                             .Select(x => new CatMain {

                                                 id = (string)x["id"],
                                                 name = (string)x["name"],
                                                 sort = (string)x["sort"],
                                                 count = (int)x["count"]

                                             }).ToList();

                                f.cat_main = cm;

                            }

                            //小分
                            if (rq.facets.Where(x => x.Equals("cat")).Count() == 1) {
                                List<Cat> ca =
                                    ((JArray)obj["data"]["facets"]["cat"])
                                        .Select(x => new Cat {

                                            id = (string)x["id"],
                                            name = (string)x["name"],
                                            sort = (string)x["sort"],
                                            count = (int)x["count"]

                                        }).ToList();

                                f.cat = ca;
                            }


                            if (rq.facets.Where(x => x.Equals("total_time")).Count() == 1) {
                                List<TotalTime> tt =
                                     ((JArray)obj["data"]["facets"]["total_time"])
                                         .Select(x => new TotalTime {

                                             time = (int)x["time"],
                                             count = (int)x["count"]

                                         }).ToList();

                                f.total_time = tt;

                            }

                            if (rq.facets.Where(x => x.Equals("guide_lang")).Count() == 1) {
                                List<GuideLang> gl =
                                     ((JArray)obj["data"]["facets"]["guide_lang"])
                                         .Select(x => new GuideLang {

                                             id = (string)x["id"],
                                             name = (string)x["name"],
                                             count = (int)x["count"]

                                         }).ToList();

                                f.guide_lang = gl;
                            }

                            //可販售日期
                            if (rq.facets.Where(x => x.Equals("sale_dt")).Count() == 1) {
                                List<SaleDt> sd =
                                    ((JArray)obj["data"]["facets"]["sale_dt"])
                                        .Select(x => new SaleDt {

                                            id = (int)x["id"],
                                            count = (int)x["count"]

                                        }).ToList();

                                f.sale_dt = sd;
                            }

                            prod.facets = f;
                        }

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
