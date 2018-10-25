using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using KKday.Web.B2D.BE.AppCode;
using KKday.Web.B2D.BE.AppCode.DAL.Account;
using KKday.Web.B2D.BE.AppCode.DAL.RegisterDAL;
using KKday.Web.B2D.BE.Areas.KKday.Models.DataModel.Account;
using KKday.Web.B2D.BE.Models.Common;
using KKday.Web.B2D.BE.Models.Model.Account;
using KKday.Web.B2D.BE.Models.Model.Common;
using Resources;

namespace KKday.Web.B2D.BE.Models.Repository
{

    public class AccountRepository
    {
        private readonly ILocalizer _localizer;

        public AccountRepository(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        #region 使用者認證 Authentication

        public UserAccount GetAccount(string email, string password)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
            byte[] source = Encoding.Default.GetBytes(password);//將字串轉為Byte[]
            byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
            var chiperPasswod = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串

            // 檢查登入者身分
            UserAccount account = AccountAuthDAL.UserAuth(email, chiperPasswod);
            // 若無效身分則送出登入異常
            if (!(account is KKdayAccount) && !(account is B2dAccount))
            {
                throw new Exception("Invalid User Login");
            }

            return account;
        }

        public B2dUserProfile GetProfile(string account)
        {
            return AccountAuthDAL.GetB2dProfile(account);
        }

        #endregion

        #region 註冊新分銷商

        public void Register(RegisterModel reg)
        {
            try
            {
                if (reg.PASSWORD != null)
                {
                    reg.PASSWORD = Sha256Helper.Gethash(reg.PASSWORD);
                    reg.USER_UUID = Guid.NewGuid().ToString();
                    RegisterDAL.InsCompany(reg);
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
    }
}