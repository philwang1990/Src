using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.Web.B2D.EC.Models;
using KKday.Web.B2D.EC.Models.Model.Booking;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using KKday.Web.B2D.EC.Models.Repostory.Booking;
using Microsoft.Extensions.Caching.Memory;
using KKday.Web.B2D.EC.Models.Model.Pmch;

namespace KKday.Web.B2D.EC.Controllers
{
    public class HomeController : Controller
    {
        private static IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


        public IActionResult Index()
        {
            var dd = Request.HttpContext.Connection.RemoteIpAddress;

            //string dd = GibberishAES.OpenSSLEncrypt("4093240835103617", "card%no$kk#@");

            //string dd = GibberishAES.OpenSSLDecrypt("U2FsdGVkX18unCee5VKYXzSO56iLi3inWiLxOoZGLEY=","pmgw@%#@trans*no");

            //PmchSslReq pmch = new PmchSslReq();

            //pmch.isSuccess = true;
            //pmch.errorCode = "";
            //pmch.errorMsg = "";
            //pmch.pmgwTransNo = "PMGW000000000";
            //pmch.pmgwMethod = "AUTH";
            //pmch.transactionCode = "76767";
            //pmch.payCurrency = "TWD";
            //pmch.payAmount = 210;
            //pmch.is3D = false;
            //PmchSslMemberInfo mem = new PmchSslMemberInfo();
            //mem.encodeCardNo = "3337";
            //pmch.memberInfo = mem;
            //pmch.isFraud = "0";
            //pmch.riskNote = "";

            //string d = JsonConvert.SerializeObject(pmch);

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
