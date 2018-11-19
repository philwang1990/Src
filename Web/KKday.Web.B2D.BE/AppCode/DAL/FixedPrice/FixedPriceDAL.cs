﻿using System;
using System.Collections.Generic;
using System.Data;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.FixedPrice;
using Npgsql;

namespace KKday.Web.B2D.BE.AppCode.DAL.FixedPrice
{
    public class FixedPriceDAL
    {
        #region 固定價商品 Methods

        public static int GetFixedPriceProdCount(Int64 comp_xid, string filter)
        {
            try
            {
                string sqlStmt = @"SELECT COUNT(*)
FROM b2b.b2d_fixedprice_prod a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE a.company_xid=:company_xid {FILTER}";

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

        // 取得分銷商所有固定價產品
        public static List<FixedPriceProductEx> GetFixedPriceProds(Int64 comp_xid, string filter, int skip, int size, string sorting)
        {
            List<FixedPriceProductEx> prods = new List<FixedPriceProductEx>();

            try
            {
                string sqlStmt = @"SELECT a.*, b.comp_currency, b.comp_name
FROM b2b.b2d_fixedprice_prod a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE a.company_xid=:company_xid {FILTER}
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
                        prods.Add(new FixedPriceProductEx()
                        {
                            XID = dr.ToInt64("xid"),
                            PROD_NO = dr.ToStringEx("prod_no"),
                            PROD_NAME = dr.ToStringEx("prod_name"),
                            STATE = dr.ToStringEx("state"),
                            CURRENCY = dr.ToStringEx("comp_currency"),
                            COMPANY_NAME = dr.ToStringEx("comp_name"),
                            COMPANY_XID = dr.ToInt64("company_xid")
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

        // 取得分銷商所有固定價產品
        public static FixedPriceProductEx GetFixedPriceProd(Int64 xid)
        {
            try
            {
                FixedPriceProductEx _prod = null;

                string sqlStmt = @"SELECT a.*, b.comp_currency
FROM b2b.b2d_fixedprice_prod a
JOIN b2b.b2d_company b ON a.company_xid=b.xid
WHERE a.xid=:xid ";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("xid", xid)
                };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    _prod = new FixedPriceProductEx()
                    {
                        XID = dr.ToInt64("xid"),
                        PROD_NO = dr.ToStringEx("prod_no"),
                        PROD_NAME = dr.ToStringEx("prod_name"),
                        STATE = dr.ToStringEx("state"),
                        CURRENCY = dr.ToStringEx("comp_currency"),
                        COMPANY_XID = dr.ToInt64("company_xid")
                    };
                }

                return _prod;
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void InsertFixedPriceProduct(FixedPriceProduct fp_prod, string crt_user)
        {
            try
            {
                string sqlStmt = @"INSERT INTO b2b.b2d_fixedprice_prod(company_xid, prod_no, prod_name, state, crt_user, crt_datetime)
VALUES (:company_xid, :prod_no, :prod_name, :state, :crt_user, now())";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("company_xid", fp_prod.COMPANY_XID),
                    new NpgsqlParameter("prod_no", fp_prod.PROD_NO),
                    new NpgsqlParameter("prod_name", fp_prod.PROD_NAME),
                    new NpgsqlParameter("state", fp_prod.STATE),
                    new NpgsqlParameter("crt_user", crt_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public static void UpdateFixedPriceProduct(FixedPriceProduct fp_prod, string upd_user)
        {
            try
            {
                string sqlStmt = @"INSERT INTO b2b.b2d_fixedprice_prod(company_xid, prod_no, prod_name, upd_user, upd_datetime)
VALUES (:company_xid, :prod_no, :prod_name, :upd_user, now())";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("company_xid", fp_prod.COMPANY_XID),
                    new NpgsqlParameter("prod_no", fp_prod.PROD_NO),
                    new NpgsqlParameter("prod_name", fp_prod.PROD_NAME),
                    new NpgsqlParameter("crt_user", upd_user)
                };

                NpgsqlHelper.ExecuteNonQuery(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        #endregion 固定價商品 Methods

        ////////////////

        #region 固定價商品套餐 Methods

        public static int GetFixedPricePackageCount(Int64 prod_xid, string filter)
        {
            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_fixedprice_prod_pkg
WHERE prod_xid=:prod_xid {FILTER}";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("prod_xid", prod_xid)
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

        // 取得固定價所有套餐　
        public static List<FixedPricePackageEx> GetFixedPricePackages(Int64 prod_xid, string filter, int skip, int size, string sorting)
        {
            List<FixedPricePackageEx> prods = new List<FixedPricePackageEx>();

            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_fixedprice_prod_pkg
WHERE prod_xid=:prod_xid {FILTER}
{SORTING}
LIMIT :Size OFFSET :Skip";

                sqlStmt = sqlStmt.Replace("{FILTER}", !string.IsNullOrEmpty(filter) ? filter : string.Empty);
                sqlStmt = sqlStmt.Replace("{SORTING}", !string.IsNullOrEmpty(sorting) ? "ORDER BY " + sorting : string.Empty);

                List<NpgsqlParameter> sqlParams = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("prod_xid", prod_xid),
                    new NpgsqlParameter("Size", size),
                    new NpgsqlParameter("Skip", skip)
                };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams.ToArray());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    prods.Add(new FixedPricePackageEx()
                    {
                        XID = dr.ToInt64("xid"),
                        PKG_NO = dr.ToStringEx("pkg_no"),
                        PKG_NAME = dr.ToStringEx("pkg_name"),
                        ONLINE_SDATE = dr.ToDateTime("online_sdate"),
                        ONLINE_EDATE = dr.ToDateTime("online_edate"),
                        PROD_XID = dr.ToInt64("prod_xid"),
                        Prices = GetPackagePrices(dr.ToInt64("xid"))
                    });
                }
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return prods;
        }

        public static List<FixedPricePackageDtl> GetPackagePrices(Int64 pkg_xid)
        {
            List<FixedPricePackageDtl> dtl_list = new List<FixedPricePackageDtl>();

            try
            {
                string sqlStmt = @"SELECT * FROM b2b.b2d_fixedprice_pkg_price
WHERE pkg_xid=:pkg_xid";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                    new NpgsqlParameter("pkg_xid", pkg_xid)
                };

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.SqlConnectionString, CommandType.Text, sqlStmt, sqlParams);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dtl_list.Add(new FixedPricePackageDtl()
                    {
                        XID = dr.ToInt64("xid"),
                        PKG_XID = dr.ToInt64("pkg_xid"),
                        PRICE_COND = dr.ToStringEx("price_cond"),
                        PRICE = dr.ToDouble("price")
                    });
                }

                return dtl_list;
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        // 匯入商品套餐與價格
        public static void ImportPackage(Areas.KKday.Models.DataModel.FixedPrice.ImportPackage pkg, string crt_user)
        {
            NpgsqlConnection conn = new NpgsqlConnection(Website.Instance.SqlConnectionString);
            NpgsqlTransaction trans = null;
            string sqlStmt;
            NpgsqlParameter[] sqlParams = null;

            try
            {
                conn.Open();
                trans = conn.BeginTransaction(); 
                
                foreach (var item in pkg.packages)
                {
                    sqlStmt = @"INSERT INTO b2b.b2d_fixedprice_prod_pkg(prod_xid, pkg_no, pkg_name, 
online_sdate, online_edate, crt_user, crt_datetime)
VAlUES (:prod_xid, :pkg_no, :pkg_name, :online_sdate, :online_edate, :crt_user, now());
SELECT currval('b2b.b2d_fixedprice_prod_pkg_xid_seq') AS new_pkg_xid;
";
                    sqlParams = new NpgsqlParameter[] {
                        new NpgsqlParameter("prod_xid", pkg.PROD_XID),
                        new NpgsqlParameter("pkg_no", item.PKG_NO),
                        new NpgsqlParameter("pkg_name", item.PKG_NAME),
                        new NpgsqlParameter("online_sdate", pkg.S_DATE),
                        new NpgsqlParameter("online_edate", pkg.E_DATE),
                        new NpgsqlParameter("crt_user", crt_user)
                    };

                    // 新增到 b2d_fixedprice_prod_pkg, 並取得序號
                    var new_pkg_xid = NpgsqlHelper.ExecuteScalar(trans, CommandType.Text, sqlStmt, sqlParams);

                    foreach (var price in item.prices)
                    { 
                        sqlStmt = @"INSERT INTO b2b.b2d_fixedprice_pkg_price(pkg_xid, price_cond, price, crt_user, crt_datetime)
VALUES (:pkg_xid, :price_cond, :price, :crt_user, now())";

                        sqlParams = new NpgsqlParameter[] {
                            new NpgsqlParameter("pkg_xid", new_pkg_xid),
                            new NpgsqlParameter("price_cond", price.COND),
                            new NpgsqlParameter("price", price.PRICE),
                            new NpgsqlParameter("crt_user", crt_user)
                        };

                        // 新增到 b2d_fixedprice_pkg_price
                        NpgsqlHelper.ExecuteNonQuery(trans, CommandType.Text, sqlStmt, sqlParams);
                    }
                }

                trans.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                Website.Instance._log.FatalFormat("{0}.{1}", ex.Message, ex.StackTrace);

                if (trans != null) trans.Rollback();
                conn.Close();

                throw ex;
            }
        }

        #endregion 固定價商品套餐 Methods
    }
}
