using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Common;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        /// 檢視基本資料
        /// </summary>
        /// <returns>The login.</returns> 
        public IActionResult MyProfile()
        {
            var account = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();

            var accountRepo = (AccountRepository)HttpContext.RequestServices.GetService(typeof(AccountRepository));
            B2dUserProfile _profile = accountRepo.GetProfile(account);

            return View(_profile);
        }

        /// <summary>
        /// 更改密碼
        /// </summary>
        /// <returns>The login.</returns> 
        public IActionResult Password()
        {
            var _strUuid = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "UUID").Select(c => c.Value)).FirstOrDefault();
            ViewData["UUID"] = _strUuid;

            return View();
        }

        /// <summary>
        /// 更改使用者密碼
        /// </summary>
        /// <returns>Json Result</returns>

        [HttpPost]
        public IActionResult UpdatePassword(string uuid, string password)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);
            Dictionary<string, string> jsonData = new Dictionary<string, string>();

            try
            {
                var _strAccount = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();
                if (string.IsNullOrEmpty(_strAccount))
                {
                    throw new Exception("Invalid account to updated password");
                }

                var accountRepo = (B2dAccountRepository)HttpContext.RequestServices.GetService(typeof(B2dAccountRepository));
                accountRepo.SetNewPassword(_strAccount, password);

                jsonData.Add("status", "OK");
            }
            catch(Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
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

        /// <summary>
        /// 註冊分銷商
        /// </summary>
        /// <returns>Json Result</returns> 
        [HttpPost]
        [AllowAnonymous]
        public IActionResult InsertCompany([FromBody] RegisterModel reg)
        {
            try
            {
                var accountRepo = (AccountRepository)HttpContext.RequestServices.GetService(typeof(AccountRepository));
                accountRepo.Register(reg);
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
    }
}
