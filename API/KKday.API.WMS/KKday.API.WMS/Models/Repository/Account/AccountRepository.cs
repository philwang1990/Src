using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.DataModel.Account;
using KKday.API.WMS.AppCode.Proxy;
using System.Collections.Generic;
using Npgsql;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using KKday.API.WMS.AppCode;
using System.Linq;

namespace KKday.API.WMS.Models.Repository.Account
{
    public class AccountRepository
    {

        public static JObject RegisterIs4User(AccountModel model)
        {
            int count = 0;

            try
            {

                count = AccountDAL.RegisterIs4User(model);

                return JObject.Parse("{ \"result\":  \"0000\",\"result_msg\": \"OK\",\"count\":" + count.ToString() + "}");
            }
            catch (Exception ex)
            {

                return JObject.Parse("{ \"result\":  \"10001\",\"result_msg\": \"InsertOrder  Error :\"" + ex.Message + "," + ex.StackTrace + ",\"count\":" + count + "}");

            }
        }

        public static JObject UpdatetUser(AccountModel model)
        {
            int count = 0;

            try
            {

                count = AccountDAL.UpdatetUser(model);

                return JObject.Parse("{ \"result\":  \"0000\",\"result_msg\": \"OK\",\"count\":" + count.ToString() + "}");
            }
            catch (Exception ex)
            {

                return JObject.Parse("{ \"result\":  \"10001\",\"result_msg\": \"InsertOrder  Error :\"" + ex.Message + "," + ex.StackTrace + ",\"count\":" + count + "}");

            }
        }

        #region 註冊新分銷商

        public static RegisterRSModel RegisterAccount(RegisterRQModel reg)
        {
            RegisterRSModel rs = new RegisterRSModel();
            try
            {
                if (reg.PASSWORD != null)
                {
                    reg.PASSWORD = Sha256Helper.Gethash(reg.PASSWORD);
                    reg.USER_UUID = Guid.NewGuid().ToString();
                    RegisterDAL.InsCompany(reg, ref rs);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rs;
        }

        #endregion

    }
}
