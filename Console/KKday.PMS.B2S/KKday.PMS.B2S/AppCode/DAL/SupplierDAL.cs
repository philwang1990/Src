using System;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using log4net;
using KKday.PMS.B2S.Models.PMSModel;
using System.Collections.Generic;
using System.Linq;
using KKday.PMS.B2S.Models.Shared.Enum;


namespace KKday.PMS.B2S.AppCode.DAL
{
    public class SupplierDAL
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(SupplierDAL));

        public static JObject GetSupplierList(string pms_source)
        {
            var obj = new JObject();

            try
            {
                String sql = @" SELECT xid,pms_supplier_name,pms_supplier_id,kkday_supplier_oid,pms_source 
                                FROM pms_suppliers
                                WHERE 
                                1=1
                                AND pms_source = :PMS_SOURCE ";

                NpgsqlParameter[] sqlParams = new NpgsqlParameter[] {
                        new NpgsqlParameter("PMS_SOURCE", pms_source)
                    };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Startup.Instance.npg_conn, CommandType.Text, sql, sqlParams);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ds.AcceptChanges();
                    //把dataset轉成結森物件
                    string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

                    obj = JObject.Parse(json);
                }
            }
            catch (Exception ex)
            {
                _log.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }

            return obj;
        }

        /// <summary>
        /// Inserts the PMSS upplier product.
        /// </summary>
        /// <returns>The PMSS upplier product.</returns>
        /// <param name="pmsSupplierProductModel">Pms supplier product model.</param>
        public static int InsertPMSSupplierProduct(PMSSupplierProductModel pmsSupplierProductModel)
        {
            try
            {
                String elementStr = "";
                String valueStr = "";
                String sql = @"INSERT INTO pms_supplier_product ({0}) VALUES ({1})";

                elementStr = "product_datamodel,package_datamodel,bookingfields_datamodel,creator,create_datetime";
                valueStr = "'{}','{}','{}','console',now()";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

                if (pmsSupplierProductModel.pms_suppliers_xid > 0)
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}pms_suppliers_xid";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:pms_suppliers_xid";
                    parameters.Add(new NpgsqlParameter("pms_suppliers_xid", pmsSupplierProductModel.pms_suppliers_xid));
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.pms_supplier_id))
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}pms_supplier_id";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:pms_supplier_id";
                    parameters.Add(new NpgsqlParameter("pms_supplier_id", pmsSupplierProductModel.pms_supplier_id));
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.prod_code))
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}prod_code";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:prod_code";
                    parameters.Add(new NpgsqlParameter("prod_code", pmsSupplierProductModel.prod_code));
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.prod_name))
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}prod_name";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:prod_name";
                    parameters.Add(new NpgsqlParameter("prod_name", pmsSupplierProductModel.prod_name));
                }
                if (pmsSupplierProductModel.kkday_prod_oid > 0)
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}kkday_prod_oid";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:kkday_prod_oid";
                    parameters.Add(new NpgsqlParameter("kkday_prod_oid", pmsSupplierProductModel.kkday_prod_oid));
                }
                if (pmsSupplierProductModel.prod_create_datetime != DateTime.MinValue)
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}prod_create_datetime";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:prod_create_datetime";
                    parameters.Add(new NpgsqlParameter("prod_create_datetime", pmsSupplierProductModel.prod_create_datetime));
                }
                if (pmsSupplierProductModel.prod_update_datetime != DateTime.MinValue)
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}prod_update_datetime";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:prod_update_datetime";
                    parameters.Add(new NpgsqlParameter("prod_update_datetime", pmsSupplierProductModel.prod_create_datetime));
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.is_availability))
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}is_availability";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:is_availability";
                    parameters.Add(new NpgsqlParameter("is_availability", pmsSupplierProductModel.is_availability));
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.is_finish))
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}is_finish";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:is_finish";
                    parameters.Add(new NpgsqlParameter("is_finish", pmsSupplierProductModel.is_finish));
                }

                sql = string.Format(sql, elementStr, valueStr);

                //log
                _log.Info($"Insert PMSSupplierProduct: {sql}");

                return NpgsqlHelper.ExecuteNonQuery(Startup.Instance.npg_conn, CommandType.Text, sql, parameters.ToArray());
            }
            catch (Exception ex)
            {
                _log.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// Updates the PMSS upplier product.
        /// </summary>
        /// <returns>The PMSS upplier product.</returns>
        /// <param name="pmsSupplierProductModel">Pms supplier product model.</param>
        public static int UpdatePMSSupplierProduct(PMSSupplierProductModel pmsSupplierProductModel)
        {
            if (string.IsNullOrEmpty(pmsSupplierProductModel.prod_code)) { throw new Exception("prod_code 不能為空."); }
            var obj = new JObject();

            try
            {
                string updateStr = "";
                String sql = @"UPDATE pms_supplier_product SET {0}";

                updateStr = "update_datetime=now()";

                if (!string.IsNullOrEmpty(pmsSupplierProductModel.prod_name))
                {
                    updateStr += $",prod_name='{pmsSupplierProductModel.prod_name}'";
                }
                if (pmsSupplierProductModel.prod_update_datetime != DateTime.MinValue)
                {
                    updateStr += $",prod_update_datetime='{pmsSupplierProductModel.prod_update_datetime.ToString("yyyy-MM-dd HH:mm:ss")}'";
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.is_availability))
                {
                    updateStr += $",is_availability='{pmsSupplierProductModel.is_availability}'";
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.is_finish))
                {
                    updateStr += $",is_finish='{pmsSupplierProductModel.is_finish}'";
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.product_datamodel))
                {
                    updateStr += $",product_datamodel='{pmsSupplierProductModel.product_datamodel}'";
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.package_datamodel))
                {
                    updateStr += $",package_datamodel='{pmsSupplierProductModel.package_datamodel}'";
                }
                if (!string.IsNullOrEmpty(pmsSupplierProductModel.bookingfields_datamodel))
                {
                    updateStr += $",bookingfields_datamodel='{pmsSupplierProductModel.bookingfields_datamodel}'";
                }

                sql = string.Format(sql, updateStr);
                sql += $" WHERE prod_code='{pmsSupplierProductModel.prod_code}'";

                //log
                _log.Info($"Update PMSSupplierProduct: {sql}");

                return NpgsqlHelper.ExecuteNonQuery(Startup.Instance.npg_conn, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                _log.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// Inserts the PMSS upplier.
        /// </summary>
        /// <returns>The PMSS upplier.</returns>
        /// <param name="pmsSupplierModel">Pms supplier model.</param>
        public static int InsertPMSSupplier(PMSSupplierModel pmsSupplierModel)
        {
            var obj = new JObject();

            try
            {
                String elementStr = "";
                String valueStr = "";
                String sql = @"INSERT INTO pms_suppliers ({0}) VALUES ({1})";

                elementStr = "create_datetime";
                valueStr = "now()";

                List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

                if (!string.IsNullOrEmpty(pmsSupplierModel.pms_supplier_name))
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}pms_supplier_name";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:pms_supplier_name";
                    parameters.Add(new NpgsqlParameter("pms_supplier_name", pmsSupplierModel.pms_supplier_name));
                }
                if (!string.IsNullOrEmpty(pmsSupplierModel.pms_supplier_id))
                {
                    elementStr += elementStr.Length > 0 ? $",pms_supplier_id" : "pms_supplier_id";
                    valueStr += valueStr.Length > 0 ? $",:pms_supplier_id" : ":pms_supplier_id";

                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}pms_supplier_name";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:pms_supplier_name";
                    parameters.Add(new NpgsqlParameter("pms_supplier_id", pmsSupplierModel.pms_supplier_id));
                }
                if (pmsSupplierModel.kkday_supplier_oid > 0)
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}kkday_supplier_oid";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:kkday_supplier_oid";
                    parameters.Add(new NpgsqlParameter("kkday_supplier_oid", pmsSupplierModel.kkday_supplier_oid));
                }
                if (!string.IsNullOrEmpty(pmsSupplierModel.pms_source))
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}pms_source";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}:pms_source";
                    parameters.Add(new NpgsqlParameter("pms_source", pmsSupplierModel.pms_source));
                }
                if (!string.IsNullOrEmpty(pmsSupplierModel.creator))
                {
                    elementStr += $"{(elementStr.Length > 0 ? "," : "")}creator";
                    valueStr += $"{(valueStr.Length > 0 ? "," : "")}: creator";
                    parameters.Add(new NpgsqlParameter("creator", pmsSupplierModel.creator));
                }

                sql = string.Format(sql, elementStr, valueStr);

                //log
                _log.Info($"Insert PMSSupplier: {sql}");

                return NpgsqlHelper.ExecuteNonQuery(Startup.Instance.npg_conn, CommandType.Text, sql, parameters.ToArray());
            }
            catch (Exception ex)
            {
                _log.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// Updates the PMSS upplier.
        /// </summary>
        /// <returns>The PMSS upplier.</returns>
        /// <param name="pmsSupplierModel">Pms supplier model.</param>
        public static int UpdatePMSSupplier(PMSSupplierModel pmsSupplierModel)
        {
            if (pmsSupplierModel.xid == 0) { throw new Exception("xid 不能為空."); }
            var obj = new JObject();

            try
            {
                string updateStr = "";
                String sql = @"UPDATE pms_suppliers SET {0}";

                updateStr = "update_datetime=now()";

                if (!string.IsNullOrEmpty(pmsSupplierModel.pms_supplier_name))
                {
                    updateStr += $",pms_supplier_name='{pmsSupplierModel.pms_supplier_name}'";
                }
                if (!string.IsNullOrEmpty(pmsSupplierModel.pms_supplier_id))
                {
                    updateStr += $",pms_supplier_id='{pmsSupplierModel.pms_supplier_id}'";
                }
                if (pmsSupplierModel.kkday_supplier_oid > 0)
                {
                    updateStr += $",kkday_supplier_oid='{pmsSupplierModel.kkday_supplier_oid}'";
                }
                if (!string.IsNullOrEmpty(pmsSupplierModel.pms_source))
                {
                    updateStr += $",pms_source='{pmsSupplierModel.pms_source}'";
                }
                if (!string.IsNullOrEmpty(pmsSupplierModel.updater))
                {
                    updateStr += $",updater='{pmsSupplierModel.updater}'";
                }

                sql = string.Format(sql, updateStr);
                sql += $" WHERE xid='{pmsSupplierModel.xid}'";

                //log
                _log.Info($"Update PMSSupplier: {sql}");

                return NpgsqlHelper.ExecuteNonQuery(Startup.Instance.npg_conn, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                _log.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// Inserts the product map log.
        /// </summary>
        /// <returns>The product map log.</returns>
        /// <param name="productMapLogModel">Product map log model.</param>
        public static int InsertProductMapLog(ProductMapLogModel productMapLogModel)
        {
            var obj = new JObject();

            try
            {
                string sql = "INSERT INTO product_map_log (kkday_prod_oid,step,create_datetime) " +
                             $"VALUES ({productMapLogModel.kkday_prod_oid},'{productMapLogModel.step.ToString()}',now())";

                //log
                _log.Info($"Insert ProductMapLog: {sql}");

                return NpgsqlHelper.ExecuteNonQuery(Startup.Instance.npg_conn, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                _log.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                return 0;
            }
        }

        /// <summary>
        /// Queries the product map log.
        /// </summary>
        /// <returns>The product map log.</returns>
        /// <param name="productMapLogModel">Product map log model.</param>
        public static List<ProductMapLogModel> QueryProductMapLog(ProductMapLogModel productMapLogModel = null)
        {
            List<ProductMapLogModel> results = new List<ProductMapLogModel>();
            try
            {
                string sql = @"SELECT * FROM product_map_log ";
                string queryStr = "";

                if (productMapLogModel != null)
                {
                    if (productMapLogModel.xid > 0)
                    {
                        queryStr += $"{(queryStr.Length > 0 ? "AND" : "")} xid={productMapLogModel.xid}";
                    }
                    if (productMapLogModel.kkday_prod_oid > 0)
                    {
                        queryStr += $"{(queryStr.Length > 0 ? "AND" : "")} kkday_prod_oid={productMapLogModel.kkday_prod_oid}";
                    }

                    sql += $" WHERE {queryStr}";
                }

                DataSet ds = NpgsqlHelper.ExecuteDataset(Startup.Instance.npg_conn, CommandType.Text, sql);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    results = ds.Tables[0].AsEnumerable()
                                    .Select(dataRow =>
                                    new ProductMapLogModel
                                    {
                                        xid = dataRow.Field<long>("xid"),
                                        kkday_prod_oid = dataRow.Field<long>("kkday_prod_oid"),
                                        step = (Step)Enum.Parse(typeof(Step), dataRow.Field<string>("step"), false),
                                        create_datetime = dataRow.Field<DateTime>("create_datetime")
                                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                _log.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
                return results;
            }
            return results;
        }
    }
}
