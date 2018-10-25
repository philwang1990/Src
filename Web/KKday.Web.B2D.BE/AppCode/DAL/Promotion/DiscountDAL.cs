using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.Discount;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.Promotion
{
    public class DiscountDAL
    {
        // 取得折扣主規則(不含明細)
        public static List<B2dDiscountMst> GetDiscountMst()
        { 
            List<B2dDiscountMst> mst_list = new List<B2dDiscountMst>();

            try
            { 
                string sqlStmt = @"SELECT A.company_xid, B.comp_name, C.*
FROM b2b.b2d_comp_disc_map A
JOIN b2b.b2d_company B ON A.company_xid=B.xid
JOIN b2b.b2d_discount_mst C ON A.disc_mst_xid=C.xid
ORDER BY comp_name
";

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    mst_list.Add(new B2dDiscountMst()
                    {
                        XID = dr.ToInt64("xid"),
                        DISC_NO = dr.ToStringEx("disc_no"),
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
         
        // 折扣明細(黑白名單)
        public static List<B2dDiscountDtl> GetDiscountDtl(Int64 mst_xid)
        {
            List<B2dDiscountDtl> dtl_lst = new List<B2dDiscountDtl>();

            try
            {
                string sqlStmt = @"SELECT B.*
FROM b2b.b2d_discount_mst A
JOIN b2b.b2d_discount_dtl B ON A.xid=B.mst_xid
WHERE A.xid=:mst_xid
";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("mst_xid", mst_xid)
                };

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dtl_lst.Add(new B2dDiscountDtl()
                    {
                        XID = dr.ToInt64("xid"),
                        DISC_TYPE = dr.ToStringEx("disc_type"),
                        DISC_LIST = dr.ToStringEx("disc_list"),
                        DISC_LIST_NAME = dr.ToStringEx("disc_list_name"),
                        WHITELIST = dr.ToStringEx("whitelist")
                    });
                }
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return dtl_lst;
        }

        // 折扣外幣加減價
        public static List<B2dDiscountCurrAmt> GetDiscountCurrAmt(Int64 mst_xid)
        {
            List<B2dDiscountCurrAmt> curr_amt_lst = new List<B2dDiscountCurrAmt>();

            try
            {
                string sqlStmt = @"SELECT B.xid, B.mst_xid, B.currency, B.amount
FROM b2b.b2d_discount_mst A
JOIN b2b.b2d_discount_curr_amt B ON A.xid=B.mst_xid
WHERE A.xid=:mst_xid
";
                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("mst_xid", mst_xid)
                };

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    curr_amt_lst.Add(new B2dDiscountCurrAmt()
                    {
                        XID = dr.ToInt64("xid"),
                        CURRENCY = dr.ToStringEx("currency"),
                        AMOUNT = dr.ToDouble("amount")
                    });
                }
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return curr_amt_lst;
        }

        //////////////////////////

        public static void InsertDiscountMst(B2dDiscountMst mst, string crt_user)
        {
            try
            {
                string sqlStmt = @"";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {

                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace); 
                throw ex;
            }
        }

        public static void InsertDiscountDtl(B2dDiscountDtl dtl, string crt_user)
        {
            try
            {
                string sqlStmt = @"";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {

                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void InsertDiscountCurrAmnt(B2dDiscountCurrAmt cur_amt, string crt_user)
        {
            try
            {
                string sqlStmt = @"";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {

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
