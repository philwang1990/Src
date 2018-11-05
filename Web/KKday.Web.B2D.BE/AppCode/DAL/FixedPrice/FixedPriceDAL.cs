using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.FixedPrice;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.FixedPrice
{
    public class FixedPriceDAL
    {
        public static int GetFixedPriceProdCount(Int64 comp_xid, string filter)
        {
            try
            {
                string sqlStmt = @"SELECT COUNT(*) FROM b2b.b2d_fixedprice_prod
WHERE company_xid=:company_xid {FILTER}";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("company_xid", comp_xid)
                };

                int total_count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams));
                return total_count;
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 取得所有分銷商使用者列表　
        public static List<FixedProduct> GetBlacklistProds(Int64 comp_xid, string filter, int skip, int size, string sorting)
        {
            List<FixedProduct> prods = new List<FixedProduct>();

            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_fixedprice_prod
WHERE company_xid=:company_xid {FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);
                sqlStmt = sqlStmt.Replace("{SORTING}", !string.IsNullOrEmpty(sorting) ? "ORDER BY " + sorting : string.Empty);

                List<NpgsqlParameter> sqlParams = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("company_xid", comp_xid),
                    new NpgsqlParameter("Size", size),
                    new NpgsqlParameter("Skip", skip)
                };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams.ToArray());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        prods.Add(new FixedProduct()
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
    }
}
