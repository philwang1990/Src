using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.PriceSetting;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.Company
{
    public class CompanyDiscountDAL
    {
        public static List<B2dDiscountMst> GetDiscountMst(Int64 company_xid)
        {
            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.SqlConnectionString);
            List<B2dDiscountMst> mst_list = new List<B2dDiscountMst>();

            try
            {
                conn.Open();

                string sqlStmt = @"SELECT A.company_xid, B.comp_name, C.*
FROM b2b.b2d_comp_disc_map A
JOIN b2b.b2d_company B ON A.company_xid=B.xid
JOIN b2b.b2d_discount_mst C ON A.disc_mst_xid=C.xid AND C.status='01'
WHERE B.xid=:company_xid
ORDER BY comp_name
";
                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("company_xid", company_xid)
                };

                var ds = NpgsqlHelper.ExecuteDataset(conn, CommandType.Text, sqlStmt, sqlParams);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mst_list.Add(new B2dDiscountMst()
                    {
                        XID = dr.ToInt64("xid"),
                        DISC_NAME = dr.ToStringEx("disc_name"),
                        DISC_PERCENT = dr.ToDouble("disc_percent"),
                        DISC_TYPE = dr.ToStringEx("disc_type"),
                        S_DATE = dr.ToDateTimeEx("s_date"),
                        E_DATE = dr.ToDateTimeEx("e_date"),
                        RULE_STATUS = dr.ToStringEx("rule_status")
                    });
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                conn.Close();
                throw ex;
            }

            return mst_list;
        }

        public static List<B2dDiscountMst> GetAvailableDiscountMst(Int64 company_xid)
        {
            List<B2dDiscountMst> mst_list = new List<B2dDiscountMst>();

            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_discount_mst 
WHERE xid NOT IN (
    SELECT DISTINCT A.disc_mst_xid
    FROM b2b.b2d_comp_disc_map A
    JOIN b2b.b2d_discount_mst B ON A.disc_mst_xid=B.xid AND B.status='01'
    WHERE A.company_xid=:company_xid
) AND status='01'
";
                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("company_xid", company_xid)
                };

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mst_list.Add(new B2dDiscountMst()
                    {
                        XID = dr.ToInt64("xid"),
                        DISC_NAME = dr.ToStringEx("disc_name"),
                        DISC_PERCENT = dr.ToDouble("disc_percent"),
                        DISC_TYPE = dr.ToStringEx("disc_type"),
                        S_DATE = dr.ToDateTimeEx("s_date"),
                        E_DATE = dr.ToDateTimeEx("e_date"),
                        RULE_STATUS = dr.ToStringEx("rule_status")
                    });
                }

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return mst_list;
        }

        public static void InsertDiscount(Int64 company_xid, Int64[] items, string crt_user)
        {
            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.SqlConnectionString);
            NpgsqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                foreach (var disc_mst_xid in items)
                {
                    string sqlStmt = @"INSERT INTO b2b.b2d_comp_disc_map (company_xid, 
 disc_mst_xid, crt_datetime, crt_user)
VALUES (:company_xid, :disc_mst_xid, now(), :crt_user)";

                    NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                        new NpgsqlParameter("company_xid", company_xid),
                        new NpgsqlParameter("disc_mst_xid", disc_mst_xid),
                        new NpgsqlParameter("crt_user", crt_user)
                    };

                    NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlStmt, sqlParams);
                }

                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                if (trans != null) trans.Rollback();
                conn.Close();

                throw ex;
            }
        }

        public static void RemoveDiscount(Int64 company_xid, Int64 disc_mst_xid, string del_user)
        {
            try
            {
                string sqlStmt = @"DELETE FROM b2b.b2d_comp_disc_map WHERE company_xid=:company_xid AND disc_mst_xid=:disc_mst_xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                        new NpgsqlParameter("company_xid", company_xid),
                        new NpgsqlParameter("disc_mst_xid", disc_mst_xid)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}
