using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Text;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.AppCode;
using KKday.Web.B2D.BE.Commons;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Resources;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.BE.Areas.User.Views
{
    [Area("User")]
    [TypeFilter(typeof(CultureFilter))]
    [Authorize(Policy = "UserOnly")]
    public class AccountController : Controller
    {
        // 列表顯示行數
        const int PAGE_SIZE = 5;

        // 挖字
        readonly ILocalizer _localizer;
        public AccountController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // 我的帳號
        public IActionResult Index()
        {
            var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
            var accountRepo = services.First(o => o.GetType() == typeof(B2dAccountRepository));

            var account = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();
            var countryRepos = HttpContext.RequestServices.GetService<CommonRepository>();
            var locale = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Locale").Select(c => c.Value)).FirstOrDefault();

            B2dUserProfile _profile = accountRepo.GetProfile(account);
            _profile.USER_TYPE_DESC = _profile.USER_TYPE == "01" ? "管理者" : "使用者";

            ViewData["CountryAreas"] = countryRepos.GetCountryAreas(locale);
            ViewData["CountryLocales"] = countryRepos.GetCountryLocales();

            //寄送mail
            //var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
            //var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

            //var md5 = System.Security.Cryptography.MD5.Create();
            //var token = Sha256Helper.Gethash(UserData.EMAIL + UserData.TEL + "#BID%&*KK@auth");
            //var url = "https://localhost:5001/Login/?verify=" + token;

            //寄送註冊成功通知
            //string from_email = "dora.tang@kkday.com";
            //string from_name = "dora";
            //Dictionary<string, string> user = new Dictionary<string, string>();
            //user.Add("doraemon", "dora.tang@kkday.com");
            //string subject = "註冊已完成";
            //string body = "請點選連結：" + url + "以完成帳號啟用";

            return View(_profile);
        }

        // 修改我的帳號資料（與子帳戶共用）
        [HttpPost]
        [AllowAnonymous]
        public IActionResult UpdateProfile([FromBody] B2dAccount acc)
        {
            try
            {
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var accountRepo = services.First(o => o.GetType() == typeof(B2dAccountRepository));
                var _strUuid = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "UUID").Select(c => c.Value)).FirstOrDefault();
                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));
                var upd_user = UserData.EMAIL;

                accountRepo.UpdateAccount(acc, upd_user);
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }

        // 更改密碼
        public IActionResult Password()
        {
            var _strUuid = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "UUID").Select(c => c.Value)).FirstOrDefault();
            ViewData["UUID"] = _strUuid;

            return View();
        }

        #region 子帳號區塊

        // 子帳戶列表
        public IActionResult WebUser(string query)
        {
            var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
            var acctRepos = services.First(o => o.GetType() == typeof(B2dAccountRepository));
            var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
            var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

            QueryParamsModel queryParams = null;

            if (!string.IsNullOrEmpty(query))
            {
                query = System.Web.HttpUtility.UrlDecode(query).Replace("&quot;", "\"");
                queryParams = JsonConvert.DeserializeObject<QueryParamsModel>(query);
            }
            else
            {
                queryParams = acctRepos.GetQueryParamModel(UserData.COMPANY_XID, string.Empty, string.Empty, PAGE_SIZE, 1);
            }

            var countryRepos = HttpContext.RequestServices.GetService<CommonRepository>();
            var locale = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Locale").Select(c => c.Value)).FirstOrDefault();
            ViewData["CountryAreas"] = countryRepos.GetCountryAreas(locale);
            ViewData["CountryLocales"] = countryRepos.GetCountryLocales();

            ViewData["QUERY_PARAMS"] = queryParams;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParams);

            var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
            var _accounts = acctRepos.GetAccounts(UserData.COMPANY_XID, queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

            return View(_accounts);
        }
        // 子帳戶單一帳號細節
        public IActionResult WebUserProfile(Int64 id)
        {
            try
            {
                var queryArgc = System.Web.HttpUtility.UrlEncode(this.Request.Query["query"].ToString());
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var acctRepos = services.First(o => o.GetType() == typeof(B2dAccountRepository));
                B2dAccount _account = acctRepos.GetAccount(id);

                ViewData["QueryParams"] = queryArgc;

                return View(_account);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        // 新增子帳號
        public IActionResult InsertWebUser([FromBody] B2dAccount acct)
        {
            try
            {
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var accountRepo = services.First(o => o.GetType() == typeof(B2dAccountRepository));
                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));
                var upd_user = UserData.EMAIL;
                acct.COMPANY_XID = UserData.COMPANY_XID;

                accountRepo.InsertAccount(acct, upd_user);
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
        // 修改子帳號資料與主帳號共用
        // 更改使用者密碼
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

                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var accountRepo = services.First(o => o.GetType() == typeof(B2dAccountRepository));
                accountRepo.SetNewPassword(uuid, password);

                jsonData.Add("status", "OK");
            }
            catch (Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
        }
        // 關閉帳號
        public IActionResult AccountProfile_close(Int64 xid)
        {
            try
            {
                var queryArgc = System.Web.HttpUtility.UrlEncode(this.Request.Query["query"].ToString());
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var acctRepos = services.First(o => o.GetType() == typeof(B2dAccountRepository));

                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                acctRepos.CloseAccount(xid, UserData.EMAIL);

                ViewData["QueryParams"] = queryArgc;

                return Json("OK");
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        // 刷新頁面
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Refresh([FromBody]QueryParamsModel queryParams)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var acctRepos = services.First(o => o.GetType() == typeof(B2dAccountRepository));
                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                //更新分頁資料
                if (queryParams.RecountFlag)
                {
                    queryParams = acctRepos.GetQueryParamModel(UserData.COMPANY_XID, queryParams.Filter, queryParams.Sorting, PAGE_SIZE, queryParams.Paging.current_page);
                }
                ViewData["QUERY_PARAMS"] = queryParams;

                var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
                var _accounts = acctRepos.GetAccounts(UserData.COMPANY_XID, queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

                jsonData["query_params"] = JsonConvert.SerializeObject(queryParams);
                jsonData["content"] = await this.RenderViewAsync<List<B2dAccount>>("WebUserList", _accounts, true);
                jsonData["status"] = "OK";
            }
            catch (Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
        }

        #endregion 子帳號區塊

        #region API帳號區塊

        // API子帳號列表
        public IActionResult ApiUser(string query)
        {
            var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
            var acctRepos = services.First(o => o.GetType() == typeof(B2dApiAccountRepository));
            var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
            var UserData = JsonConvert.DeserializeObject<B2dApiAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

            QueryParamsModel queryParams = null;

            if (!string.IsNullOrEmpty(query))
            {
                query = System.Web.HttpUtility.UrlDecode(query).Replace("&quot;", "\"");
                queryParams = JsonConvert.DeserializeObject<QueryParamsModel>(query);
            }
            else
            {
                queryParams = acctRepos.GetQueryParamModel(UserData.COMPANY_XID, string.Empty, string.Empty, PAGE_SIZE, 1);
            }

            ViewData["QUERY_PARAMS"] = queryParams;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParams);
            ViewData["CACHETIME"] = B2dApiAccountRepository.GetCache(UserData.COMPANY_XID);

            var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
            var _accounts = acctRepos.GetAccounts(UserData.COMPANY_XID, queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

            return View(_accounts);
        }
        // API單一帳號細節
        public IActionResult ApiUserProfile(Int64 id)
        {
            try
            {
                var queryArgc = System.Web.HttpUtility.UrlEncode(this.Request.Query["query"].ToString());
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var acctRepos = services.First(o => o.GetType() == typeof(B2dApiAccountRepository));
                B2dAccount _account = acctRepos.GetAccount(id);

                ViewData["QueryParams"] = queryArgc;

                return View(_account);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        // API修改帳號資料
        public IActionResult ApiUserUpdate([FromBody]B2dApiAccount acc)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var acctRepos = services.First(o => o.GetType() == typeof(B2dApiAccountRepository));
                var upd_user = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();

                //更新分銷商公司資料
                acctRepos.UpdateAccount(acc, upd_user);
                jsonData["status"] = "OK";
            }
            catch (Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
        }
        // 新增API帳號
        public IActionResult InsertApiUser([FromBody] B2dApiAccount acc)
        {
            try
            {
                //var accountRepo = (B2dApiAccountRepository)HttpContext.RequestServices.GetService(typeof(B2dApiAccountRepository));
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var accountRepo = (B2dApiAccountRepository)services.First(o => o.GetType() == typeof(B2dApiAccountRepository));

                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dApiAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));
                var crt_user = UserData.EMAIL;
                acc.COMPANY_XID = UserData.COMPANY_XID;
                acc.TEL_AREA = UserData.TEL_AREA;

                accountRepo.InsertAccount(acc, crt_user);
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }
        }
        // 更改API使用者密碼
        [HttpPost]
        public IActionResult UpdatePassword_Api(string mail, string password)
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

                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var accountRepo = services.First(o => o.GetType() == typeof(B2dApiAccountRepository));
                accountRepo.SetNewPassword(mail, password);

                jsonData.Add("status", "OK");
            }
            catch (Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
        }
        // 取得現有token
        public IActionResult ApiAccount_GetToken(Int64 xid)
        {
            Dictionary<string, string> jsonData = new Dictionary<string, string>();

            var token = B2dApiAccountRepository.GetToken(xid);

            if (token != "")
            {
                jsonData.Add("token", token);
                jsonData.Add("status", "OK");
            }

            else
            {
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", "請重新取得Token");
            }
            return Json(jsonData);
        }
        // 取得新token
        public IActionResult New_ApiAccount_Token(Int64 xid)
        {
            Dictionary<string, string> jsonData = new Dictionary<string, string>();

            var account = B2dApiAccountRepository.GetApiAccount(xid);
            var token = B2dApiAccountRepository.GetNewToken(account.EMAIL,account.PASSWORD);

            if (token.access_token != null)
            {
                jsonData.Add("token", token.access_token);
                jsonData.Add("status", "OK");
            }
            else 
            {
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg",token.error_description);
            }
            return Json(jsonData);
        }
        // 更改快取時間
        public IActionResult Update_CacheTime(Int64 time)
        {
            Dictionary<string, string> jsonData = new Dictionary<string, string>();
            var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
            var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

            B2dApiAccountRepository.UpdateCacheTime(time,UserData.COMPANY_XID);

            jsonData.Add("status", "OK");

            return Json(jsonData);
        }
        // 關閉API帳號
        public IActionResult ApiAccountProfile_close(Int64 xid)
        {
            try
            {
                var queryArgc = System.Web.HttpUtility.UrlEncode(this.Request.Query["query"].ToString());
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var acctRepos = services.First(o => o.GetType() == typeof(B2dApiAccountRepository));

                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                acctRepos.CloseAccount(xid, UserData.EMAIL);

                ViewData["QueryParams"] = queryArgc;

                return Json("OK");
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
        // 刷新API頁面
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> ApiRefresh([FromBody]QueryParamsModel queryParams)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var services = HttpContext.RequestServices.GetServices<IB2dAccountRepository>();
                var acctRepos = services.First(o => o.GetType() == typeof(B2dApiAccountRepository));
                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                //更新分頁資料
                queryParams = acctRepos.GetQueryParamModel(UserData.COMPANY_XID, queryParams.Filter, queryParams.Sorting, PAGE_SIZE, queryParams.Paging.current_page);
                ViewData["QUERY_PARAMS"] = queryParams;

                var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
                var _accounts = acctRepos.GetAccounts(UserData.COMPANY_XID, queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

                jsonData["query_params"] = JsonConvert.SerializeObject(queryParams);
                jsonData["content"] = await this.RenderViewAsync<List<B2dAccount>>("ApiUserList", _accounts, true);
                jsonData["status"] = "OK";
            }
            catch (Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
        }

        #endregion API帳號區塊

        #region 分銷商註冊

        // 註冊頁面
        [AllowAnonymous]
        public IActionResult Register()
        {
            var timezone = TimeZoneInfo.GetSystemTimeZones();
            //ViewData["TimeZone"] = TimeZoneInfo.GetSystemTimeZones();
            var countryRepos = HttpContext.RequestServices.GetService<CommonRepository>();
            ViewData["CountryAreas"] = countryRepos.GetCountryAreas("zh-tw");
            ViewData["CountryLocales"] = countryRepos.GetCountryLocales();

            //寄送註冊成功通知
            string from_email = "noreply@kkday.com";
            string from_name = "我是測試信";
            Dictionary<string, string> user = new Dictionary<string, string>();
            user.Add("doraemon", "bid@kkday.com");
            string subject = "註冊已完成";
            string body = "請等候通知";

            SendMail.SendTextMail(from_email, from_name, user, subject, body);
                       
            return View();
        }

        // 註冊分銷商
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

        #endregion 分銷商註冊

    }
}
