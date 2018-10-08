using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Areas.Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.BE.Areas.User.Controllers
{
    [Area("User")]
    public class LoginController : Controller
    {
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
                // 檢查登入者身分
                bool IsKKdayUser = false, IsB2dUser = false;
                // 檢查是否為合法KKday員工
                if(loginModel.Email.IndexOf("kkday.com", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    IsKKdayUser = true;
                }
                // 檢查是否為合法分銷商
                else {
                    IsB2dUser = true;
                }

                // 以上皆非, 則送出登入身分異常
                if(!IsKKdayUser && !IsB2dUser) {
                    throw new Exception("Invalid User Login");
                }

                var UserType = IsKKdayUser ? "KKDAY" : "USER";
               
                // 使用者姓名
                loginModel.Username = IsKKdayUser ? "KK員工" : "酷遊天旅行社";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginModel.Email),
                    new Claim(ClaimTypes.Name, loginModel.Username),
                    new Claim("UserType", UserType)
                };
                  
                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20), // 預設 Cookie 有效時間
                        IsPersistent = false,
                        AllowRefresh = false
                    });

                //Just redirect to our index after logging in. 
                var _url =  IsKKdayUser ? Url.Content("~/KKday/") : Url.Content("~/User") ;
                jsonData.Add("status", "OK");
                jsonData.Add("url", _url);
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
