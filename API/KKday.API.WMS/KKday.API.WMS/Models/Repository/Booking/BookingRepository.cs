using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.DataModel.Booking;
using KKday.API.WMS.AppCode.Proxy;
using System.Collections.Generic;
using Npgsql;
using Newtonsoft.Json;

namespace KKday.API.WMS.Models.Repository.Booking
{
    public class BookingRepository
    {


        public static void InsertOrder(OrderModel queryRQ)
        {



            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.Configuration["ConnectionStrings:NpgsqlConnection"]);
            conn.Open();
            NpgsqlTransaction trans = conn.BeginTransaction();
            String order_no = null;
            string json_data = JsonConvert.SerializeObject(queryRQ);
            JObject obj = JObject.Parse(json_data);

            try
            {
                BookingDAL.InsertOrders(obj, trans, ref order_no);

                if (obj["source"] != null)
                    BookingDAL.InsertOrderSource(obj["source"] as JObject, trans, order_no);

                //if (obj["order_lst"] != null)
                //{
                //    JArray order_lst = (JArray)obj["order_lst"];

                //    foreach (var item in order_lst)
                //    {
                //        BookingDAL.InsertOrderLst(item as JObject, trans, order_no);
                //    } // foreach
                //} // if

                //BookingDAL.InsertOrderCus(obj, trans, order_no);
                //BookingDAL.InsertOrderDiscountRuleMst(obj, trans, order_no);
                //BookingDAL.InsertOrderDiscountRuleDtl(obj, trans, order_no);

                trans.Commit();

                conn.Close();



            }
            catch (Exception ex)
            {
                // 如果還沒commit 回傳false
                if(!trans.IsCompleted){
                    trans.Rollback();
                    conn.Close();
                }

                Website.Instance.logger.FatalFormat($"getCurrency  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }

            //return currency;

        }



    }
}
