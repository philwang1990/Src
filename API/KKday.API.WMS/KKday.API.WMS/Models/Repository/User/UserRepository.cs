using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using KKday.API.WMS.AppCode;
using KKday.API.WMS.AppCode.DAL;
using KKday.API.WMS.Models.DataModel.User;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.Models.Repository {
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

                //1.將明碼加密 
                SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
                byte[] source = Encoding.Default.GetBytes(pw);//將字串轉為Byte[]
                byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
                var chiperPW = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串

                //2.檢查登入者身分
                JObject obj = UserDAL.GetUser(email, chiperPW);

                if (obj != null && obj.Count > 0) {

                    aum.result = "00";
                    aum.result_msg = "OK";
                    aum.user_xid = (Int64)obj["Table"][0]["xid"];
                    aum.user_name = obj["Table"][0]["name_first"].ToString()
                        + obj["Table"][0]["name_last"].ToString();
                    aum.user_email = obj["Table"][0]["email"].ToString();
                    aum.company_xid = (Int64)obj["Table"][0]["company_xid"];
                    aum.comapny_name = obj["Table"][0]["comp_name"].ToString();
                    aum.company_language = obj["Table"][0]["comp_locale"].ToString();
                    aum.company_currency = obj["Table"][0]["comp_currency"].ToString();
                    aum.payment_type = obj["Table"][0]["payment_type"].ToString();

                } else {
                    //若帳密有誤 僅傳送錯誤代碼 
                    aum.result = "03";
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
                   
                        aum.result = "00";
                        aum.result_msg = "OK";
                        aum.user_xid = (Int64)obj["Table"][0]["xid"];
                        aum.user_name = obj["Table"][0]["name_first"].ToString()
                            + obj["Table"][0]["name_last"].ToString();
                        aum.user_email = obj["Table"][0]["email"].ToString();
                        aum.company_xid = (Int64)obj["Table"][0]["company_xid"];
                        aum.comapny_name = obj["Table"][0]["comp_name"].ToString();
                        aum.company_language = obj["Table"][0]["comp_locale"].ToString();
                        aum.company_currency = obj["Table"][0]["comp_currency"].ToString();
                        aum.payment_type = obj["Table"][0]["payment_type"].ToString();
                       
                    } else {
                        //若帳密有誤 僅傳送錯誤代碼 
                        aum.result = "03";
                        aum.result_msg = "Unauthorized";
                    }

                } catch (Exception ex) {

                Website.Instance.logger.FatalFormat($"getApiUser  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }

            return aum;

        }

        #region 使用者認證 Authentication

        public static B2dAccountModel AuthAccount(string email, string password)
        {
            // 檢查登入者身分
            //B2dAccountModel account = AccountAuthDAL.UserAuth(email, Sha256Helper.Gethash(password));
            B2dAccountModel account = AccountAuthDAL.UserAuth(email, password);
            // 若無效身分則送出登入異常
            if (account.ACCOUNT is null)
            {
                //若帳密有誤 僅傳送錯誤代碼 
                account.result = "02";
                account.result_msg = "Invalid User Login";

            }

            return account;
        }

        public static B2dAccountModel AuthApiAccount(string email)
        {
            // 檢查登入者身分
            B2dAccountModel account = AccountAuthDAL.UserApiAuth(email);
            // 若無效身分則送出登入異常
            if (account is null)
            {
                //若帳密有誤 僅傳送錯誤代碼 
                account.result = "02";
                account.result_msg = "Invalid User Login";

            }

            return account;
        }



        public static B2dUserProfile GetProfile(string account)
        {
            return AccountAuthDAL.GetB2dProfile(account);
        }

        #endregion



    }
}
