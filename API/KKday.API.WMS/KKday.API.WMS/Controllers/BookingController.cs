using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.API.WMS.Models.Repository.Book;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {

    [Route("api/[controller]")]
    public class BookingController : Controller {

        [HttpGet("InsertOrder")]
        public void InsertOrder()
        {
            //Website.Instance.logger.Info($"WMS QueryProduct Start! B2D Xid:{queryRQ.company_xid},KKday ProdOid:{queryRQ.prod_no}");

            //var prod_dtl = new ProductModel();

            //prod_dtl = ProductRepository.GetProdDtl(queryRQ);

            BookRepository.InsertOrder();

            //return prod_dtl;
        }
    }
}
