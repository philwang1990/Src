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
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
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
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return prods;
        }
        
        public static void InsertBlacklistProd(string prod_no, string prod_name, string crt_user)
        {
            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.SqlConnectionString);

            try
            {
                conn.Open();

                string sqlStmt = @"SELECT COUNT(*) FROM b2b.b2d_list_price_blacks WHERE prod_no=:prod_no";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]{
                    new NpgsqlParameter("prod_no", prod_no)
                };

                var count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(conn, CommandType.Text, sqlStmt, sqlParams));
                if(count > 0)
                {
                    throw new Exception($"{ prod_no } is duplicated");
                }

                sqlStmt = @"INSERT INTO b2b.b2d_list_price_blacks(prod_no, prod_name, crt_user, crt_datetime)
    VALUES (:prod_no, :prod_name, :crt_user, now())";

                sqlParams = new NpgsqlParameter[]{ 
                    new NpgsqlParameter("prod_no", prod_no),
                    new NpgsqlParameter("prod_name", prod_name),
                    new NpgsqlParameter("crt_user", crt_user) 
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);

                conn.Close();
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                conn.Close();
                throw ex;
            }
        }

        public static void UpdateBlacklistProd(Int64 xid, string prod_no, string prod_name, string upd_user)
        {
            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.SqlConnectionString);

            try
            {
                conn.Open();

                string sqlStmt = @"SELECT COUNT(*) FROM b2b.b2d_list_price_blacks WHERE xid <> :xid AND prod_no=:prod_no";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]{
                    new NpgsqlParameter("xid", xid),
                    new NpgsqlParameter("prod_no", prod_no)
                };

                var count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(conn, CommandType.Text, sqlStmt, sqlParams));
                if (count > 0)
                {
                    throw new Exception($"{ prod_no } is duplicated");
                }

                sqlStmt = @"UPDATE b2b.b2d_list_price_blacks SET prod_no=:prod_no, 
 prod_name=:prod_name, upd_user=:upd_user, upd_datetime=now()
WHERE xid=:xid";
                 
                 sqlParams = new NpgsqlParameter[]{
                    new NpgsqlParameter("xid", xid),
                    new NpgsqlParameter("prod_no", prod_no),
                    new NpgsqlParameter("prod_name", prod_name),
                    new NpgsqlParameter("upd_user", upd_user)
                };

                NpgsqlHelper.ExecuteNonQuery(conn, CommandType.Text, sqlStmt, sqlParams);

                conn.Close();
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                conn.Close();
                throw ex;
            }
        }

        public static void DeleteBlacklistProd(Int64 xid, string del_user)
        {
            try
            {
                string sqlStmt = @"DELETE FROM b2b.b2d_list_price_blacks WHERE xid=:xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[]{
                    new NpgsqlParameter("xid", xid) 
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }
    }
}
