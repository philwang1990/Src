using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.API.WMS.Models.Repository.Booking;
using KKday.API.WMS.Models.DataModel.Booking;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {

    [Route("api/[controller]")]
    public class BookingController : Controller {

        [HttpPost("InsertOrder")]
        public OrderNoModel InsertOrder([FromBody]OrderModel queryRQ)
        {
            //Website.Instance.logger.Info($"WMS QueryProduct Start! B2D Xid:{queryRQ.company_xid},KKday ProdOid:{queryRQ.prod_no}");

            //var prod_dtl = new ProductModel();

            //prod_dtl = ProductRepository.GetProdDtl(queryRQ);

            //BookingRepository.InsertOrder(queryRQ);

            Website.Instance.logger.Info($"WMS InsertOrder Start! ");

            return BookingRepository.InsertOrder(queryRQ);
        }

        [HttpPost("UpdateOrder")]
        public ActionResult UpdateOrder([FromBody]UpdateOrderModel queryRQ)
        {

            Website.Instance.logger.Info($"WMS UpdateOrder Start! ");

            return Content(BookingRepository.UpdateOrder(queryRQ).ToString(), "application/json");
        }
    }
}
