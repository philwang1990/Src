using System;
using KKday.SearchProd.AppCode;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.AppCode.DAL.Account;
using KKday.Web.B2D.EC.AppCode.DAL.Register;
using KKday.Web.B2D.EC.Models.Model.Account;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.Web.B2D.EC.Models.Repostory.Account
{
    public  class AccountRepository
    {
        // 使用者認證 Authentication
        public static UserAccount GetAccount(string email, string password) 
        {
            // 檢查登入者身分
            string jsonResult = AccountProxy.GetUserAccount(email, password);
            UserAccount account = null;
            var jobjAcct = JObject.Parse(jsonResult);
 
            try
            {
                // 若無效身分則送出登入異常
                switch (jobjAcct["ACCOUNT_TYPE"].ToString())
                {
                    case "KKdayAccount":
                        account = jobjAcct["ACCOUNT"].ToObject<KKdayAccount>();
                        break;
                    case "B2dAccount":
                        account = jobjAcct["ACCOUNT"].ToObject<B2dAccount>();
                        break;
                    default: throw new Exception("Invalid User Login");
                }
            }
            catch
            {
                throw new Exception("Invalid User Login");
            };

            return account;
        }

        // 註冊新分銷商
        public void Register(RegisterModel reg)
        {
            try
            {
                if (reg.PASSWORD != null)
                {
                    reg.PASSWORD = Sha256Helper.Gethash(reg.PASSWORD);
                    reg.USER_UUID = Guid.NewGuid().ToString();

                    string[] time = reg.TIMEZONE.Split(new char[2] { ',', ':' });
                    reg.TIMEZONE = time[1];

                    string[] country = reg.COUNTRY_CODE.Split(new char[1] { ',' });
                    reg.COUNTRY_CODE = country[0];
                    reg.TEL_CODE = country[1];

                    RegisterDAL.InsCompany(reg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
