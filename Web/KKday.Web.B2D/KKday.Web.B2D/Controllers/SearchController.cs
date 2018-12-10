using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.SearchProd.Models.Repostory;
using KKday.API.WMS.Models.DataModel.Search;
//using KKday.API.WMS.Models.DataModel.Product;
using KKday.SearchProd.Models.Model;
using KKday.Web.B2D.EC.Models.Model.Booking;
using KKday.Web.B2D.EC.Models.Repostory.Booking;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Model.Product;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using KKday.Web.B2D.EC.Models.Model.Account;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using KKday.Web.B2D.EC.Models.Repostory.Common;
using KKday.Web.B2D.BE.App_Code;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.SearchProd.Controllers
{
    [Authorize(Policy = "UserOnly")]
    public class SearchController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private static IRedisHelper RedisHelper;

        public SearchController(IHttpContextAccessor _httpContextAccessor, IRedisHelper _redisHelper)
        {
            httpContextAccessor = _httpContextAccessor;
            RedisHelper = _redisHelper;
        }

        string[] _durations = null;
        // GET: /<controller>/
        public IActionResult ProdList(string pg, string cat_main, string cat_sub, string key1, string citykey, string pricerange,
               string datefilter, string budget, string[] duration, string[] guidelang)
        {
            //紀錄目前頁次
            int active_page_idx = Convert.ToInt32(pg ?? "1");
            //紀緣分割後的durations

            if (duration != null)
            {
                _durations = duration.Select(s => s.Replace("-", ",")).ToArray();
            }

            //B2d分銷商資料
            var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
            var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));
            //分銷商語系
            string locale = UserData.LOCALE;
            //分銷商幣別
            string currency = UserData.CURRENCY;
            //取得可售商品之國家&城市
            var countries = CountryRepostory.GetCountries(locale);

            Dictionary<string, string> uikey = CommonRepostory.getuiKey(RedisHelper, UserData.LOCALE);
            ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(JsonConvert.SerializeObject(uikey));

            List<ProductBaseModel> prodList = null;

            //查詢條件不為空值才查
            if (!string.IsNullOrEmpty(key1) || !string.IsNullOrEmpty(citykey))
            {
                int offset = (active_page_idx - 1) * 20;  //計算從第幾筆開始
                int size = 20;                            //分頁筆數
                int total_count = 0;                      //out 參數 (接收返回的total_count參數)
                int total_pages = 0;                      //out 參數 (接收返回的total_pages參數)
                Facets facets = new Facets();             //out 參數 (接收返回的facets參數)
                Stats stats = new Stats();                //out 參數 (接收返回的stats參數)

                //取得資料
                prodList = SearchRepostory.GetProduct(locale, currency, key1, citykey, offset, size, datefilter, budget, _durations, guidelang, cat_main, cat_sub,
                                                                    out total_count, out total_pages, out stats, out facets);

                List<CountryInfo> country = new List<CountryInfo>();
                country = CountryRepostory.GetCountries(key1, citykey, locale);

                //當城市或國家不為空，則清除查詢關鍵字
                if (country != null && country.Count() > 0)
                {
                    //已選城市的判斷
                    if (!string.IsNullOrEmpty(citykey))
                    {
                        var cd3 = country.SelectMany(co => co.Cities.Where(ci => ci.IsSelceted == true).ToList()).ToList().First();
                        string citycd = cd3.CityCode;
                        ViewData["citycode"] = citycd;
                    }
                    else
                    {
                        var city = country.FirstOrDefault().Cities.FirstOrDefault();
                        city.IsSelceted = true;
                        ViewData["citycode"] = city.CityCode;
                    }
                    key1 = null;
                }

                //傳入VIEW的參數 
                ViewData["total_count"] = total_count;
                ViewData["active_page_idx"] = active_page_idx;
                ViewData["total_pages"] = total_pages;
                ViewData["key"] = key1;
                ViewData["facets"] = facets;
                ViewData["duration"] = duration;
                ViewData["guidelang"] = guidelang;
                ViewData["stats"] = stats;
                ViewData["pricerange"] = !string.IsNullOrEmpty(pricerange) ? pricerange : string.Format("{0};{1}", stats.price.min, stats.price.max);
                ViewData["budget"] = !string.IsNullOrEmpty(budget) ? budget : string.Format("{0};{1}", stats.price.min, stats.price.max);
                ViewData["country"] = country;
                ViewData["prodTitle"] = title;
            }

            return View(prodList);
        }
    }
}
