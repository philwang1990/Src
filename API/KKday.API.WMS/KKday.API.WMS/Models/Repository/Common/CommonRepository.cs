using System;
using System.Data;
using KKday.API.WMS.AppCode.DAL;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.DataModel.Common;
using KKday.API.WMS.AppCode.Proxy;

namespace KKday.API.WMS.Models.Repository.Common
{
    public class CommonRepository
    {

        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <returns>The currency.</returns>
        /// <param name="locale">Locale.</param>
        public static CurrencyModel GetCurrency(string locale)
        {
            CurrencyModel currency = new CurrencyModel();
            //JObject result = new JObject();
            JObject obj = null;
            try
            {

                //obj = CommonDAL.GetCurrency(locale);
                obj = CurrencyProxy.getCurrency(locale);

                //if (obj != null && obj.Count > 0)
                if (obj["content"]["result"].ToString() == "0000")
                {
                    //result.Add("result","200");
                    //result.Add("result_msg", "OK");
                    //result.Add("currencyList", (JArray)obj["Table"]);

                    //JArray currencyList = (JArray)obj["Table"];
                                           

                    Json ms = new Json()
                    {
                        result = obj["content"]["result"].ToString(),
                        msg = obj["content"]["msg"].ToString()
                    };

                    currency.content = ms;

                    JArray codelst = (JArray)obj["content"]["codeList"];
                    Json2 ms2 = new Json2();
                    Json3 ms3 = new Json3();

                    for (int i = 0; i < codelst.Count ; i++ ){
                        ms.codeList.Add(ms2);

                    }




                }
                else
                {
                    //若找不到該語系的幣別 則回傳找不到的訊息 
                    //result.Add("result", "404");
                    //result.Add("result_msg", "currencyList not found!");

                    currency.content.result = obj["content"]["result"].ToString();
                    currency.content.msg = $"kkday package api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday currency api response msg is not correct! {obj["content"]["msg"].ToString()}");
                }

            }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat($"getCurrency  Error :{ex.Message},{ex.StackTrace}");

                throw ex;

            }

            return currency;

        }

      
    }
}
