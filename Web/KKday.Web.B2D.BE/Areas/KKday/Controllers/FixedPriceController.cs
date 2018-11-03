using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.Models.Model.FixedProd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.BE.Areas.KKday.Controllers
{
    [Area("KKday")]
    [Authorize(Policy = "KKdayOnly")]
    [TypeFilter(typeof(CultureFilter))]
    public class FixedPriceController : Controller
    {
        // GET: /<controller>/Prods
        public IActionResult Prods(Int64 id, string name)
        {
            List<FixedProduct> prods = new List<FixedProduct>();

            prods.Add(new FixedProduct()
            {
                COMPANY_XID = id,
                COMPANY_NAME = name,
                XID = 1,
                PROD_NO = "18208",
                PROD_NAME = "【迎接 2019】紐約跨年煙火遊船船票"
            });

            ViewData["COMPANY_NAME"] = name;

            return View(prods);
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
