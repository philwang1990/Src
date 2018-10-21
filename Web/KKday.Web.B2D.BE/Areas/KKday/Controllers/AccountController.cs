using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.App_Code;
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

namespace KKday.Web.B2D.BE.Areas.KKday.Controllers
{
    [Area("KKday")]
    [Authorize(Policy = "KKdayOnly")]
    [TypeFilter(typeof(CultureFilter))]
    public class AccountController : Controller
    {
        const int PAGE_SIZE = 1;

        readonly ILocalizer _localizer;

        public AccountController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // GET: /<controller>/
        public IActionResult Index()
        { 
            var acctRepos = HttpContext.RequestServices.GetService<B2dAccountRepository>();

            var queryParamsModel = acctRepos.GetQueryParamModel(string.Empty, string.Empty, PAGE_SIZE, 1);

            ViewData["QUERY_PARAMS"] = queryParamsModel;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParamsModel);
            ViewData["ACCOUNTS"] = acctRepos.GetAccounts(string.Empty, 0, PAGE_SIZE, string.Empty);

            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Refresh([FromBody]QueryParamsModel queryParams)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var acctRepos = (B2dAccountRepository)HttpContext.RequestServices.GetService<B2dAccountRepository>();
                //更新分頁資料
                queryParams = acctRepos.GetQueryParamModel(queryParams.Filter, queryParams.Sorting, PAGE_SIZE, queryParams.Paging.current_page);

                var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
                var accounts = acctRepos.GetAccounts(queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

                ViewData["QUERY_PARAMS"] = queryParams;
                ViewData["ACCOUNTS"] = accounts;

                jsonData["query_params"] = JsonConvert.SerializeObject(queryParams);
                jsonData["content"] = await this.RenderViewAsync<string>("AccountList", null, true);
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

        public async System.Threading.Tasks.Task<IActionResult> RenderEdit(Int64 id)
        {
            string shtml = "";

            try
            {
                var acctRepos = HttpContext.RequestServices.GetService<B2dAccountRepository>();
                var ctryRepos = HttpContext.RequestServices.GetService<CountryRepository>();
                var locale = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Locale").Select(c => c.Value)).FirstOrDefault();

                B2dAccount _account = acctRepos.GetAccount(id);
                  
                shtml = await this.RenderViewAsync<B2dAccount>("CompanyEdit", _account, true);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                return StatusCode(500);
            }

            return Content(shtml);
        }


        /////////////////////////
         

        #region API User

        // API 使用者
        public IActionResult ApiUser()
        {
            return View();
        }

        #endregion API User

    }
}
