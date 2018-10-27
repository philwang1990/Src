using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.Promotion;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.Promotion
{
    public class DiscountDAL
    {
        #region DiscountMst Methods 

        // 取得折扣主規則總筆數
        public static int GetDiscountMstCount(string filter)
        { 
            try
            {
                string sqlStmt = @"SELECT COUNT(*) FROM b2b.b2d_discount_mst
WHERE 1=1 {FILTER}";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);

                var total_count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt));
                return total_count;

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 取得折扣主規則
        public static List<B2dDiscountMst> GetDiscountMsts(string filter, int skip, int size, string sorting)
        { 
            List<B2dDiscountMst> mst_list = new List<B2dDiscountMst>();

            try
            { 
                string sqlStmt = @"SELECT * FROM b2b.b2d_discount_mst
WHERE 1=1 {FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);
                sqlStmt = sqlStmt.Replace("{SORTING}", !string.IsNullOrEmpty(sorting) ? "ORDER BY " + sorting : string.Empty);

                List<NpgsqlParameter> sqlParams = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("Size", size),
                    new NpgsqlParameter("Skip", skip)
                };

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams.ToArray());
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
                        RULE_STATUS = dr.ToStringEx("rule_status"),
                        STATUS = dr.ToStringEx("status"),
                        CRT_USER = dr.ToStringEx("crt_user"),
                        CRT_DATETIME = dr.ToDateTime("crt_datetime")
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

        // 取得折扣主規則
        public static B2dDiscountMst GetDiscountMst(Int64 xid)
        {
            List<B2dDiscountMst> mst_list = new List<B2dDiscountMst>();

            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_discount_mst WHERE C.xid=:xid";
                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", xid)
                };

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
                DataRow dr = ds.Tables[0].Rows[0];
                var mst = new B2dDiscountMst()
                {
                    XID = dr.ToInt64("xid"),
                    DISC_NO = dr.ToStringEx("disc_no"),
                    DISC_NAME = dr.ToStringEx("disc_name"),
                    DISC_PERCENT = dr.ToDouble("disc_percent"),
                    DISC_TYPE = dr.ToStringEx("disc_type"),
                    S_DATE = dr.ToDateTimeEx("s_date"),
                    E_DATE = dr.ToDateTimeEx("e_date"),
                    RULE_STATUS = dr.ToStringEx("rule_status")
                };

                return mst;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
             
        }

        #endregion DiscountMst Methods 

        ////////////////////////////

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
