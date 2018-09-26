using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.BE.Areas.User.Views
{
    [Area("User")]
    public class AccountController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登入頁面
        /// </summary>
        /// <returns>The login.</returns>
        public IActionResult Login()
        {

            return View();
        }

        /// <summary>
        /// 註冊頁面
        /// </summary>
        /// <returns>The register.</returns>
        public IActionResult Register()
        {

            return View();
        }
    }
}
