using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
//using KKday.API.WMS.Models.DataModel.Product;
using KKday.API.WMS.Models.DataModel.Search;
using KKday.SearchProd.AppCode;
using KKday.Web.B2D.EC.AppCode;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 
using KKday.Web.B2D.EC.Models.Model.Product;

namespace KKday.SearchProd.Models.Repostory
{
    public class SearchRepostory
    {
        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <returns>The product.</returns>
        /// <param name="lang">Lang.</param>
        /// <param name="currency">Currency.</param>
        /// <param name="filter">Filter.</param>
        /// <param name="skip">Skip.</param>
        /// <param name="size">Size.</param>
        /// <param name="total_count">Total count.</param>
        /// <param name="total_pages">Total pages.</param>
        public static List<ProductBaseModel> GetProduct(string lang, string currency, string filter, string citykey,
            int skip, int size, string datefilter, string budgetfilter, string[] durations, string[] guidelang, string cat_main, string cat_sub,
                out int total_count, out int total_pages, out Stats stats, out Facets facets) 
        {
            //連接WMS-API
            try
            {
                List<ProductBaseModel> pLst = new List<ProductBaseModel>();

                string jsonResult;

                //建立連線到WMS-API
                var _uri = "https://192.168.2.83:6001/api/Search";

                using (var handler = new HttpClientHandler())
                {
                    // Ignore Certificate Error!!
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                    using (var client = new HttpClient(handler))
                    {
                        #region Uri with QueryStrings

                        /*
                          {
                             "lang":"en",
                             "currency":"TWD",
                             "cat_keys":["TAG_1_3"],
                             "cat_main_keys":["TAG_1"],
                             "city_keys":["A01-001-00006"],
                             "count":"10",
                             "country_keys":["A01-001"],
                             "date_from":"20180101",
                             "date_to":"20181018",
                             "locale":"繁體中文",    //語系(WMS)
                             "price_from":"0",
                             "price_to":"1000",
                             "prod_ids":["20140","20159"],
                             "q":"水金九",
                             "sort":"PDESC",
                             "start":"0",
                             "stats":["price"]
                          }
                        */

                        var query = new Dictionary<string, object>
                        {
                            ["lang"] = lang,         //語系
                            ["currency"] = currency, //幣別
                            ["start"] = skip,        //計算從第幾筆開始
                            ["count"] = size,        //分頁筆數
                            ["q"] = filter,          //查詢條件
                            //["city_key"] = citykey,
                            ["stats"] = new string[] { "price" },
                            ["facets"] = new string[] { "cat_main", "cat", "guide_lang" },  //, "total_time", "sale_dt"
                            ["company_xid"] = "1"
                        };

                        //城市
                        if (citykey != null && citykey.Length > 0)
                        {
                            query.Add("city_keys", new string[] { citykey });
                        }

                        #region 加入Filter

                        //日期起始
                        if (!string.IsNullOrEmpty(datefilter) && datefilter.IndexOf("-") != -1)
                        {
                            //將StarDate&EndDate分割，加入WMS-API(query)的查詢條件中
                            var daterange = datefilter.Split(new char[2] { '-', '/' });
                            query.Add("date_from", daterange[0] + daterange[1] + daterange[2]);
                            query.Add("date_to", daterange[3] + daterange[4] + daterange[5]);
                        }
                        //價格起始
                        if (!string.IsNullOrEmpty(budgetfilter) && budgetfilter.IndexOf(";") != -1)
                        {
                            var budget = budgetfilter.Split(new char[1] { ';' });
                            query.Add("price_from", budget[0]);
                            query.Add("price_to", budget[1]);
                        }
                        //行程時間
                        if (durations != null && durations.Length > 0)
                        {
                            query.Add("durations", durations);
                        }
                        //導覽語言
                        query.Add("guide_langs", guidelang);

                        //次分類
                        if (cat_sub != null && cat_sub.Length > 0)
                        {
                            query.Add("cat_keys", new string[] { cat_sub });
                        }
                        //主分類
                        if (cat_main != null && cat_main.Length > 0)
                        {
                            query.Add("cat_main_keys", new string[] { cat_main });
                        }

                        #endregion

                        //轉換JSON格式
                        var content = JsonConvert.SerializeObject(query);

                        #endregion Uri with QueryStrings

                        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _uri))
                        {
                            //request.Headers.Add("x-auth-key", "kkdaysearchapi_Rfd_fsg+x+TcJy");
                            request.Headers.Add("Accept", "application/json");
                            // Add body content
                            request.Content = new StringContent(
                                content,
                                Encoding.UTF8,
                                "application/json"
                            );

                            var response = client.SendAsync(request).Result;
                            jsonResult = response.Content.ReadAsStringAsync().Result;
                        }
                    }

                    //分析資料並轉換回ProductBaseModel串列
                    JObject jProdobj = JObject.Parse(jsonResult);

                    //查詢產品      (將OBJECT物件轉為LIST型態)
                    pLst = jProdobj["prods"].ToObject<List<ProductBaseModel>>();

                    //準備商品之國家&城市資料
                    List<Country> countries = new List<Country>();

                    foreach (var naToken in pLst)
                    {
                        var naVal = naToken.countries;

                        Country countryinfo = new Country()
                        {
                            id = naVal.ToString(),
                            name = naVal.ToString(),
                            cities = new List<City>()
                        };

                        foreach (var ctVal in naVal)
                        {
                            countryinfo.cities.Add(new City()
                            {
                                id = ctVal.ToString(),
                                name = ctVal.ToString()
                            });
                        }

                        countries.Add(countryinfo);
                    }

                    facets = new Facets()
                    {
                        cat_main = jProdobj["facets"]["cat_main"].ToObject<List<CatMain>>(),         //主分類
                        cat_sub = jProdobj["facets"]["cat"].ToObject<List<CatSub>>(),                //次分類
                        guide_lang = jProdobj["facets"]["guide_lang"].ToObject<List<GuideLang>>(),   //導覽語系
                        //total_time = jProdobj["facets"]["total_time"].ToObject<List<TotalTime>>()    //行程時間
                    };

                    stats = new Stats()
                    {
                        //價格條件
                        price = jProdobj["stats"]["price"].ToObject<Price>()
                    };

                    //查詢返迴總筆數 (將OBJECT物件轉為INT型態)
                    total_count = jProdobj["metadata"]["total_count"].ToObject<int>();

                    //計算總頁數
                    total_pages = (total_count / size) + ((total_count % size == 0) ? 0 : 1);

                }

                return pLst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
