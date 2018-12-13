
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
        private readonly OrderRepository _Order;
        public OrderController(OrderRepository Order) {
            _Order = Order;
        }

        [HttpPost("QueryOrders")]
        public OrderListModel QueryOrders([FromBody]QueryOrderModel queryRQ)
        {
            Website.Instance.logger.Info($"WMS QueryOrders Start! B2D Xid:{queryRQ.company_xid}");

            var orders = new OrderListModel();

            return orders = _Order.GetOrders(queryRQ);
        }

        [HttpPost("QueryOrderInfo/{order_no}")]
        public OrderInfoModel QueryOrderInfo([FromBody]QueryOrderModel queryRQ,string order_no)
        {
            Website.Instance.logger.Info($"WMS QueryOrderInfo Start! B2D Xid:{queryRQ.company_xid},B2D OrderNo:{order_no}");

            var order_info = new OrderInfoModel();
          
            return order_info = _Order.GetOrderInfo(queryRQ, order_no);
        }

        ////////////以下為憑證
        //[HttpPost("QueryFileList")]
        //public FileListModel QueryFileList([FromBody]QueryOrderModel queryRQ) {
        //    Website.Instance.logger.Info($"WMS QueryOrders Start! B2D Xid:{queryRQ.company_xid}");

        //    var files = new FileListModel();

        //    return files = OrderRepository.GetOrders(queryRQ);
        //}

    }
}

