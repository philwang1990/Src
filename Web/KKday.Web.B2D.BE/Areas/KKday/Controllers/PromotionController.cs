using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Filters;
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
    public class PromotionController : Controller
    {
        const int PAGE_SIZE = 30;
        readonly ILocalizer _localizer;

        public PromotionController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var promoRepos = HttpContext.RequestServices.GetService<PromotionRepository>();
            var queryParams = promoRepos.GetQueryParamModel(string.Empty, string.Empty, PAGE_SIZE, 1);

            ViewData["QUERY_PARAMS"] = queryParams;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParams);
            var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
            var _discMst = promoRepos.GetDiscountMsts(queryParams.Filter, skip, PAGE_SIZE, queryParams.Sorting);

            return View(_discMst);
        }
    }
}
