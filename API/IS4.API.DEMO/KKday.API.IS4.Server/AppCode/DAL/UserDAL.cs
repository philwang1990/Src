using System;
using System.Collections.Generic;
using System.Data;
using KKday.API.IS4.Server.Models.DataModel.User;

namespace KKday.API.IS4.Server.AppCode.DAL {
    public class UserDAL {

        //public static DataSet GetAllUser() {

        //    DataSet ds = null;

        //    try {

        //        string strsql = "select * from is4.users";

        //        ds = NpgsqlHelper.ExecuteDataset(Website.Instance.ERP_DB, CommandType.Text, strsql);
        //    }
        //    catch (Exception ex) {

        //        Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
        //        throw ex;
        //    }
        //    return ds;

        //}

        // 取得使用者清單
        public static List<CustomUser> GetAllUser() {

            List<CustomUser> ulist = new List<CustomUser>();

            try {

                string strsql = "select * from is4.users where status='01'";

                var ds = NpgsqlHelper.ExecuteDataset(Website.Instance.IS4_DB, CommandType.Text, strsql);
                //  Console.WriteLine("DB連線："+$"{Website.Instance.IS4_DB}");

                if (ds != null && ds.Tables[0].Rows.Count > 0) {

                    foreach (DataRow dr in ds.Tables[0].Rows) {
                        CustomUser user = new CustomUser() {

                     SubjectId = dr.ToStringEx("XID"),
                            UserName = dr.ToStringEx("USER_NO"),
                            Password = dr.ToStringEx("USER_PASS"),
                            IsActive = dr.ToStringEx("STATUS").Equals("01") ? true : false//00 diseable

                        };

                        ulist.Add(user);
                    }
                }

            } catch (Exception ex) {

                Console.WriteLine($"{ex.Message} {ex.StackTrace} {Website.Instance.IS4_DB}");
                Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }

            return ulist;
        }
    }
}
