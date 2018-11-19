using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.App_Code;
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
        const int PAGE_SIZE = 25;
        const int OPTION_PAGE_SIZE = 20;
        readonly ILocalizer _localizer;

        public PriceSettingController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // GET: /<controller>/
        public IActionResult Index(string query)
        {
            var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();
            QueryParamsModel queryParams = null;

            if (!string.IsNullOrEmpty(query))
            {
                query = System.Web.HttpUtility.UrlDecode(query).Replace("&quot;", "\"");
                queryParams = JsonConvert.DeserializeObject<QueryParamsModel>(query);
            }
            else
            {
                queryParams = prsetRepos.GetMstQueryParamModel(string.Empty, string.Empty, PAGE_SIZE, 1);
            }

            ViewData["QUERY_PARAMS"] = queryParams;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParams);
            var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
            var _discMst = prsetRepos.GetDiscountMsts(queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

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
                    queryParams = prsetRepos.GetMstQueryParamModel(queryParams.Filter, queryParams.Sorting, PAGE_SIZE, queryParams.Paging.current_page);
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

        [HttpPost]
        public IActionResult InsertMst([FromBody] B2dDiscountMst mst)
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
         
        [HttpPost]
        public IActionResult UpdateMst([FromBody] B2dDiscountMst mst)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var upd_user = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 修改折扣規則主檔
                prsetRepos.UpdateMst(mst, upd_user);

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

        [HttpPost]
        public IActionResult RemoveMst([FromBody] JObject req)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var del_user = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == "Account").Select(c => c.Value)).FirstOrDefault();
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 移除折扣規則主檔
                var xid = (Int64)req["xid"];
                prsetRepos.RemvoeMst(xid, del_user);

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

        ////////////////////////

        public IActionResult MstProfile(Int64 id)
        {
            var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();
            var commonRepos = HttpContext.RequestServices.GetService<CommonRepository>();
            var _discMst = prsetRepos.GetDiscountMst(id);
            int skip = 0;

            // 建構折扣明細資料模型與分頁參數
            var queryParamsDtl = prsetRepos.GetDtlQueryParamModel(id, string.Empty, string.Empty, OPTION_PAGE_SIZE, 1);
            ViewData["DTL_QUERY_PARAMS"] = queryParamsDtl;
            ViewData["DTL_QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParamsDtl);
           
            skip = (queryParamsDtl.Paging.current_page - 1) * queryParamsDtl.Paging.page_size;
            var dtl_list = prsetRepos.GetDiscountDtls(id, queryParamsDtl.Filter, skip, queryParamsDtl.Paging.page_size, queryParamsDtl.Sorting);
            ViewData["DTL_MODEL"] = dtl_list;

            // 建構折扣多幣別加減價資料模型與分頁參數 
            var queryParamsCurAmt = prsetRepos.GetCurrAmtQueryParamModel(id, string.Empty, string.Empty, OPTION_PAGE_SIZE, 1);
            ViewData["CUR_AMT_QUERY_PARAMS"] = queryParamsCurAmt;
            ViewData["CUR_AMT_QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParamsCurAmt);

            // 多幣別清單
            var locale = User.FindFirst("Locale").Value; 
            ViewData["CURRENCIES"] = commonRepos.GetCurrencies(locale); ;

            skip = (queryParamsCurAmt.Paging.current_page - 1) * queryParamsCurAmt.Paging.page_size;
            var curamt_list = prsetRepos.GetDiscountCurrAmts(id, queryParamsCurAmt.Filter, skip, queryParamsCurAmt.Paging.page_size, queryParamsCurAmt.Sorting);
            ViewData["CURR_AMT_MODEL"] = curamt_list;

            return View(_discMst);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> RefreshDtl([FromQuery] Int64 id, [FromBody]QueryParamsModel queryParams)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();
               
                // 若 RecountFlag=true 更新分頁資料
                if (queryParams.RecountFlag)
                {
                    queryParams = prsetRepos.GetDtlQueryParamModel(id, queryParams.Filter, queryParams.Sorting, OPTION_PAGE_SIZE, queryParams.Paging.current_page);
                }

                ViewData["MST_XID"] = id;
                ViewData["DTL_QUERY_PARAMS"] = queryParams;

                var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
                var _discDtl = prsetRepos.GetDiscountDtls(id, queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

                jsonData["query_params"] = JsonConvert.SerializeObject(queryParams);
                jsonData["content"] = await this.RenderViewAsync<List<B2dDiscountDtl>>("DiscountDtlList", _discDtl, true);
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

        public IActionResult GetDtl(string id)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                jsonData["item"] = JsonConvert.SerializeObject(prsetRepos.GetDiscountDtl(Convert.ToInt64(id)));
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


        [HttpPost]
        public IActionResult InsertDtl([FromBody] B2dDiscountDtl dtl)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var crt_user = User.FindFirst("Account").Value;
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 新增公司與折扣規則對應
                prsetRepos.InsertDtl(dtl, crt_user);
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

        [HttpPost]
        public IActionResult UpdateDtl([FromBody]B2dDiscountDtl dtl)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var del_user = User.FindFirst("Account").Value;
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 新增公司與折扣規則對應
                prsetRepos.UpdateDtl(dtl, del_user);
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

        [HttpPost]
        public IActionResult RemoveDtl([FromBody]JObject req)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var del_user = User.FindFirst("Account").Value;
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 新增公司與折扣規則對應
                prsetRepos.RemvoeDtl(Convert.ToInt64(req["xid"]), del_user);
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

        ///////////////////

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> RefreshCurrAmt([FromQuery] Int64 id, [FromBody]QueryParamsModel queryParams)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>(); 

                // 若 RecountFlag=true 更新分頁資料
                if (queryParams.RecountFlag)
                {
                    queryParams = prsetRepos.GetCurrAmtQueryParamModel(id, queryParams.Filter, queryParams.Sorting, OPTION_PAGE_SIZE, queryParams.Paging.current_page);
                }

                ViewData["MST_XID"] = id;
                ViewData["CUR_AMT_QUERY_PARAMS"] = queryParams;

                var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
                var _discCurrAmt = prsetRepos.GetDiscountCurrAmts(id, queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);

                jsonData["query_params"] = JsonConvert.SerializeObject(queryParams);
                jsonData["content"] = await this.RenderViewAsync<List<B2dDiscountCurrAmt>>("DiscountCurrAmtList", _discCurrAmt, true);
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

        public IActionResult GetCurrAmt(string id)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();
                 
                jsonData["item"] = JsonConvert.SerializeObject(prsetRepos.GetDiscountCurrAmt(Convert.ToInt64(id)));
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

        [HttpPost]
        public IActionResult InsertCurrAmt([FromBody] B2dDiscountCurrAmt dtl)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var crt_user = User.FindFirst("Account").Value;
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 新增公司與折扣規則對應
                prsetRepos.InsertCurrAmt(dtl, crt_user);
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

        [HttpPost]
        public IActionResult UpdateCurrAmt([FromBody] B2dDiscountCurrAmt dtl)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var upd_user = User.FindFirst("Account").Value;
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 新增公司與折扣規則對應
                prsetRepos.UpdateCurrAmt(dtl, upd_user);
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

        [HttpPost]
        public IActionResult RemoveCurrAmt([FromBody]JObject req)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var del_user = User.FindFirst("Account").Value;
                var prsetRepos = HttpContext.RequestServices.GetService<PriceSettingRepository>();

                // 新增公司與折扣規則對應
                prsetRepos.RemvoeCurrAmt(Convert.ToInt64(req["xid"]), del_user);
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
