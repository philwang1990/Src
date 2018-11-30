using System;
using System.Collections.Generic;
using System.Linq;
using KKday.Web.B2D.BE.Models.DataModel;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Model.Common;
using KKday.Web.B2D.BE.Models.Model.Order;
using KKday.Web.B2D.BE.Proxy;
using Newtonsoft.Json.Linq;

namespace KKday.Web.B2D.BE.Models.Repository

{
    public class OrderRepository
    {
        public static OrderListModel GetOrderList(B2dAccount acct, OrderOptionModel option)
        {
            OrderRQModel rq = new OrderRQModel()
            {
                COMP_XID = acct.COMPANY_XID,
                CHANNEL_OID = 456,//UserData.CHANNEL_OID,
                LOCALE_LANG = acct.LOCALE,
                CURRENT_CURRENCY = acct.CURRENCY,
                STATE = "TW",//acct.CHANNEL_OID,
                OPTION = option
            };

            var jsonResult = OrderProxy.GetOrderList(rq);

            OrderListModel order_list = new OrderListModel();
            JObject jsonObject = JObject.Parse(jsonResult);
            //order_list = jsonObject["order"].ToObject<List<OrderModel>>();

            order_list.ORDERS = new List<OrderModel>();
            order_list.PAGES = (Int32)jsonObject["order_qty"] / option.PAGE_SIZE + (((Int32)jsonObject["order_qty"] % option.PAGE_SIZE != 0) ? 1 : 0);
            order_list.CURRENT_PAGE = (Int32)jsonObject["current_page"];

            foreach (JToken item in jsonObject["order"].AsJEnumerable())
            {
                order_list.ORDERS.Add(new OrderModel()
                {
                    ORDER_NO = item["orderNo"].ToString(),
                    ORDER_MID = item["orderMid"].ToString(),
                    ORDER_OID = item["orderOid"].ToString(),
                    ORDER_DATE = item["crtDt"].ToString(),
                    PROD_SDATE = item["begLstGoDt"].ToString(),
                    ORDER_STATUS = item["orderStatusTxt"].ToString(),
                    PROD_NAME = item["productName"].ToString(),
                    PKG_NAME = item["packageName"].ToString(),
                    QTY = item["qtyTotal"].ToString()
                });
            }



            return order_list;
        }

        public QueryParamsModel GetQueryParamModel(string filter, string sorting, int size, int current_page)
        {
            var rec_count = GetAccountsCount(filter);
            var total_pages = (int)(rec_count / size) + ((rec_count % size != 0) ? 1 : 0);

            return new QueryParamsModel()
            {
                Filter = filter,
                Sorting = sorting,
                Paging = new Pagination()
                {
                    current_page = current_page,
                    total_pages = total_pages,
                    page_size = size
                }
            };
        }

        // 取得共有多少筆訂單[分頁用]
        public int GetAccountsCount(string filter)
        {
            try
            {
                var _filter = GetFieldFiltering(filter);
                //var order_list= OrderProxy.GetOrderList(_filter);

                return 0;//order_list.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Fields Mapping

        private string GetFieldFiltering(string filter)
        {
            var jObjFilter = string.IsNullOrEmpty(filter) ? new JObject() : JObject.Parse(filter);
            var _filter = "";

            // Full Name
            if (!string.IsNullOrEmpty((string)jObjFilter["name"]))
                _filter += $" AND LOWER(name_first || name_last) LIKE '%{jObjFilter["name"]}%' ";
            // Company Name
            if (!string.IsNullOrEmpty((string)jObjFilter["comp_name"]))
                _filter += $" AND LOWER(comp_name) LIKE '%{jObjFilter["comp_name"]}%' ";
            // Email
            if (!string.IsNullOrEmpty((string)jObjFilter["email"]))
                _filter += $" AND LOWER(email) LIKE '%{jObjFilter["email"]}%' ";
            // enable
            if (!string.IsNullOrEmpty((string)jObjFilter["status"]))
                _filter += $" AND enable = '{jObjFilter["status"]}' ";

            return _filter;
        }

        private string GetFieldSorting(string sorting)
        {
            var _sorting = "";

            switch (sorting)
            {
                case "name": _sorting = "name"; break;
                case "email": _sorting = "email"; break;
                case "comp_name": _sorting = "comp_name"; break;
                case "enable": _sorting = "enable"; break;
                case "xid": _sorting = "xid"; break;

                default: break;
            }

            return _sorting;
        }

        #endregion Fields Mapping
    }
}
