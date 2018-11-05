using System;
using System.Collections.Generic;
using System.Linq;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.Models.Model.Company;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.PriceSetting;
using KKday.Web.B2D.BE.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Resources;
using KKday.Web.B2D.BE.Areas.KKday.Models.DataModel;
using System.Diagnostics.Contracts;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Models.Model.Voucher;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.BE.Areas.KKday.Controllers
{
  
    [Area("KKday")]
    [Authorize(Policy = "KKdayOnly")]
    [TypeFilter(typeof(CultureFilter))]
    public class CompanyController : Controller
    {
        const int PAGE_SIZE =3;

        private readonly ILocalizer _localizer;

        public CompanyController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // GET: /<controller>/
        public IActionResult Index(string query)
        {
            //var query = this.Request.Query["query"].ToString();
            var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();
            QueryParamsModel queryParams = null;

            if (!string.IsNullOrEmpty(query))
            {
                query = System.Web.HttpUtility.UrlDecode(query).Replace("&quot;", "\"");
                queryParams = JsonConvert.DeserializeObject<QueryParamsModel>(query);
            }
            else
            {
                queryParams = compRepos.GetQueryParamModel(string.Empty, string.Empty, PAGE_SIZE, 1);
            }
           
            ViewData["QUERY_PARAMS"] = queryParams;
            ViewData["QUERY_PARAMS_JSON"] = JsonConvert.SerializeObject(queryParams);
            var skip = (queryParams.Paging.current_page - 1) * queryParams.Paging.page_size;
            var _companies = compRepos.GetCompanies(queryParams.Filter, skip, PAGE_SIZE, queryParams.Sorting);

            return View(_companies);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Refresh([FromBody]QueryParamsModel queryParams)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();

                // 若 RecountFlag=true 更新分頁資料
                if (queryParams.RecountFlag)
                {
                    queryParams = compRepos.GetQueryParamModel(queryParams.Filter, queryParams.Sorting, PAGE_SIZE, queryParams.Paging.current_page);
                }
                ViewData["QUERY_PARAMS"] = queryParams;

                var skip = (queryParams.Paging.current_page -1) * queryParams.Paging.page_size;
                var _companies = compRepos.GetCompanies(queryParams.Filter, skip, queryParams.Paging.page_size, queryParams.Sorting);
                 
                jsonData["query_params"] = JsonConvert.SerializeObject(queryParams);
                jsonData["content"] = await this.RenderViewAsync<List<B2dCompany>>("CompanyList", _companies, true);
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
         
        public IActionResult Profile(Int64 id)
        {
            try
            {
                var queryArgc = System.Web.HttpUtility.UrlEncode(this.Request.Query["query"].ToString());
                var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();
                var countryRepos = HttpContext.RequestServices.GetService<CommonRepository>();
                var vouchRepos = HttpContext.RequestServices.GetService<VouchAddonRepository>();
                var locale = User.FindFirst("Locale").Value;
                B2dCompany _company = compRepos.GetCompany(id);

                ViewData["VouchAddOn"] = vouchRepos.GetVoucherAddon(id);
                ViewData["QueryParams"] = queryArgc;
                ViewData["CountryAreas"] = countryRepos.GetCountryAreas(locale);
                ViewData["CountryLocales"] = countryRepos.GetCountryLocales();

                return View(_company); 
            }
            catch(Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                return StatusCode(500);
            } 
        }

        [HttpPost]
        public IActionResult Update([FromBody]CompanyUpdModel company)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();
                var upd_user = User.FindFirst("Account").Value;

                //更新分銷商公司資料
                compRepos.Update(company, upd_user);
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
        public IActionResult SetStatus([FromBody] JObject data)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            { 
                var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();
                var upd_user = User.FindFirst("Account").Value;

                compRepos.SetStatus(Convert.ToInt64(data["xid"]), data["status"].ToString(), upd_user);

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

        public IActionResult Promotion(Int64 id, string name)
        {
            try
            {
                var queryArgc = System.Web.HttpUtility.UrlEncode(this.Request.Query["query"].ToString());
                var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();
                var disc_mst_list = compRepos.GetDiscounts(id);

                ViewData["xid"] = id;
                ViewData["CompanyName"] = name;
                ViewData["QueryParams"] = queryArgc;
                return View(disc_mst_list);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                return StatusCode(500);
            }
        }

        public async System.Threading.Tasks.Task<IActionResult> GetAvailDiscounts(Int64 id)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            { 
                var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();  
                var _discounts = compRepos.GetAvailableDiscounts(Convert.ToInt64(id)); 

                jsonData["content"] = await this.RenderViewAsync<List<B2dDiscountMst>>("CompAvailDiscList", _discounts, true);
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

        public IActionResult InsertDiscount([FromBody] JObject req) 
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                Int64 cmp_xid = Convert.ToInt64(req["xid"]);
                Int64[] items = req["disc"].Select(d => (Int64)d).ToArray();
                var crt_user = User.FindFirst("Account").Value;
                var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();

                // 新增公司與折扣規則對應
                compRepos.InsertDiscount(cmp_xid, items, crt_user);
 
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

        public IActionResult RemoveDiscount([FromBody] JObject req)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                Int64 cmp_xid = Convert.ToInt64(req["xid"]);
                Int64 mst_xid = Convert.ToInt64(req["mst_xid"]);
                var del_user = User.FindFirst("Account").Value;
                var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();

                // 刪除公司與折扣規則對應
                compRepos.RemoveDiscount(cmp_xid, mst_xid, del_user);

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
        public async Task<IActionResult> UploadLogo([FromQuery]Int64 cid, List<IFormFile> files)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var upd_user = User.FindFirst("Account").Value;
                var vouchRepos = HttpContext.RequestServices.GetService<VouchAddonRepository>();
                List<string> logo_url = new List<string>();

                var size = files.Sum(f => f.Length);
               
                foreach (var _file in files)
                {
                    if (_file.Length > 0)
                    {
                        var hash_code = System.Text.RegularExpressions.Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                        // full path to file in temp location
                        var filePath = Path.Combine(Path.GetTempPath(), hash_code + "_" + _file.FileName);  // Path.GetTempFileName();

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await _file.CopyToAsync(stream);
                        }

                        // 透過 FS API 把檔案傳上 S3, 並取得 URL 

                        // 儲放網址
                        logo_url.Add(filePath);
                    }
                }

                vouchRepos.UpdateVoucheLogo(cid, logo_url[0], upd_user);

                jsonData["img_url"] = logo_url;
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
        public async Task<IActionResult> UploadLicenses([FromQuery]Int64 cid, List<IFormFile> files)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var upd_user = User.FindFirst("Account").Value;
                var compRepos = HttpContext.RequestServices.GetService<CompanyRepository>();
                List<string> license_url = new List<string>();

                var size = files.Sum(f => f.Length);
               
                foreach (var _file in files)
                {
                    if (_file.Length > 0)
                    {
                        var hash_code = System.Text.RegularExpressions.Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                        // full path to file in temp location
                        var filePath = Path.Combine(Path.GetTempPath(), hash_code + "_" + _file.FileName); // Path.GetTempFileName();
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await _file.CopyToAsync(stream);
                        }

                        // 透過 FS API 把檔案傳上 S3, 並取得 URL

                        // 儲放網址
                        license_url.Add(filePath);
                    }
                }

                compRepos.UpdateLicenses(cid, license_url.ToArray(), upd_user);

                jsonData["img_url"] = license_url;
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
        public IActionResult UpdateVouchAddon([FromBody] B2dVoucherAddon vouch_addon)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var vouchRepos = HttpContext.RequestServices.GetService<VouchAddonRepository>();
                var upd_user = User.FindFirst("Account").Value;

                //更新分銷商公司資料
                vouchRepos.UpdateVouchAddon(vouch_addon, upd_user);
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
