using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Areas.Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.BE.Areas.User.Views
{
    [Area("User")]
    [Authorize(Policy = "UserOnly")]
    public class AccountController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 基本資料
        /// </summary>
        /// <returns>The login.</returns> 
        public IActionResult MyProfile()
        {

            return View();
        }

        /// <summary>
        /// 使用者密碼
        /// </summary>
        /// <returns>The login.</returns> 
        public IActionResult Password()
        {

            return View();
        }



        /// <summary>
        /// 註冊頁面
        /// </summary>
        /// <returns>The register.</returns>
        [AllowAnonymous]
        public IActionResult Register()
        {

            return View();
        }
    }
}
