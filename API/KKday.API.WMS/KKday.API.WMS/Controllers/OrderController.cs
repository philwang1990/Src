
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.API.WMS.Models.DataModel.Order;
using KKday.API.WMS.Models.Repository.Order;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        [HttpPost("QueryOrders")]
        public OrderListModel QueryOrders([FromBody]QueryOrderModel queryRQ)
        {
            Website.Instance.logger.Info($"WMS QueryOrders Start! B2D Xid:{queryRQ.company_xid}");

            var orders = new OrderListModel();

            return orders = OrderRepository.GetOrders(queryRQ);
        }

        [HttpPost("QueryOrderInfo/{order_no}")]
        public OrderInfoModel QueryOrderInfo([FromBody]QueryOrderModel queryRQ,string order_no)
        {
            Website.Instance.logger.Info($"WMS QueryOrderInfo Start! B2D Xid:{queryRQ.company_xid},B2D OrderNo:{order_no}");

            var order_info = new OrderInfoModel();

            return order_info = OrderRepository.GetOrderInfo(queryRQ, order_no);
        }
    }
}

