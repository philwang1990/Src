using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.DataModel.Booking;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.AppCode.DAL;
using System.Collections.Generic;
using Npgsql;


namespace KKday.API.WMS.Models.Repository.Booking
{
    public class BookingRepository
    {


        public static void InsertOrder()
        {



            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.Configuration["ConnectionStrings:NpgsqlConnection"]);
            conn.Open();
            NpgsqlTransaction trans = conn.BeginTransaction();
            String sql = null;
            NpgsqlParameter[] np = null;
            DateTime now = DateTime.Now;
            try
            {
                BookingDAL.InsertOrders(trans, sql, np, now);
                BookingDAL.InsertOrderSource(trans, sql, np, now);
                BookingDAL.InsertOrderLst(trans, sql, np, now);
                BookingDAL.InsertOrderCus(trans, sql, np, now);
                BookingDAL.InsertOrderDiscountRuleMst(trans, sql, np, now);
                BookingDAL.InsertOrderDiscountRuleDtl(trans, sql, np, now);

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
