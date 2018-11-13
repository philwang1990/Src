using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using KKday.API.WMS.Models.DataModel.User;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace KKday.API.WMS.AppCode.DAL {
    public class UserDAL {
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="email">Email.</param>
        /// <param name="pw">Pw.</param>
        public static JObject GetUser(string email, string pw) {

            var obj = new JObject();

            try {

                String sql = @"SELECT B.comp_name,B.comp_locale,B.comp_currency,
                    B.contact_user_email, B.payment_type, A.*
                    FROM b2b.b2d_account A
                    JOIN b2b.b2d_company B ON A.company_xid = B.xid
                    WHERE A.enable = TRUE 
                    AND A.email = :email
                    AND A.password = :pw ";


                NpgsqlParameter[] np = new NpgsqlParameter[]{
                     new NpgsqlParameter("email",email),
                     new NpgsqlParameter("pw",pw)
                    };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql.ToString(), np);


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

        /// <summary>
        /// Gets the API user.
        /// </summary>
        /// <returns>The API user.</returns>
        /// <param name="email">Email.</param>
        public static JObject GetApiUser(string email) {

            var obj = new JObject();

            try {

                String sql = @"SELECT B.comp_name,comp_locale,B.comp_currency,
                    B.contact_user_email, B.payment_type, A.*
                    FROM b2b.b2d_account_api A
                    JOIN b2b.b2d_company B ON A.company_xid = B.xid
                    WHERE A.enable = TRUE                      
                    AND A.email = :email
                    AND B.status = '01'";


                NpgsqlParameter[] np = new NpgsqlParameter[]{
                     new NpgsqlParameter("email",email)
                    };

                DataSet ds = NpgsqlHelper.ExecuteDataset(Website.Instance.B2D_DB, CommandType.Text, sql.ToString(),np);


                if (ds != null && ds.Tables[0].Rows.Count > 0) {

                    ds.AcceptChanges();
                    //把dataset轉成結森物件
                    string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

                    obj = JObject.Parse(json);
                }
               
            } 
                catch (Exception ex) {
            
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }

           return obj;

        }

    }
}
