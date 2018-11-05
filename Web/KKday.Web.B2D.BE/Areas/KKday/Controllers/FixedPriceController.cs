using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.FixedPrice;
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
    public class FixedPriceController : Controller
    {
        const int PAGE_SIZE = 25;

        private readonly ILocalizer _localizer;

        public FixedPriceController(ILocalizer localizer)
        {
            _localizer = localizer;
        }


        // GET: /<controller>/Prods
        public IActionResult Prods(Int64 id, string name)
        {
            ViewData["COMPANY_XID"] = id;
            ViewData["COMPANY_NAME"] = name;

            //var query = this.Request.Query["query"].ToString();
            var fxpRepos = HttpContext.RequestServices.GetService<FixedPriceRepository>();
            QueryParamsModel queryParams = queryParams = fxpRepos.GetQueryParamModel(id, string.Empty, string.Empty, PAGE_SIZE, 1);
             
            ViewData["QUERY_PARAMS"] = queryParams;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParams);
            var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
            var _prods = fxpRepos.GetFixedPriceProds(id, queryParams.Filter, skip, PAGE_SIZE, queryParams.Sorting);

            return View(_prods); 
        }


        public IActionResult PkgPrices(string id, Int64 cid, string cname, Int64 pid, string pname)
        {
            List<FixedProductPackageDtl> pkgPrices = new List<FixedProductPackageDtl>();

            ViewData["COMP_XID"] = cid;
            ViewData["COMP_NAME"] = cname;
            ViewData["PROD_XID"] = pid;
            ViewData["PROD_NO"] = id;
            ViewData["PROD_NAME"] = pname;

            return View(pkgPrices);
        }

         
    }
}
