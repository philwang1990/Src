using System;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using log4net;

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
                String sql = @" SELECT xid,pms_supplier_name,pms_supplier_id,kkday_supplier_oid,scm_account,scm_password,pms_source 
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
    }
}
