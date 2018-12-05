using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Model.Account;
using KKday.Web.B2D.EC.Models.Model.Login;
using KKday.Web.B2D.EC.Models.Repostory.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using KKday.Web.B2D.BE.App_Code;

namespace KKday.Web.B2D.EC.Controllers
{

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
                //var accountRepo = (AccountRepository)HttpContext.RequestServices.GetService(typeof(AccountRepository));
                var encPassword = WebUtility.UrlEncode(Sha256Helper.Gethash(loginModel.Password));
                var account = AccountRepository.GetAccount(loginModel.Email, encPassword);
                //分流-KKdayUser&UserAdmin
                var IsKKdayUser = account is KKdayAccount ? true : false;
                var IsUserAdmin = (account is B2dAccount && ((B2dAccount)account).USER_TYPE.Equals("01")) ? true : false;

                var strChiperAcct = AesCryptHelper.aesEncryptBase64(JsonConvert.SerializeObject(account), Website.Instance.AesCryptKey);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.NAME),
                    new Claim("Account", account.EMAIL),
                    new Claim("UUID", account.UUID),
                    new Claim("UserType", IsKKdayUser ? "KKDAY" : (IsUserAdmin ? "ADMIN":"USER") ),
                    new Claim("Locale", account.LOCALE),
                    new Claim("Currency", IsKKdayUser ? "" : ((B2dAccount)account).CURRENCY),
                    new Claim(ClaimTypes.UserData,strChiperAcct), // 以AES加密JSON格式把使用者資料保存於Cookie
                };

                //var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                //var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

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
                    HttpContext.Session.SetString("B2D_COMPANY_LOCALE", ((B2dAccount)account).LOCALE);
                    HttpContext.Session.SetString("B2D_COMPANY_CURRENCY", ((B2dAccount)account).CURRENCY);
                }

                jsonData.Add("status", "OK");
                //Just redirect to our index after logging in. 
                jsonData.Add("url", IsKKdayUser ? Url.Content("~/KKday/") : Url.Content("~/"));
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
