using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.ListPrice;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.ListPrice
{
    public class BlacklistProdDAL
    {
        public static int GetBlacklistProdCount(string filter)
        {
            try
            {
                string sqlStmt = @"SELECT COUNT(*)
FROM b2b.b2d_list_price_blacks
WHERE 1=1 {FILTER}";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);

                int total_count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt));
                return total_count;
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 取得所有分銷商使用者列表　
        public static List<B2dBlacklistProduct> GetBlacklistProds(string filter, int skip, int size, string sorting)
        {
            List<B2dBlacklistProduct> prods = new List<B2dBlacklistProduct>();

            try
            {
                string sqlStmt = @"SELECT *
FROM b2b.b2d_list_price_blacks
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

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        prods.Add(new B2dBlacklistProduct()
                        {
                            XID = dr.ToInt64("xid"),
                            PROD_NO = dr.ToStringEx("prod_no"),
                            PROD_NAME = dr.ToStringEx("prod_name")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return prods;
        }

        public static void UpdateBlacklistProd(B2dBlacklistProduct prod, string upd_user)
        {
            try
            {
                string sqlStmt = @"UPDATE b2b.b2d_list_price_blacks SET prod_no=:PROD_NO,
 prod_name=:PROD_NAME, upd_user:UPD_USER, upd_datetime=now()
WHERE xid=:XID";
                 
                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]{
                    new NpgsqlParameter("XID", prod.XID),
                    new NpgsqlParameter("PROD_NO", prod.PROD_NO),
                    new NpgsqlParameter("PROD_NAME", prod.PROD_NAME),

                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);

            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void DeleteBlacklistProd(Int64 xid)
        {
            try
            {
                string sqlStmt = @"DELETE FROM b2b.b2d_list_price_blacks WHERE xid=:XID";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]{
                    new NpgsqlParameter("XID", xid) 
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);

            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}
