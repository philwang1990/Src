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
using Newtonsoft.Json.Linq;
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

            var commRepos = HttpContext.RequestServices.GetService<CommonRepository>();
            var locale = User.FindFirst("Locale").Value;
            ViewData["COUNTRIES_LOCALE"] = commRepos.GetCountryAreas(locale);

            return View(_prods); 
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Refresh([FromQuery]Int64 cid, [FromBody]QueryParamsModel queryParams)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var fxpRepos = HttpContext.RequestServices.GetService<FixedPriceRepository>();

                //更新分頁資料
                queryParams = fxpRepos.GetQueryParamModel(cid, queryParams.Filter, queryParams.Sorting, PAGE_SIZE, queryParams.Paging.current_page);
                ViewData["QUERY_PARAMS"] = queryParams;

                var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
                var _prods = fxpRepos.GetFixedPriceProds(cid, queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

                jsonData["query_params"] = JsonConvert.SerializeObject(queryParams);
                jsonData["content"] = await this.RenderViewAsync<List<FixedPriceProductEx>>("ProdList", _prods, true);
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

        public IActionResult InsertProd([FromBody]FixedPriceProduct prod)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var crt_user = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();
                var fxpRepos = HttpContext.RequestServices.GetService<FixedPriceRepository>();
                fxpRepos.InsertProd(prod, crt_user);

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


        // 商品套餐與價格

        public IActionResult PkgPrices(string id, Int64 cid, string cname, Int64 pid, string pname)
        {
            List<FixedPricePackageEx> pkgPrices = new List<FixedPricePackageEx>();

            ViewData["COMP_XID"] = cid;
            ViewData["COMP_NAME"] = cname;
            ViewData["PROD_XID"] = pid;
            ViewData["PROD_NO"] = id;
            ViewData["PROD_NAME"] = pname;

            return View(pkgPrices);
        }

         
    }
}
