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
