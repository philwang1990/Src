using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.AppCode.DAL
{
    public class CommonDAL
    {
        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <returns>The currency.</returns>
        /// <param name="locale">Locale.</param>
        public static JObject GetCurrency(string locale)
        {

            var obj = new JObject();

            try
            {

                String sql = @"SELECT CUR.currency,CUR.name  
                               FROM b2b.b2d_currency CUR
                                WHERE 
                                1=1
                                AND CUR.locale = :locale ";


                NpgsqlParameter[] np = new NpgsqlParameter[]{
                     new NpgsqlParameter("locale",locale)
                    };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql.ToString(), np);


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

                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }



            return obj;

        }
    }
}