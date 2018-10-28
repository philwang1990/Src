using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.PriceSetting;
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
    public class PriceSettingController : Controller
    {
        const int PAGE_SIZE = 30;
        readonly ILocalizer _localizer;

        public PriceSettingController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();
            var queryParams = prsetRepos.GetQueryParamModel(string.Empty, string.Empty, PAGE_SIZE, 1);

            ViewData["QUERY_PARAMS"] = queryParams;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParams);
            var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
            var _discMst = prsetRepos.GetDiscountMsts(queryParams.Filter, skip, PAGE_SIZE, queryParams.Sorting);

            return View(_discMst);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Refresh([FromBody]QueryParamsModel queryParams)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 若 RecountFlag=true 更新分頁資料
                if (queryParams.RecountFlag)
                {
                    queryParams = prsetRepos.GetQueryParamModel(queryParams.Filter, queryParams.Sorting, PAGE_SIZE, queryParams.Paging.current_page);
                }
                ViewData["QUERY_PARAMS"] = queryParams;

                var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
                var _discMst = prsetRepos.GetDiscountMsts(queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

                jsonData["query_params"] = JsonConvert.SerializeObject(queryParams);
                jsonData["content"] = await this.RenderViewAsync<List<B2dDiscountMst>>("DiscountMstList", _discMst, true);
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

        public IActionResult InsertDiscount([FromBody] B2dDiscountMst mst)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var crt_user = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 新增公司與折扣規則對應
                prsetRepos.InsertMst(mst, crt_user);

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

        public IActionResult MstProfile(Int64 id) 
        {
            return View();
        }
    }
}
