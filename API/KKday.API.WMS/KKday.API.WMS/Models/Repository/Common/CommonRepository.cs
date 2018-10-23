using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.Models.Repository.Common
{
    public class CommonRepository
    {

        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <returns>The currency.</returns>
        /// <param name="locale">Locale.</param>
        public static JObject GetCurrency(string locale)
        {

            JObject result = new JObject();
            try
            {

                JObject obj = CommonDAL.GetCurrency(locale);

                if (obj != null && obj.Count > 0)
                {
                    result.Add("result","200");
                    result.Add("result_msg", "OK");
                    result.Add("currencyList", (JArray)obj["Table"]);

                    JArray currencyList = (JArray)obj["Table"];

                   


                }
                else
                {
                    //若找不到該語系的幣別 則回傳找不到的訊息 
                    result.Add("result", "404");
                    result.Add("result_msg", "currencyList not found!");
                }

            }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat($"getCurrency  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }

            return result;

        }

      
    }
}
