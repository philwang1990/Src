using System;
using KKday.Web.B2D.BE.Models.Account;

namespace KKday.Web.B2D.BE.Models.Repository
{
    public class AccountRepository
    {
        public UserAccount GetAccount(string email, string password)
        {
            UserAccount account = null;
            // 檢查登入者身分
            bool IsKKdayUser = false, IsB2dUser = false;

            // 檢查KKday帳號
            if (email.IndexOf("kkday.com", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                // 叫用WMS-API驗證KKday使用者身分

                // 判斷是否有效使用者
                {
                    IsKKdayUser = true;

                    account = new KKdayAccount()
                    {
                        XID = 581,
                        UUID = "451706a7a6724aebaa0e7638db2ca567",
                        ACCOUNT = email,
                        GENDER_TITLE = "Mr.",
                        EMAIL = "eric.hu@KKday.com",
                        ENABLE = true,
                        STAFF_NO = "KK00581",
                        NAME_FIRST = "胡",
                        NAME_LAST = "良寬",
                        DEPARTMENT = "bid",
                        ROLES = "SYS"
                    };
                }
            }
            // 檢查分銷商帳號
            else
            {
                // 叫用WMS-API驗證分銷商使用者身分

                // 判斷是否有效使用者
                {
                    IsB2dUser = true;

                    account = new B2dAccount()
                    {
                        XID = 100,
                        UUID = "a2933afeae764451a4fa3e48a27e1de5",
                        ACCOUNT = email,
                        GENDER_TITLE = "Mr.",
                        EMAIL = "guest@example.com",
                        ENABLE = true,
                        NAME_FIRST = "王",
                        NAME_LAST = "大名",
                        COMPANY_XID = 1,
                        USER_TYPE = "01", // 00:使用者 01: 管理者
                        COMPANY_NAME = "酷遊天旅行社",
                        LOCALE = "zh-TW",
                        CURRENCY = "TWD"
                    };
                }
            }

            // 以上皆非, 則送出登入身分異常
            if (!IsKKdayUser && !IsB2dUser)
            {
                throw new Exception("Invalid User Login");
            }

            return account;
        }
         
        public bool SetNewPassword(string email, string password)
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
