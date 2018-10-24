using System;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace KKday.API.WMS.AppCode.DAL {
    public class DiscountDAL {

        /// <summary>
        /// Gets the black list.撈所有的黑名單
        /// </summary>
        /// <returns>The black list.</returns>
        public static JObject GetBlackList() {

            var obj = new JObject();

            try {

                String sql = @"SELECT b.prod_no                     
                               FROM b2b.b2d_list_price_blacks b ";
                               
                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql.ToString());


                if (ds != null && ds.Tables[0].Rows.Count > 0) {

                    ds.AcceptChanges();
                    //把dataset轉成結森物件
                    string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

                    obj = JObject.Parse(json);
                }

            } catch (Exception ex) {

                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }

            return obj;
            }

        }
}
