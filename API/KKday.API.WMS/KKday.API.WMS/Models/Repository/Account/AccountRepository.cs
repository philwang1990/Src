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

        public static JObject InsertUser(AccountModel model)
        {
            int count = 0;

            try
            {

                count = AccountDAL.InsertUser(model);

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

    }
}
