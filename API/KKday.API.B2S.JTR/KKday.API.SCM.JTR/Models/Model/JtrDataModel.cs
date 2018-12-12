using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace KKday.API.B2S.JTR.Models.Model
{
    public class JtrDataModel
    {
        public List<Mappinginfo> mappinginfo { get; set; }
        public List<JtrRsinfo> jtrrsinfo { get; set; }
    }

    public class Mappinginfo
    {
        public string price_type { get; set; }
        public string jtr_prod_no { get; set; }
        public int qty { get; set; }
    }

    public class JtrRsinfo
    {
        public string kkOrder_no { get; set; }
        public string kkprice_type { get; set; }
        public int kkprice_qty { get; set; }

        public string order_error_state { get; set; }
        public string order_error_msg { get; set; }
        public string order_id { get; set; }

        public string pay_error_state { get; set; }
        public string pay_error_msg { get; set; }
        public string code { get; set; }

        public List<string> codeUrls { get; set; }

    }

    [Serializable]
    public class order
    {
        [XmlElement(ElementName = "peoples")]
        public Peoples peoples { get; set; }

        [XmlElement(ElementName = "travel_date")]
        public string travel_date { get; set; }
        [XmlElement(ElementName = "end_travel_date")]
        public string end_travel_date { get; set; }
        [XmlElement(ElementName = "arrived_time")]
        public string arrived_time { get; set; }
        [XmlElement(ElementName = "info_id")]
        public string info_id { get; set; }
        [XmlElement(ElementName = "cust_id")]
        public string cust_id { get; set; }
        [XmlElement(ElementName = "get_type")]
        public string get_type { get; set; }
        [XmlElement(ElementName = "order_source_id")]
        public string order_source_id { get; set; }
        [XmlElement(ElementName = "order_memo")]
        public string order_memo { get; set; }
        [XmlElement(ElementName = "num")]
        public int num { get; set; }
        [XmlElement(ElementName = "user_id")]
        public string user_id { get; set; }
        [XmlElement(ElementName = "link_man")]
        public string link_man { get; set; }
        [XmlElement(ElementName = "link_phone")]
        public string link_phone { get; set; }
        [XmlElement(ElementName = "link_address")]
        public string link_address { get; set; }
        [XmlElement(ElementName = "link_code")]
        public string link_code { get; set; }
        [XmlElement(ElementName = "link_email")]
        public string link_email { get; set; }
        [XmlElement(ElementName = "link_credit_type")]
        public string link_credit_type { get; set; }
        [XmlElement(ElementName = "link_credit_no")]
        public string link_credit_no { get; set; }

    }

    [XmlRoot(ElementName = "peoples")]
    public class Peoples
    {
        [XmlElement(ElementName = "people")]
        public People PersonData { get; set; }
    }


    [XmlRoot(ElementName = "people")]
    public class People
    {
        [XmlElement(ElementName = "link_man")]
        public string link_man { get; set; }
        [XmlElement(ElementName = "link_credit_type")]
        public string link_credit_type { get; set; }
        [XmlElement(ElementName = "link_credit_no")]
        public string link_credit_no { get; set; }
    }



    [Serializable]
    [XmlRoot(ElementName = "result")]
    public class order_result
    {
        [XmlElement(ElementName = "status")]
        public string status { get; set; }
        [XmlElement(ElementName = "msg")]
        public string msg { get; set; }
        [XmlElement(ElementName = "order_id")]
        public string order_id { get; set; }
        [XmlElement(ElementName = "error_state")]
        public string error_state { get; set; }
        [XmlElement(ElementName = "error_msg")]
        public string error_msg { get; set; }
        [XmlElement(ElementName = "order_money")]
        public string order_money { get; set; }
        [XmlElement(ElementName = "mem_order_money")]
        public string mem_order_money { get; set; }
        [XmlElement(ElementName = "order_state")]
        public string order_state { get; set; }

    }

    [Serializable]
    [XmlRoot(ElementName = "result")]
    public class pay_result
    {
        [XmlElement(ElementName = "orderId")]
        public string orderId { get; set; }
        [XmlElement(ElementName = "status")]
        public string status { get; set; }
        [XmlElement(ElementName = "msg")]
        public string msg { get; set; }
        [XmlElement(ElementName = "code")]
        public string code { get; set; }
        [XmlElement(ElementName = "error_msg")]
        public string error_msg { get; set; }
        [XmlElement(ElementName = "error_state")]
        public string error_state { get; set; }

    }


    [Serializable]
    [XmlRoot(ElementName = "result")]
    public class orderDetail_result
    {
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "orders")]
        public Orders Orders { get; set; }
    }

    [XmlRoot(ElementName = "orders")]
    public class Orders
    {
        [XmlElement(ElementName = "order")]
        public OrderInfo Order { get; set; }
        [XmlElement(ElementName = "peoples")]
        public string Peoples { get; set; }
    }

    [XmlRoot(ElementName = "order")]
    public class OrderInfo
    {
        //[XmlElement(ElementName = "orderState")]
        //public string OrderState { get; set; }
        //[XmlElement(ElementName = "isOnlinepay")]
        //public string IsOnlinepay { get; set; }
        //[XmlElement(ElementName = "isConfirm")]
        //public string IsConfirm { get; set; }
        //[XmlElement(ElementName = "getType")]
        //public string GetType { get; set; }
        [XmlElement(ElementName = "code")]
        public string Code { get; set; }
        //[XmlElement(ElementName = "security_code")]
        //public string Security_code { get; set; }
        [XmlElement(ElementName = "qrCodeUrl")]
        public string QrCodeUrl { get; set; }
        //[XmlElement(ElementName = "qrType")]
        //public string QrType { get; set; }
        //[XmlElement(ElementName = "getPrice")]
        //public string GetPrice { get; set; }
        //[XmlElement(ElementName = "freeMemo")]
        //public string FreeMemo { get; set; }
        //[XmlElement(ElementName = "cancelState")]
        //public string CancelState { get; set; }
        //[XmlElement(ElementName = "cancelStateTXT")]
        //public string CancelStateTXT { get; set; }
        //[XmlElement(ElementName = "productNo")]
        //public string ProductNo { get; set; }
        //[XmlElement(ElementName = "productName")]
        //public string ProductName { get; set; }
        //[XmlElement(ElementName = "num")]
        //public string Num { get; set; }
        //[XmlElement(ElementName = "salePrice")]
        //public string SalePrice { get; set; }
        //[XmlElement(ElementName = "marketPrice")]
        //public string MarketPrice { get; set; }
        //[XmlElement(ElementName = "orderMoney")]
        //public string OrderMoney { get; set; }
        //[XmlElement(ElementName = "memOrderMoney")]
        //public string MemOrderMoney { get; set; }
        //[XmlElement(ElementName = "payMoney")]
        //public string PayMoney { get; set; }
        [XmlElement(ElementName = "sourceId")]
        public string SourceId { get; set; }
        //[XmlElement(ElementName = "isCancel")]
        //public string IsCancel { get; set; }
        //[XmlElement(ElementName = "linkMan")]
        //public string LinkMan { get; set; }
        //[XmlElement(ElementName = "linkPhone")]
        //public string LinkPhone { get; set; }
        //[XmlElement(ElementName = "linkEmail")]
        //public string LinkEmail { get; set; }
        //[XmlElement(ElementName = "linkAddress")]
        //public string LinkAddress { get; set; }
        //[XmlElement(ElementName = "linkCreditType")]
        //public string LinkCreditType { get; set; }
        //[XmlElement(ElementName = "linkCreditNo")]
        //public string LinkCreditNo { get; set; }
        //[XmlElement(ElementName = "travelDate")]
        //public string TravelDate { get; set; }
        //[XmlElement(ElementName = "orderDate")]
        //public string OrderDate { get; set; }
        //[XmlElement(ElementName = "cancelDate")]
        //public string CancelDate { get; set; }
        //[XmlElement(ElementName = "userId")]
        //public string UserId { get; set; }
        //[XmlElement(ElementName = "orderMemo")]
        //public string OrderMemo { get; set; }
        //[XmlElement(ElementName = "finishNum")]
        //public string FinishNum { get; set; }
        //[XmlElement(ElementName = "payUrl")]
        //public string PayUrl { get; set; }
        //[XmlElement(ElementName = "cancelMoney")]
        //public string CancelMoney { get; set; }
    }

}
