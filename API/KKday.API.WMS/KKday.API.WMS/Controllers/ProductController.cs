using KKday.API.WMS.Models.DataModel.Package;
using KKday.API.WMS.Models.DataModel.Product;
using KKday.API.WMS.Models.Repository.Product;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {
  
    [Route("api/[controller]")]
    public class ProductController : Controller {

        private readonly ProductRepository _Product;
        public ProductController(ProductRepository Product) {
            _Product = Product;
        }

        /// <summary>
        /// Queries the product.
        /// </summary>
        /// <returns>The product.</returns>
        /// <param name="queryRQ">Query rq.</param>
        [HttpPost("QueryProduct")]
        public ProductModel QueryProduct([FromBody]QueryProductModel queryRQ)
        {
            Website.Instance.logger.Info($"WMS QueryProduct Start! B2D Xid:{queryRQ.company_xid},KKday ProdOid:{queryRQ.prod_no}");

            var prod_dtl = new ProductModel();

            prod_dtl = _Product.GetProdDtl(queryRQ);

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

            Website.Instance.logger.Info($"WMS QueryPackage Start! B2D Xid:{queryRQ.company_xid},KKday ProdOid:{queryRQ.prod_no}");
                       
            PackageModel pkg_dtl = _Product.GetPkgLst(queryRQ);

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

            Website.Instance.logger.Info($"WMS QueryEvent Start! B2D Xid:{queryRQ.company_xid},KKday ProdOid:{queryRQ.prod_no}");

            PkgEventsModel pkg_events = _Product.GetPkgEvents(queryRQ);

            return pkg_events;
        }

        /// <summary>
        /// Queries the module.
        /// </summary>
        /// <returns>The module.</returns>
        /// <param name="queryRQ">Query rq.</param>
        [HttpPost("QueryModule")]
        public ProductModuleModel QueryModule([FromBody]QueryProductModel queryRQ)
        {
            Website.Instance.logger.Info($"WMS QueryModule Start! B2D Xid:{queryRQ.company_xid},KKday ProdOid:{queryRQ.prod_no}");

            var prod_model = new ProductModuleModel();

            prod_model = _Product.GetProdModule(queryRQ);

            return prod_model;
        }

    }




}
