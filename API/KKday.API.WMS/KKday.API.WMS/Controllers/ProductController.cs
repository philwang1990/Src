using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.API.WMS.Models.DataModel.Package;
using KKday.API.WMS.Models.DataModel.Product;
using KKday.API.WMS.Models.Repository.Package;
using KKday.API.WMS.Models.Repository.Product;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {

    [Route("api/[controller]")]
    public class ProductController : Controller {

        [HttpPost("QueryProduct")]
        public ProductModel QueryProduct([FromBody]QueryProductModel queryRQ)
        {
            var prod_dtl = new ProductModel();

            prod_dtl = ProductRepository.GetProdDtl(queryRQ);

            return prod_dtl;
        }
        /// <summary>
        /// Queries the package.
        /// </summary>
        /// <returns>The package.</returns>
        /// <param name="queryRQ">Query rq.</param>
        /// 取套餐
        [HttpPost("QueryPackage")]
        public PackageModel QueryPackage([FromBody]QueryProductModel queryRQ) {

            var pkg_dtl = new PackageModel();

            pkg_dtl = PackageRepository.GetPkgLst(queryRQ);

            return pkg_dtl;
        }
        /// <summary>
        /// Queries the package events.
        /// </summary>
        /// <returns>The package events.</returns>
        /// <param name="queryRQ">Query rq.</param>
        /// 取套餐場次
        [HttpPost("QueryEvent")]
        public PkgEventsModel QueryPkgEvents([FromBody]QueryProductModel queryRQ) {

            PkgEventsModel pkg_events = PackageRepository.GetPkgEvents(queryRQ);

            return pkg_events;
        }

    }




}
