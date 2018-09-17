using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.API.WMS.Models.DataModel.Product;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {

    [Route("api/[controller]")]
    public class ProductController : Controller {

        [HttpPost("QueryProduct")]
        public ProductResponseModel QueryProduct([FromBody]QueryProdRQModel list_rq)
        {
            var prod_dtl = new ProductResponseModel();
            //prod_dtl = ProdRepository.GetProdList(list_rq);

            return prod_dtl;
        }

    }




}
