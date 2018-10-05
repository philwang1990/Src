using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.API.WMS.Models.DataModel.Product;
using KKday.API.WMS.Models.Repository.Product;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {
  
    [Route("api/[controller]")]
    public class ProductController : Controller {

        /// <summary>
        /// Queries the product.
        /// </summary>
        /// <returns>The product.</returns>
        /// <param name="queryRQ">Query rq.</param>
        [HttpPost("QueryProduct")]
        public ProductModel QueryProduct([FromBody]QueryProductModel queryRQ)
        {
            var prod_dtl = new ProductModel();

            prod_dtl = ProductRepository.GetProdDtl(queryRQ);

            return prod_dtl;
        }


        /// <summary>
        /// Queries the module.
        /// </summary>
        /// <returns>The module.</returns>
        /// <param name="queryRQ">Query rq.</param>
        [HttpPost("QueryModule")]
        public ProductModuleModel QueryModule([FromBody]QueryProductModel queryRQ)
        {
            var prod_model = new ProductModuleModel();

            prod_model = ProductRepository.GetProdModule(queryRQ);

            return prod_model;
        }

    }




}
