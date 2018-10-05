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

        public static JObject GetApiUser(string email,string pw) {

            var obj = new JObject();

            try {

                String sql = @"SELECT B.comp_name,B.comp_language,B.comp_currency,
                    B.comp_email, B.payment_type, A.*
                    FROM b2b.b2d_api_account A
                    JOIN b2b.b2d_company B ON A.company_xid = B.xid
                    WHERE A.enable = TRUE 
                    AND A.email = :email
                    AND A.password = :pw ";


                NpgsqlParameter[] np = new NpgsqlParameter[]{
                     new NpgsqlParameter("email",email),
                     new NpgsqlParameter("pw",pw)
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

      


        public string ToMD5(string pw) {

            using (var cryptoMD5 = System.Security.Cryptography.MD5.Create()) {
                //將字串編碼成 UTF8 位元組陣列
                var bytes = Encoding.UTF8.GetBytes(pw);

                //取得雜湊值位元組陣列
                var hash = cryptoMD5.ComputeHash(bytes);

                //取得 MD5
                var md5 = BitConverter.ToString(hash)
                  .Replace("-", String.Empty)
                  .ToUpper();

                return md5;
            }
        }

    }
}
