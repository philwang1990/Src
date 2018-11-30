using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Filters;
using KKday.Web.B2D.BE.Models.DataModel;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.Order;
using KKday.Web.B2D.BE.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.BE.Areas.User.Controllers
{
    [Area("User")]
    [TypeFilter(typeof(CultureFilter))]
    [Authorize(Policy = "UserOnly")]
    public class OrderController : Controller
    {
        const int Page_Size = 5;

        // GET: /<controller>/
        public IActionResult Index()
        {
            //準備參數
            //CALL WMS獲得訂單查詢結果
            //計算分頁數
            //以MODELS（訂單列表）帶到view

            try
            {
                var aesUserData = User.FindFirst(ClaimTypes.UserData).Value;
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                OrderOptionModel option = new OrderOptionModel()
                {
                    PAGE_SIZE = Page_Size,
                    CURRENT_PAGE = 1
                    //TIME_ZONE = "",
                    //PROD_SDATE = "",
                    //PROD_EDATE = "",
                    //ORDER_SDATE = "",
                    //ORDER_EDATE = "",
                    //ORDERS = search.ORDER_NO;
                };

                var orders = OrderRepository.GetOrderList(UserData,option);

                return View(orders);
            }

            catch(Exception ex)
            {
                throw ex ;
            }
        }

        // 查詢訂單
        [HttpPost]
        public async Task<IActionResult> SearchOrdersAsync([FromBody] OrderModel search)
        {
            Dictionary<string, string> jsonData = new Dictionary<string, string>();

            try
            {
                var aesUserData = User.FindFirst(ClaimTypes.UserData).Value;
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                OrderOptionModel option = new OrderOptionModel()
                {
                    PAGE_SIZE = Page_Size,
                    CURRENT_PAGE = 1,
                    TIME_ZONE = "",
                    PROD_SDATE = "",
                    PROD_EDATE = "",
                    ORDER_SDATE = "",
                    ORDER_EDATE = "",
                    ORDERS = new string[] { "18KK12345" }
                };

                var orders = OrderRepository.GetOrderList(UserData, option);

                jsonData.Add("status", "OK");
                jsonData["content"] = await this.RenderViewAsync<OrderListModel>("OrderList", orders, true);
            }
            catch(Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "ERROR");
                jsonData.Add("content", ex.Message);
            }

            return Json(jsonData);
        }

        // 刷新頁面
        [HttpPost]
        public async Task<IActionResult> RefreshAsync([FromBody]OrderOptionModel query)
        {
            Dictionary<string, object> jsonData = new Dictionary<string, object>();

            try
            {
                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                var _orders = OrderRepository.GetOrderList(UserData, query);

                jsonData["query_params"] = JsonConvert.SerializeObject(query);
                jsonData["content"] = await this.RenderViewAsync<OrderListModel>("OrderList", _orders, true);
                jsonData["status"] = "OK";
            }
            catch (Exception ex)
            {
                jsonData.Clear();
                jsonData.Add("status", "FAIL");
                jsonData.Add("msg", ex.Message);
            }

            return Json(jsonData);
        }

    }
}
