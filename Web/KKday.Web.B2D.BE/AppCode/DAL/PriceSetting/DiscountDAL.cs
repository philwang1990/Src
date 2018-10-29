using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.PriceSetting;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.PriceSetting
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

        //////////////////////////

        public static void InsertDiscountMst(B2dDiscountMst mst, string crt_user)
        {
            try
            {
                string sqlStmt = @"INSERT INTO b2b.b2d_discount_mst(disc_no, disc_name, 
 disc_type, s_date, e_date, status, disc_percent, rule_status, crt_user, crt_datetime)
VALUES (:disc_no, :disc_name, :disc_type, :s_date, :e_date, :status, :disc_percent, :rule_status, :crt_user, now())
";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("disc_no,", mst.DISC_NO),
                    new NpgsqlParameter("disc_name,", mst.DISC_NAME),
                    new NpgsqlParameter("disc_type,", mst.DISC_TYPE),
                    new NpgsqlParameter("s_date,", mst.S_DATE),
                    new NpgsqlParameter("e_date,", mst.E_DATE),
                    new NpgsqlParameter("status,", mst.STATUS),
                    new NpgsqlParameter("disc_percent,", mst.DISC_PERCENT),
                    new NpgsqlParameter("rule_status,", mst.RULE_STATUS),
                    new NpgsqlParameter("crt_user", crt_user)
                };
              

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void UpdateDiscountMst(B2dDiscountMst mst, string upd_user)
        {
            try
            {
                string sqlStmt = @" UPDATE b2b.b2d_discount_mst SET disc_no=:disc_no, disc_name=:disc_name, 
 disc_type=:disc_type, s_date=:s_date, e_date=:e_date, status=:status, disc_percent=:disc_percent, 
 rule_status=:rule_status, upd_user=:upd_user, upd_datetime=now()
WHERE xid=:xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] { 
                    new NpgsqlParameter("disc_no,", mst.DISC_NO),
                    new NpgsqlParameter("disc_name,", mst.DISC_NAME),
                    new NpgsqlParameter("disc_type,", mst.DISC_TYPE),
                    new NpgsqlParameter("s_date,", mst.S_DATE),
                    new NpgsqlParameter("e_date,", mst.E_DATE),
                    new NpgsqlParameter("status,", mst.STATUS),
                    new NpgsqlParameter("disc_percent,", mst.DISC_PERCENT),
                    new NpgsqlParameter("rule_status,", mst.RULE_STATUS),
                    new NpgsqlParameter("xid,", mst.XID),
                    new NpgsqlParameter("upd_user", upd_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void DeleteDiscountMst(Int64 mst_xid, string del_user)
        {
            NpgsqlConnection conn = new NpgsqlConnection();
            NpgsqlTransaction trans = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                DeleteDiscountDtl(trans, mst_xid, del_user);
                DeleteDiscountCurrAmnt(trans, mst_xid, del_user);

                string sqlStmt = @"DELETE FROM b2b.b2d_discount_mst WHERE xid=:xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid,", mst_xid)
                };

                NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlStmt, sqlParams);

                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                if(trans != null) trans.Rollback();
                conn.Close();

                throw ex;
            }
        }

        #endregion DiscountMst Methods 

        ////////////////////////////

        #region DiscountDtl Methods (黑白名單)

        public static int GetDiscountDtlCount(Int64 mst_xid, string filter)
        {
            try
            {
                string sqlStmt = @"SELECT COUNT(*) FROM b2b.b2d_company
WHERE 1=1 {FILTER}";


                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("mst_xid", mst_xid)
                };

                int total_count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams));
                return total_count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 折扣明細清單
        public static List<B2dDiscountDtl> GetDiscountDtls(Int64 mst_xid, string filter, int skip, int size, string sorting)
        {
            List<B2dDiscountDtl> dtl_lst = new List<B2dDiscountDtl>();

            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_discount_dtl
WHERE mst_xid=:mst_xid {FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);
                sqlStmt = sqlStmt.Replace("{SORTING}", !string.IsNullOrEmpty(sorting) ? "ORDER BY " + sorting : string.Empty);

                List<NpgsqlParameter> sqlParams = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("Size", size),
                    new NpgsqlParameter("Skip", skip), 
                    new NpgsqlParameter("mst_xid", mst_xid)
                };

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams.ToArray());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dtl_lst.Add(new B2dDiscountDtl()
                    {
                        XID = dr.ToInt64("xid"),
                        MST_XID = dr.ToInt64("mst_xid"),
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

        //////////////////////////

        public static void InsertDiscountDtl(B2dDiscountDtl dtl, string crt_user)
        {
            try
            {
                string sqlStmt = @"INSERT INTO b2b.b2d_discount_dtl(mst_xid, disc_type, 
 disc_list, disc_list_name, whitelist, crt_user, crt_datetime)
VALUES (:mst_xid, :disc_type, :disc_list, :disc_list_name, :whitelist, :crt_user, now())";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("mst_xid,", dtl.MST_XID),
                    new NpgsqlParameter("disc_type",dtl.DISC_TYPE), 
                    new NpgsqlParameter("disc_list", dtl.DISC_LIST), 
                    new NpgsqlParameter("disc_list_name", dtl.DISC_LIST_NAME), 
                    new NpgsqlParameter("whitelist", dtl.WHITELIST), 
                    new NpgsqlParameter("crt_user", crt_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void UpdateDiscountDtl(B2dDiscountDtl dtl, string upd_user)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_discount_dtl SET disc_type=:disc_type, 
 disc_list=:disc_list, disc_list_name=:disc_list_name, whitelist=:whitelist, upd_user=:upd_user, upd_datetime=now()
WHERE xid=:xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid,", dtl.XID),
                    new NpgsqlParameter("disc_type",dtl.DISC_TYPE),
                    new NpgsqlParameter("disc_list", dtl.DISC_LIST),
                    new NpgsqlParameter("disc_list_name", dtl.DISC_LIST_NAME),
                    new NpgsqlParameter("whitelist", dtl.WHITELIST),
                    new NpgsqlParameter("upd_user", upd_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void DeleteDiscountDtl(NpgsqlTransaction trans, Int64 mst_xid, string crt_user)
        {
            try
            {
                string sqlStmt = @"DELETE FROM b2b.b2d_discount_dtl WHERE mst_xid=:mst_xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("mst_xid", mst_xid)
                };

                NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }


        public static void DeleteDiscountDtl(Int64 xid, string crt_user)
        {
            try
            {
                string sqlStmt = @"DELETE FROM b2b.b2d_discount_dtl WHERE xid=:xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", xid)
                };
                 
                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        #endregion DiscountDtl Methods 

        ////////////////////////////

        #region DiscountCurrAmt Methods 

        public static int GetDiscountCurrAmtCount(Int64 mst_xid, string filter)
        {
            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_discount_curr_amt
WHERE mst_xid=:mst_xid {FILTER}";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("mst_xid", mst_xid)
                };

                int total_count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams));
                return total_count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 折扣外幣加減價
        public static List<B2dDiscountCurrAmt> GetDiscountCurrAmts(Int64 mst_xid, 
               string filter, int skip, int size, string sorting)
        {
            List<B2dDiscountCurrAmt> curr_amt_lst = new List<B2dDiscountCurrAmt>();

            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_discount_curr_amt
WHERE mst_xid=:mst_xid {FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);
                sqlStmt = sqlStmt.Replace("{SORTING}", !string.IsNullOrEmpty(sorting) ? "ORDER BY " + sorting : string.Empty);

                List<NpgsqlParameter> sqlParams = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("Size", size),
                    new NpgsqlParameter("Skip", skip),
                    new NpgsqlParameter("mst_xid", mst_xid)
                };

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams.ToArray());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    curr_amt_lst.Add(new B2dDiscountCurrAmt()
                    {
                        XID = dr.ToInt64("xid"),
                        MST_XID = dr.ToInt64("mst_xid"),
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

        public static void InsertDiscountCurrAmnt(B2dDiscountCurrAmt cur_amt, string crt_user)
        {
            try
            {
                string sqlStmt = @"INSERT INTO b2b.b2d_discount_curr_amt(mst_xid, currency, amount, crt_user, crt_datetime)
        VALUES (:mst_xid, :currency, :amount, :crt_user, now())";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("mst_xid", cur_amt.MST_XID),
                    new NpgsqlParameter("currency", cur_amt.CURRENCY),
                    new NpgsqlParameter("amount", cur_amt.AMOUNT),
                    new NpgsqlParameter("crt_user", crt_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void UpdateDiscountCurrAmnt(B2dDiscountCurrAmt cur_amt, string upd_user)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_discount_curr_amt SET currency=:currency,
 amount=:amount, upd_user=:upd_user, upd_datetime=now()
WHERE xid=:xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", cur_amt.XID),
                    new NpgsqlParameter("currency", cur_amt.CURRENCY),
                    new NpgsqlParameter("amount", cur_amt.AMOUNT),
                    new NpgsqlParameter("upd_user", upd_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void DeleteDiscountCurrAmnt(NpgsqlTransaction trans, Int64 mst_xid, string del_user)
        {
            try
            {
                string sqlStmt = @"DELETE FROM b2b.b2d_discount_curr_amt WHERE mst_xid=:mst_xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("mst_xid", mst_xid)
                };

                NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void DeleteDiscountCurrAmnt(Int64 xid, string del_user)
        {
            try
            {
                string sqlStmt = @"DELETE FROM b2b.b2d_discount_curr_amt WHERE xid=:xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", xid)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        #endregion DiscountCurrAmt Methods 

        //////////////////////////
    }
}
