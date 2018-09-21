using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
<<<<<<< HEAD
=======
using KKday.API.WMS.Models.DataModel.Product;
using KKday.API.WMS.Models.Repository.Product;
>>>>>>> lulu_branch
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {
    public class ProductController : Controller {
<<<<<<< HEAD
        // GET: /<controller>/
        public IActionResult Index() {
            return View();
=======

        [HttpPost("QueryProduct")]
        public ProductResponseModel QueryProduct([FromBody]QueryProdRquestModel queryRQ)
        {
            var prod_dtl = new ProductResponseModel();
            prod_dtl = ProductRepository.GetProdDtl(queryRQ);

            return prod_dtl;
>>>>>>> lulu_branch
        }
    }
}
