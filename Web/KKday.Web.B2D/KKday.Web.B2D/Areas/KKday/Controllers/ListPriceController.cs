using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.ListPrice;
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
    public class ListPriceController : Controller
    {
        const int PAGE_SIZE = 30;
        readonly ILocalizer _localizer;

        public ListPriceController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // GET: /<controller>/
        public IActionResult BlackList()
        {
            var lpRepos = HttpContext.RequestServices.GetService<ListPriceRepository>();
            var queryParams = lpRepos.GetQueryParamModel(string.Empty, string.Empty, PAGE_SIZE, 1);

            ViewData["QUERY_PARAMS"] = queryParams;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParams);
            var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
            var _prods = lpRepos.GetBlacklistProds(queryParams.Filter, skip, PAGE_SIZE, queryParams.Sorting);

            return View(_prods);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Refresh([FromBody]QueryParamsModel queryParams)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var lpRepos = HttpContext.RequestServices.GetService<ListPriceRepository>();

                //更新分頁資料
                queryParams = lpRepos.GetQueryParamModel(queryParams.Filter, queryParams.Sorting, PAGE_SIZE, queryParams.Paging.current_page);
                ViewData["QUERY_PARAMS"] = queryParams;

                var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
                var _prods = lpRepos.GetBlacklistProds(queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

                jsonData["query_params"] = JsonConvert.SerializeObject(queryParams);
                jsonData["content"] = await this.RenderViewAsync<List<B2dBlacklistProduct>>("BlacklistProdList", _prods, true);
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

        public IActionResult Insert([FromBody]JObject query)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            { 
                var crt_user = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();
                var lpRepos = HttpContext.RequestServices.GetService<ListPriceRepository>();
                lpRepos.Insert(query["prod_no"].ToString(), query["prod_name"].ToString(), crt_user);

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

        public IActionResult Update([FromBody]JObject query)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var upd_user = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();
                var lpRepos = HttpContext.RequestServices.GetService<ListPriceRepository>();
                lpRepos.Update(Convert.ToInt64(query["xid"]), query["prod_no"].ToString(), query["prod_name"].ToString(), upd_user);

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

        public IActionResult Remove([FromBody]JObject query)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var del_user = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();
                var lpRepos = HttpContext.RequestServices.GetService<ListPriceRepository>();
                lpRepos.Remvoe(Convert.ToInt64(query["xid"]), del_user);

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
    }
}
