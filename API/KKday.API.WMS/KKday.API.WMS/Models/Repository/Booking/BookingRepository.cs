using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.DataModel.Booking;
using KKday.API.WMS.AppCode.Proxy;
using System.Collections.Generic;
using Npgsql;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace KKday.API.WMS.Models.Repository.Booking
{
    public class BookingRepository
    {


        public static OrderNoModel InsertOrder(OrderModel queryRQ)
        {

            OrderNoModel orderNo = new OrderNoModel();

            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.Configuration["ConnectionStrings:NpgsqlConnection"]);
            conn.Open();
            NpgsqlTransaction trans = conn.BeginTransaction();
            String order_no = null;
            string json_data = JsonConvert.SerializeObject(queryRQ);
            JObject obj = JObject.Parse(json_data);
            List<int> cus_seqno = new List<int>();
            List<int> lst_seqno = new List<int>();

            try
            {
                // 先將 order_cus order_lst  seq設0
                BookingDAL.InitialSeqs();

                BookingDAL.InsertOrders(obj, trans, ref order_no);

                if (obj["source"] != null)
                    BookingDAL.InsertOrderSource(obj["source"] as JObject, trans, order_no);

                if (obj["order_cus"] != null)
                {
                    JArray order_cus = (JArray)obj["order_cus"];

                    foreach (var item in order_cus)
                    {
                        BookingDAL.InsertOrderCus(item as JObject, trans, order_no, ref cus_seqno);
                    } // foreach
                } // if

                if (obj["order_lst"] != null)
                {
                    JArray order_lst = (JArray)obj["order_lst"];

                    foreach (var item in order_lst)
                    {
                        BookingDAL.InsertOrderLst(item as JObject, trans, order_no, cus_seqno, ref lst_seqno);

                        if (item["order_discount_rule_mst"] != null)
                        {
                            BookingDAL.InsertOrderDiscountRuleMst(item["order_discount_rule_mst"] as JObject, trans, order_no, lst_seqno);

                            if (item["order_discount_rule_mst"]["order_discount_rule_dtl"] != null)
                            {
                                BookingDAL.InsertOrderDiscountRuleDtl(item["order_discount_rule_mst"]["order_discount_rule_dtl"] as JObject, trans, order_no, lst_seqno);

                            } // if

                        } // if

                    } // foreach

                } // if


                trans.Commit();

                conn.Close();

                orderNo.result = "0000";
                orderNo.result_msg = "OK";
                orderNo.order_no = order_no;


            }
            catch (Exception ex)
            {
                // 如果還沒commit 回傳false
                if(!trans.IsCompleted){
                    trans.Rollback();
                    conn.Close();
                }

                orderNo.result = "10001";
                orderNo.result_msg = $"InsertOrder  Error :{ex.Message},{ex.StackTrace}";

                Website.Instance.logger.FatalFormat($"InsertOrder  Error :{ex.Message},{ex.StackTrace}");

                //throw ex;

            }

            //return currency;
            return orderNo;

        }



    }
}
