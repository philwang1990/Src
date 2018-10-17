using System;
using KKday.Web.B2D.BE.AppCode.DAL.Account;
using KKday.Web.B2D.BE.Models.Account;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class AccountRepository
    {
        public UserAccount GetAccount(string email, string password)
        {
            // 檢查登入者身分
            UserAccount account = AccountAuthDAL.UserAuth(email, password);

            // 以上皆非, 則送出登入身分異常
            if (!(account is KKdayAccount) && !(account is B2dAccount))
            {
                throw new Exception("Invalid User Login");
            }

            return account;
        }

        public B2dUserProfile GetProfile(string account)
        {
            return AccountDAL.GetProfile(account);
        }
         
        public bool SetNewPassword(string account, string password)
        {
            try
            {
                // 呼叫WMS-API設定使用者新密碼

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
 
}
