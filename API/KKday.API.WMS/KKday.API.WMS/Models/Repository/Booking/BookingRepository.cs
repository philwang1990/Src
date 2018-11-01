using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.DataModel.Booking;
using KKday.API.WMS.AppCode.Proxy;
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
            String sql;
            NpgsqlParameter[] np;
            DateTime now = DateTime.Now;
            try
            {
                sql = @"INSERT INTO b2b.orders(
    order_no, kkday_order_oid, kkday_order_mid, order_date, order_type, order_status, order_amt, order_b2c_amt, connect_name, connect_tel, connect_mail, order_note)
    VALUES (:order_no, :kkday_order_oid, :kkday_order_mid, :order_date, :order_type, :order_status, :order_amt, :order_b2c_amt, :connect_name, :connect_tel, :connect_mail, :order_note); ";


                   np = new NpgsqlParameter[]{
                        new NpgsqlParameter("order_no","1"),
                        new NpgsqlParameter("kkday_order_oid","2"),
                        new NpgsqlParameter("kkday_order_mid","3"),
                        new NpgsqlParameter("order_date",DateTime.Now),
                        new NpgsqlParameter("order_type","5"),
                        new NpgsqlParameter("order_status",null),
                        new NpgsqlParameter("order_amt",7),
                        new NpgsqlParameter("order_b2c_amt",8),
                        new NpgsqlParameter("connect_name","9"),
                        new NpgsqlParameter("connect_tel","10"),
                        new NpgsqlParameter("connect_mail","11"),
                        new NpgsqlParameter("order_note","12")
                    };

                NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);
                // The number of rows affected if known; -1 otherwise.
                //if (NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np) > 0 )
                //{
                    //trans.Commit();
                //}

                sql = @"INSERT INTO b2b.order_sources(
    order_no, booking_type, company_xid, channel_oid, connect_name, connect_tel, connect_mail, order_note, client_ip, source_pk1, source_pk2, source_pk3, source_pk4, crt_datetime)
    VALUES (:order_no, :booking_type, :company_xid, :channel_oid, :connect_name, :connect_tel, :connect_mail, :order_note, :client_ip, :source_pk1, :source_pk2, :source_pk3, :source_pk4, :crt_datetime); ";


                np = new NpgsqlParameter[]{
                     new NpgsqlParameter("order_no","1"),
                     new NpgsqlParameter("booking_type","5"),
                     new NpgsqlParameter("company_xid",6),
                     new NpgsqlParameter("channel_oid",7),
                     new NpgsqlParameter("connect_name","9"),
                     new NpgsqlParameter("connect_tel","10"),
                     new NpgsqlParameter("connect_mail","11"),
                     new NpgsqlParameter("order_note","12"),
                     new NpgsqlParameter("client_ip","ip"),
                     new NpgsqlParameter("source_pk1","0"),
                     new NpgsqlParameter("source_pk2","0"),
                     new NpgsqlParameter("source_pk3","0"),
                     new NpgsqlParameter("source_pk4","0"),
                     new NpgsqlParameter("crt_datetime",DateTime.Now )
                    };

                NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);

                trans.Commit();



                conn.Close();



            }
            catch (Exception ex)
            {
                trans.Rollback();
                Website.Instance.logger.FatalFormat($"getCurrency  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }

            //return currency;

        }

      
    }
}
