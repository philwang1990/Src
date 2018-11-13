using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using KKday.API.WMS.Models.DataModel.Booking;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.AppCode.DAL
{
    public class BookingDAL
    {

        public static void InitialSeqs(){
            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.Configuration["ConnectionStrings:NpgsqlConnection"]);
            conn.Open();
            NpgsqlTransaction trans = conn.BeginTransaction();
            String sql = null;
            NpgsqlParameter[] np = null;

            sql = @"SELECT setval('b2b.b2d_order_cus_seq', 0, true),setval('b2b.b2d_order_lst_seq', 0, true); ";

            NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);

            trans.Commit();

            conn.Close();

        }

        public static int InsertOrders(JObject obj, NpgsqlTransaction trans,ref String order_no)
        {

            String sql = null;
            NpgsqlParameter[] np = null;

            sql = @"select  replace('KOD'|| to_char(Nextval('b2b.b2d_order_no_seq') ,'9999999999999'),' ','0') order_no;";
            DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql, np);
            //ds.AcceptChanges();
            order_no = ds.Tables[0].Rows[0]["order_no"].ToString() ;

            sql = @"INSERT INTO b2b.orders(
    order_no, kkday_order_oid, kkday_order_mid, order_date, order_type, order_status, order_amt, order_b2c_amt, connect_name, connect_tel, connect_mail, order_note)
    VALUES (:order_no, :kkday_order_oid, :kkday_order_mid, to_timestamp(:order_date,'MM/DD/YYYY HH:mi:SS' ), :order_type, :order_status, :order_amt, :order_b2c_amt, :connect_name, :connect_tel, :connect_mail, :order_note); ";


            np = new NpgsqlParameter[]{
                     new NpgsqlParameter("order_no",order_no),
                     new NpgsqlParameter("kkday_order_oid",obj["kkday_order_oid"].ToString()),
                     new NpgsqlParameter("kkday_order_mid",obj["kkday_order_mid"].ToString()),
                     new NpgsqlParameter("order_date",obj["order_date"].ToString()), // date : 10/26/2018 04:09:21
                     new NpgsqlParameter("order_type",obj["order_type"].ToString()),
                     new NpgsqlParameter("order_status",obj["order_status"].ToString()),
                     new NpgsqlParameter("order_amt",(int)obj["order_amt"]),
                     new NpgsqlParameter("order_b2c_amt",(int)obj["order_b2c_amt"]),
                     new NpgsqlParameter("connect_name",obj["connect_name"].ToString()),
                     new NpgsqlParameter("connect_tel",obj["connect_tel"].ToString()),
                     new NpgsqlParameter("connect_mail",obj["connect_mail"].ToString()),
                     new NpgsqlParameter("order_note",obj["order_note"].ToString())
                    };

            return NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);
            // The number of rows affected if known; -1 otherwise.
            //if (NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np) > 0 )
            //{
            //trans.Commit();
            //}

        }

        public static int InsertOrderSource(JObject obj, NpgsqlTransaction trans, String order_no)
        {

            String sql = null;
            NpgsqlParameter[] np = null;


            sql = @"INSERT INTO b2b.order_source(
    order_no, booking_type, company_xid, channel_oid, connect_name, connect_tel, connect_mail, order_note, client_ip, source_pk1, source_pk2, source_pk3, source_pk4, crt_datetime)
    VALUES (:order_no, :booking_type, :company_xid, :channel_oid, :connect_name, :connect_tel, :connect_mail, :order_note, :client_ip, :source_pk1, :source_pk2, :source_pk3, :source_pk4, :crt_datetime); ";


            np = new NpgsqlParameter[]{
                     new NpgsqlParameter("order_no",order_no),
                     new NpgsqlParameter("booking_type",obj["booking_type"].ToString()),
                     new NpgsqlParameter("company_xid",(int)obj["company_xid"]),
                     new NpgsqlParameter("channel_oid",(int)obj["channel_oid"]),
                     new NpgsqlParameter("connect_name",obj["connect_name"].ToString()),
                     new NpgsqlParameter("connect_tel",obj["connect_tel"].ToString()),
                     new NpgsqlParameter("connect_mail",obj["connect_mail"].ToString()),
                     new NpgsqlParameter("order_note",obj["order_note"].ToString()),
                     new NpgsqlParameter("client_ip",obj["client_ip"].ToString()),
                     new NpgsqlParameter("source_pk1",obj["source_pk1"].ToString()),
                     new NpgsqlParameter("source_pk2",obj["source_pk2"].ToString()),
                     new NpgsqlParameter("source_pk3",obj["source_pk3"].ToString()),
                     new NpgsqlParameter("source_pk4",obj["source_pk4"].ToString()),
                     new NpgsqlParameter("crt_datetime",DateTime.Now)
                    };

            return NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);

        }

        public static int InsertOrderCus(JObject obj, NpgsqlTransaction trans, String order_no, ref List<int> cus_seqno)
        {

            String sql = null;
            NpgsqlParameter[] np = null;

            sql = @"select  replace( to_char(Nextval('b2b.b2d_order_cus_seq') ,'9999999999999'),' ','') cus_seqno;";
            DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql, np);
            //ds.AcceptChanges();
            cus_seqno.Add(Convert.ToInt32(ds.Tables[0].Rows[0]["cus_seqno"]));


            sql = @"INSERT INTO b2b.order_cus(
    order_no, cus_seqno, cus_type, cus_name_e_last, cus_name_e_first, cus_sex, cus_tel, cus_mail)
    VALUES (:order_no, :cus_seqno, :cus_type, :cus_name_e_last, :cus_name_e_first, :cus_sex, :cus_tel, :cus_mail); ";


            np = new NpgsqlParameter[]{
                     new NpgsqlParameter("order_no",order_no),
                     new NpgsqlParameter("cus_seqno",cus_seqno[cus_seqno.Count-1]),
                     //new NpgsqlParameter("lst_seqno",2),
                     new NpgsqlParameter("cus_type",obj["cus_type"].ToString()),
                     new NpgsqlParameter("cus_name_e_last",obj["cus_name_e_last"].ToString()),
                     new NpgsqlParameter("cus_name_e_first",obj["cus_name_e_first"].ToString()),
                     new NpgsqlParameter("cus_sex",obj["cus_sex"].ToString()),
                     new NpgsqlParameter("cus_tel",obj["cus_tel"].ToString()),
                     new NpgsqlParameter("cus_mail",obj["cus_mail"].ToString())
                    };

            return NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);

        }

        public static int InsertOrderLst(JObject obj, NpgsqlTransaction trans, String order_no, List<int> cus_seqno, ref List<int> lst_seqno)
        {

            String sql = null;
            NpgsqlParameter[] np = null;

            sql = @"select  replace( to_char(Nextval('b2b.b2d_order_lst_seq') ,'9999999999999'),' ','') lst_seqno;";
            DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql, np);
            //ds.AcceptChanges();
            lst_seqno.Add(Convert.ToInt32(ds.Tables[0].Rows[0]["lst_seqno"]));
            int cus_seq = 0;

            // cus_seqno 只有一筆 那cus_seq 就只帶那筆 
            if( cus_seqno.Count == 1 ) 
            {
                cus_seq = cus_seqno[0];
            } // if
            // cus_seqno 大於一筆 就要對位置塞入
            else if ( cus_seqno.Count > 1 )
            {
                cus_seq = cus_seqno[lst_seqno.Count - 1];
            } // else if


            sql = @"INSERT INTO b2b.order_lst(
    order_no, lst_seqno, cus_seqno, prod_no, prod_name, prod_amt, prod_b2c_amt, prod_currency, discount_xid, prod_cond1, prod_cond2, pkg_no, pkg_name, pkg_date, op_status, sc_status, fa_status)
    VALUES (:order_no, :lst_seqno, :cus_seqno, :prod_no, :prod_name, :prod_amt, :prod_b2c_amt, :prod_currency, :discount_xid, :prod_cond1, :prod_cond2, :pkg_no, :pkg_name, :pkg_date, :op_status, :sc_status, :fa_status); ";


            np = new NpgsqlParameter[]{
                     new NpgsqlParameter("order_no",order_no),
                     new NpgsqlParameter("lst_seqno",lst_seqno[lst_seqno.Count-1]),
                     new NpgsqlParameter("cus_seqno",cus_seq),
                     new NpgsqlParameter("prod_no",obj["prod_no"].ToString()),
                     new NpgsqlParameter("prod_name",obj["prod_name"].ToString()),
                     new NpgsqlParameter("prod_amt",(int)obj["prod_amt"]),
                     new NpgsqlParameter("prod_b2c_amt",(int)obj["prod_b2c_amt"]),
                     new NpgsqlParameter("prod_currency",obj["prod_currency"].ToString()),
                     new NpgsqlParameter("discount_xid",(int)obj["discount_xid"]),
                     new NpgsqlParameter("prod_cond1",obj["prod_cond1"].ToString()),
                     new NpgsqlParameter("prod_cond2",obj["prod_cond2"].ToString()),
                     new NpgsqlParameter("pkg_no",obj["pkg_no"].ToString()),
                     new NpgsqlParameter("pkg_name",obj["pkg_name"].ToString()),
                     new NpgsqlParameter("pkg_date",obj["pkg_date"].ToString()),
                     new NpgsqlParameter("op_status",obj["op_status"].ToString()),
                     new NpgsqlParameter("sc_status",obj["sc_status"].ToString()),
                     new NpgsqlParameter("fa_status",obj["fa_status"].ToString())
                    };

            return NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);
        }

        public static int InsertOrderDiscountRuleMst(JObject obj, NpgsqlTransaction trans, String order_no, List<int> lst_seqno)
        {
            String sql = null;
            NpgsqlParameter[] np = null;
            sql = @"INSERT INTO b2b.order_discount_rule_mst(
    xid, lst_seqno, disc_name, disc_amt, disc_currency, disc_note, order_no)
    VALUES (:xid, :lst_seqno, :disc_name, :disc_amt, :disc_currency, :disc_note, :order_no); ";


            np = new NpgsqlParameter[]{
                     new NpgsqlParameter("xid",(int)obj["xid"]),
                     new NpgsqlParameter("lst_seqno",lst_seqno[lst_seqno.Count-1]),
                     new NpgsqlParameter("disc_name",obj["disc_name"].ToString()),
                     new NpgsqlParameter("disc_amt",(int)obj["disc_amt"]),
                     new NpgsqlParameter("disc_currency",obj["disc_currency"].ToString()),
                     new NpgsqlParameter("disc_note",obj["disc_note"].ToString()),
                     new NpgsqlParameter("order_no",order_no)
                    };

            return NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);

        }

        public static int InsertOrderDiscountRuleDtl(JObject obj, NpgsqlTransaction trans, String order_no, List<int> lst_seqno)
        {

            String sql = null;
            NpgsqlParameter[] np = null;
            sql = @"INSERT INTO b2b.order_discount_rule_dtl(
    xid, mst_xid, lst_seqno, order_no)
    VALUES (:xid, :mst_xid, :lst_seqno, :order_no); ";


            np = new NpgsqlParameter[]{
                     new NpgsqlParameter("xid",(int)obj["xid"]),
                     new NpgsqlParameter("mst_xid",(int)obj["mst_xid"]),
                     new NpgsqlParameter("lst_seqno",lst_seqno[lst_seqno.Count-1]),
                     new NpgsqlParameter("order_no",order_no)
                    };

            return NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);

        }

        public static int UpdateOrder(UpdateOrderModel model)
        {
            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.Configuration["ConnectionStrings:NpgsqlConnection"]);
            conn.Open();
            NpgsqlTransaction trans = conn.BeginTransaction();
            String sql = null;
            NpgsqlParameter[] np = null;
            int count = 0;

            //sql = @"UPDATE b2b.orders 
            //        SET kkday_order_oid = :kkday_order_oid, kkday_order_mid= :kkday_order_mid
            //        FROM b2b.orders a LEFT JOIN b2b.order_source b on a.order_no = b.order_no
            //        WHERE 1=1
            //        AND a.order_no = :order_no 
            //        AND b.order_no = :order_no2 
            //        AND b.company_xid = :company_xid ; ";



            //np = new NpgsqlParameter[]{
            // new NpgsqlParameter("kkday_order_oid",model.order_oid),
            // new NpgsqlParameter("kkday_order_mid",model.order_mid),
            // new NpgsqlParameter("order_no2",model.order_no),
            // new NpgsqlParameter("order_no",model.order_no),
            // new NpgsqlParameter("company_xid",model.company_xid)

            //};

            sql = @"UPDATE b2b.orders 
                    SET kkday_order_oid = :kkday_order_oid, kkday_order_mid= :kkday_order_mid
                    FROM b2b.orders a LEFT JOIN b2b.order_source b on a.order_no = b.order_no
                    WHERE 1=1
                    AND a.order_no = :order_no 
                    AND b.order_no = :order_no2 
                    AND b.company_xid = :company_xid ; ";



            np = new NpgsqlParameter[]{
                     new NpgsqlParameter("kkday_order_oid",model.order_oid),
                     new NpgsqlParameter("kkday_order_mid",model.order_mid),
                     new NpgsqlParameter("order_no2",model.order_no),
                     new NpgsqlParameter("order_no",model.order_no),
                     new NpgsqlParameter("company_xid",model.company_xid)

                    };

            count = NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, np);

            trans.Commit();

            conn.Close();

            return count;

        }
    }
}