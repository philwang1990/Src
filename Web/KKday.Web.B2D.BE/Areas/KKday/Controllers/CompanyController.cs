using System;
using System.Collections.Generic;  
using KKday.Web.B2D.BE.Areas.KKday.Models.DataModel.Company;
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
    public class CompanyController : Controller
    {
        const int PAGE_SIZE = 50;

        private readonly ILocalizer _localizer;

        public CompanyController(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var compRepos = (CompanyRepository)HttpContext.RequestServices.GetService(typeof(CompanyRepository));
           
            var rec_count = compRepos.GetCompanyCount(null);
            var total_pages = (int)(rec_count / PAGE_SIZE) + ((rec_count % PAGE_SIZE != 0) ? 1 : 0);

           var lookupModel = new LookupModel()
            {
                Paging = new Pagination() {
                    current_page = 1,
                    total_pages = total_pages,
                    page_size = PAGE_SIZE
                }
            }; 

            ViewData["LOOKUP_MODEL"] = lookupModel;
            ViewData["LOOKUP_MODEL_JSON"] = JsonConvert.SerializeObject(lookupModel);
            ViewData["COMPANIES"] = compRepos.GetCompanies(string.Empty, 0, PAGE_SIZE, string.Empty);

            return View();
        }

        [HttpPost]
        public IActionResult Lookup(LookupModel lookup)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var compRepos = (CompanyRepository)HttpContext.RequestServices.GetService(typeof(CompanyRepository));

                var skip = (lookup.Paging.current_page -1) * lookup.Paging.page_size;
                compRepos.GetCompanies(lookup.Filter, skip, lookup.Paging.page_size, lookup.Sorting);

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
