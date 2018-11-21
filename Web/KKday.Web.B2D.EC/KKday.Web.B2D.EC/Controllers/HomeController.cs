using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.SearchProd.Models;

using KKday.API.WMS.Models.DataModel.Product;
using KKday.SearchProd.Models.Model;
using KKday.SearchProd.Models.Repostory;
using KKday.Web.B2D.EC.Models;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Model.Booking;
using KKday.Web.B2D.EC.Models.Repostory.Booking;
using KKday.Web.B2D.EC.Models.Model.Product;
using Newtonsoft.Json;

namespace KKday.SearchProd.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var countries = CountryRepostory.GetCountries();

            //假分銷商
            distributorInfo fakeContact = DataSettingRepostory.fakeContact();

            Dictionary<string, string> uikey = RedisHelper.getuiKey(fakeContact.lang);
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
