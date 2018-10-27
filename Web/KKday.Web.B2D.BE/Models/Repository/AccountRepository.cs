using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using KKday.Web.B2D.BE.AppCode;
using KKday.Web.B2D.BE.AppCode.DAL.Account;
using KKday.Web.B2D.BE.AppCode.DAL.RegisterDAL;
using KKday.Web.B2D.BE.Areas.KKday.Models.DataModel.Account;
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
            // 檢查登入者身分
            UserAccount account = AccountAuthDAL.UserAuth(email, Sha256Helper.Gethash(password));
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
