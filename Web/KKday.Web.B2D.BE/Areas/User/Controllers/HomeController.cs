using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.Web.B2D.BE.Areas.User.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using KKday.Web.B2D.BE.Models.Model.Account;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.AppCode.DAL.Company;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.Company;

namespace KKday.Web.B2D.BE.Areas.User.Controllers
{
    [Area("User")]
    [TypeFilter(typeof(CultureFilter))]
    [Authorize(Policy = "UserOnly")]
    public class HomeController : Controller
    { 
        public IActionResult Index()
        { 
            var userId = User.Identity.Name;
           
            return View();
        }

        public IActionResult Review()
        {
            var aesUserData = User.FindFirst(ClaimTypes.UserData).Value;
            var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));
            var xid = UserData.COMPANY_XID;
            B2dCompany model = CompanyDAL.GetCompany(xid);
            return View(model);
        }

        #region 用不到

        /*
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
        */

        #endregion 用不到

    }
}
