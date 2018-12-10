using System;
namespace KKday.Web.B2D.Models.BE.Model.Order
{
    public class OMDL_Module
    {

        public class OrderCusList
        {
            //旅客資料(原 PMDL_CUST_DATA)
            public int COUNTS { get; set; }         //總筆數order_qty

        }

        public class OMDL_PSGR_DATA
        {
            //車輛乘客資料(原 PMDL_CAR_PSGR)
            public int COUNTS { get; set; }         //總筆數order_qty

        }

        public class OMDL_RENT_CAR
        {
            //租車資料(原 PMDL_RENT_CAR)
            public int COUNTS { get; set; }         //總筆數order_qty

        }

        public class OMDL_SEND_DATA
        {
            //寄送資料(原 PMDL_SEND_DATA)
            public int COUNTS { get; set; }         //總筆數order_qty

        }

        public class OMDL_CONTACT_DATA
        {
            //聯絡資料(原 PMDL_CONTACT_DATA)
            public int COUNTS { get; set; }         //總筆數order_qty

        }

        public class OMDL_FLIGHT_INFO
        {
            //航班資料(原 PMDL_FLIGHT_INFO)
            public int COUNTS { get; set; }         //總筆數order_qty

        }

        public class OMDL_SHUTTLE
        {
            //租車接送資料(原 PMDL_VENUE + PMDL_RENT_CAR)
            public int COUNTS { get; set; }         //總筆數order_qty

        }

        public class OrderLiOMDL_OTHER_DATAstModel
        {
            //網卡WIFI機等其他資料(原 PMDL_SIM_WIFI + PMDL_EXCHANGE)
            public int COUNTS { get; set; }         //總筆數order_qty

        }
    }
}
