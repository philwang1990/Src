using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var acctRepos = (AccountRepository)HttpContext.RequestServices.GetService(typeof(AccountRepository));
            var queryParamsModel = new QueryParamsModel(); // acctRepos.GetPagination(string.Empty, string.Empty, 1);

            ViewData["QUERY_PARAMS"] = queryParamsModel;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParamsModel);
            // ViewData["ACCOUNTS"] = acctRepos.GetAccounts(string.Empty, 0, PAGE_SIZE, string.Empty);

            return View();
        }

        // API 使用者
        public IActionResult ApiUser()
        {
            return View();
        }

    }
}
