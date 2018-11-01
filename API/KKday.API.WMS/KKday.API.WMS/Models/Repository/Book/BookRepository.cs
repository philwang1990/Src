using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.DataModel.Book;
using KKday.API.WMS.AppCode.Proxy;
using System.Collections.Generic;
using Npgsql;

namespace KKday.API.WMS.Models.Repository.Book
{
    public class BookRepository
    {


        public static void InsertOrder()
        {

            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.Configuration["ConnectionStrings:NpgsqlConnection"]);
            conn.Open();
            NpgsqlTransaction trans = conn.BeginTransaction();
            try
            {
                String sql = @"INSERT INTO b2b.orders(
    order_no, kkday_order_oid, kkday_order_mid, order_date, order_type, order_status, order_amt, order_b2c_amt, connect_name, connect_tel, connect_mail, order_note)
    VALUES (:order_no, :kkday_order_oid, :kkday_order_mid, :order_date, :order_type, :order_status, :order_amt, :order_b2c_amt, :connect_name, :connect_tel, :connect_mail, :order_note); ";


                NpgsqlParameter[] np = new NpgsqlParameter[]{
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


                // The number of rows affected if known; -1 otherwise.
                if (NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np) > 0 )
                {
                    trans.Commit();
                }

                conn.Close();



            }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat($"getCurrency  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }

            //return currency;

        }

      
    }
}
