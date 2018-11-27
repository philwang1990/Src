using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.BE.Areas.User.Controllers
{
    [Area("User")]
    [TypeFilter(typeof(CultureFilter))]
    public class LoginController : Controller
    {
        private IMemoryCache _cache;

        public LoginController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AuthenAsync(LoginModel loginModel)
        {
            Dictionary<string, string> jsonData = new Dictionary<string, string>();

            try
            {
                var accountRepo = (AccountRepository)HttpContext.RequestServices.GetService(typeof(AccountRepository));
                var account = accountRepo.GetAccount(loginModel.Email, loginModel.Password);
                var IsKKdayUser = account is KKdayAccount ? true : false;
                var IsUserAdmin = (account is B2dAccount && ((B2dAccount)account).USER_TYPE.Equals("01")) ? true : false;

                var strChiperAcct = AesCryptHelper.aesEncryptBase64(JsonConvert.SerializeObject(account), Website.Instance.AesCryptKey);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.NAME),
                    new Claim("Account", account.EMAIL),
                    new Claim("UUID", account.UUID),
                    new Claim("UserType", IsKKdayUser ? "KKDAY" : (IsUserAdmin ? "ADMIN" : "USER")),
                    new Claim("Locale", account.LOCALE),
                    new Claim("Currency", IsKKdayUser ? "" : ((B2dAccount)account).CURRENCY),
                    new Claim(ClaimTypes.UserData,strChiperAcct) // 以AES加密JSON格式把使用者資料保存於Cookie
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTime.UtcNow.AddDays(10), // 預設 Cookie 有效時間
                        IsPersistent = false,
                        AllowRefresh = false
                    });
                 
                if (!IsKKdayUser)
                {
                    HttpContext.Session.SetString("B2D_COMPANY_LOCALE",((B2dAccount)account).LOCALE);
                    HttpContext.Session.SetString("B2D_COMPANY_CURRENCY", ((B2dAccount)account).CURRENCY);
                    HttpContext.Session.SetString("B2D_ACCOUNT_EMAIL", ((B2dAccount)account).EMAIL);

                    if (((B2dAccount)account).USER_TYPE == "01") ((B2dAccount)account).USER_TYPE_DESC = "管理者";
                    else ((B2dAccount)account).USER_TYPE_DESC = "一般使用者";
                    HttpContext.Session.SetString("B2D_ACCOUNT_USERTYPE", ((B2dAccount)account).USER_TYPE_DESC);
                }

                jsonData.Add("status", "OK");
                //Just redirect to our index after logging in. 
                if(account.ENABLE==true)
                {
                    jsonData.Add("url", IsKKdayUser ? Url.Content("~/KKday/") : Url.Content("~/User"));
                }
                else
                {
                    jsonData.Add("url", IsKKdayUser ? Url.Content("~/KKday/") : Url.Content("~/User/Home/Review"));
                }

            }
            catch (Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "ERROR");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
        }

        /// <summary>
        /// 使用者登出
        /// </summary>
        /// <returns>The out async.</returns>
        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <returns>The password.</returns>
        public IActionResult ForgetPassword()
        {
            return View();
        }
    }
}
