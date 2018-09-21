using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KKday.API.WMS.Models.DataModel.Product;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.AppCode.Proxy
{
    public class ProdProxy
    {
        //static HttpClient client = new HttpClient();

        public static JObject getProd(QueryProductModel query_lst)
        {
            var obj = new JObject();
            try
            {
                string result = "";
                using(var handler = new HttpClientHandler())
                {
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };

                    using (var client = new HttpClient(handler))
                    {
                        client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        KKdayApiProdRequestModel RQ = new KKdayApiProdRequestModel()
                        {
                            apiKey = "kkdayapi",
                            userOid = "1",
                            ver = "1.0.1",
                            locale = query_lst.locale_lang,
                            currency = query_lst.current_currency,
                            ipaddress = "1",
                            json = new Json()
                            {
                                infoType = query_lst.query_Type,
                                cleanCache = "N",
                                multipricePlatform = "01"
                            }

                        };


                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_PROD"]}{query_lst.prod_no}";

                        using (HttpContent content = new StringContent(json_data))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;
                           
                        }
                    }

                }
                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"getProd  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

    }
}
