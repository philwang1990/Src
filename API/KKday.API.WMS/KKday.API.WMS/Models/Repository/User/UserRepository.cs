using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using KKday.API.WMS.Models.DataModel.User;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.Models.Repository.User {
    public class UserRepository {

        /// <summary>
        /// Gets the user.抓一般使用者資訊
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="email">email.</param>
        /// <param name="pw">Pw.</param>
        public static ApiUserModel GetUser(string email, string pw) {

            ApiUserModel aum = new ApiUserModel();
            try {

                JObject obj = UserDAL.GetUser(email, pw);

                if (obj != null && obj.Count > 0) {

                    aum.result = "200";
                    aum.result_msg = "OK";
                    aum.user_xid = (Int64)obj["Table"][0]["xid"];
                    aum.user_name = obj["Table"][0]["name_first"].ToString()
                        + obj["Table"][0]["name_last"].ToString();
                    aum.user_email = obj["Table"][0]["account"].ToString();
                    aum.company_xid = (Int64)obj["Table"][0]["company_xid"];
                    aum.comapny_name = obj["Table"][0]["comp_name"].ToString();
                    aum.company_language = obj["Table"][0]["comp_language"].ToString();
                    aum.company_currency = obj["Table"][0]["comp_currency"].ToString();
                    aum.payment_type = obj["Table"][0]["payment_type"].ToString();

                } else {
                    //若帳密有誤 僅傳送錯誤代碼 
                    aum.result = "401";
                    aum.result_msg = "Unauthorized";
                }

            } catch (Exception ex) {

                Website.Instance.logger.FatalFormat($"getUser  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }

            return aum;

        }

        /// <summary>
        /// Gets the API user.抓API使用者的資訊
        /// </summary>
        /// <returns>The API user.</returns>
        /// <param name="email">email.</param>
        public static ApiUserModel GetApiUser(string email) {

            ApiUserModel aum = new ApiUserModel();
            try {

                JObject obj = UserDAL.GetApiUser(email);

                if (obj != null && obj.Count > 0) {
                   
                        aum.result = "200";
                        aum.result_msg = "OK";
                        aum.user_xid = (Int64)obj["Table"][0]["xid"];
                        aum.user_name = obj["Table"][0]["name_first"].ToString()
                            + obj["Table"][0]["name_last"].ToString();
                        aum.user_email = obj["Table"][0]["account"].ToString();
                        aum.company_xid = (Int64)obj["Table"][0]["company_xid"];
                        aum.comapny_name = obj["Table"][0]["comp_name"].ToString();
                        aum.company_language = obj["Table"][0]["comp_language"].ToString();
                        aum.company_currency = obj["Table"][0]["comp_currency"].ToString();
                        aum.payment_type = obj["Table"][0]["payment_type"].ToString();
                       
                    } else {
                        //若帳密有誤 僅傳送錯誤代碼 
                        aum.result = "401";
                        aum.result_msg = "Unauthorized";
                    }

                } catch (Exception ex) {

                Website.Instance.logger.FatalFormat($"getApiUser  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }

            return aum;

        }
    }
}
