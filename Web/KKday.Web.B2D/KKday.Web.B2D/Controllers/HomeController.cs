using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.SearchProd.Models;
//using KKday.API.WMS.Models.DataModel.Product;
using KKday.SearchProd.Models.Model;
using KKday.SearchProd.Models.Repostory;
using KKday.Web.B2D.EC.Models;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Model.Booking;
using KKday.Web.B2D.EC.Models.Repostory.Booking;
using KKday.Web.B2D.EC.Models.Model.Product;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using KKday.Web.B2D.EC.Models.Repostory.Account;
using KKday.Web.B2D.EC.Models.Model.Account;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using KKday.Web.B2D.EC.Models.Repostory.Common;
using KKday.Web.B2D.BE.App_Code;

namespace KKday.SearchProd.Controllers
{
    [Authorize(Policy="UserOnly")]
    public class HomeController : Controller
    {
        ICompositeViewEngine ViewEngine;
        private static IRedisHelper RedisHelper;

        public HomeController(ICompositeViewEngine viewEngine, IRedisHelper _redisHelper)
        {
            ViewEngine = viewEngine;
            RedisHelper = _redisHelper;
        }

        public IActionResult Index()
        {
            //假分銷商
            //distributorInfo fakeContact = DataSettingRepostory.fakeContact();

            //B2d分銷商資料
            var aesUserData = User.FindFirst(ClaimTypes.UserData).Value;
            var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));
            //取得可售商品之國家&城市
            string locale = UserData.LOCALE;
            var countries = CountryRepostory.GetCountries(locale);
            //取挖字
            Dictionary<string, string> uikey = CommonRepostory.getuiKey(RedisHelper, UserData.LOCALE); //fakeContact.lang, UserData.LOCALE
            ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(JsonConvert.SerializeObject(uikey));


            ViewData["prodTitle"] = title;

            return View(countries);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
