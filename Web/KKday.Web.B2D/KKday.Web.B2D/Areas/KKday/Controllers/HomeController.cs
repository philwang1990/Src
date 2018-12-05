using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.Web.B2D.BE.Areas.KKday.Models;
using Microsoft.AspNetCore.Authorization;
using KKday.Web.B2D.EC.Models.Model.Account;
using Newtonsoft.Json;
using System.Security.Claims;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Filters;

namespace KKday.Web.B2D.BE.Areas.KKday.Controllers
{
    [Area("KKday")]
    [Authorize(Policy = "KKdayOnly")]
    [TypeFilter(typeof(CultureFilter))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var jsonAccount = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
            if (jsonAccount != null)
            {
                var account = JsonConvert.DeserializeObject<KKdayAccount>(AesCryptHelper.aesDecryptBase64(jsonAccount, Website.Instance.AesCryptKey));
            }

            return View();
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
