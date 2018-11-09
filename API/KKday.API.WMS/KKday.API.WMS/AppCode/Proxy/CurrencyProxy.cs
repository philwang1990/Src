using System;
using System.Net.Http;
using System.Net.Http.Headers;
using KKday.API.WMS.Models.DataModel.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.AppCode.Proxy
{
    public class CurrencyProxy
    {
        public CurrencyProxy()
        {

        }
        //

        static RedisHelper rds = new RedisHelper();

        public static JObject GetCurrency(string locale)
        {
            var obj = new JObject();
            try
            {
                string result = "";

                using (var handler = new HttpClientHandler())
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

                        KKdayApiCurrencyRQModel RQ = new KKdayApiCurrencyRQModel()
                        {
                            apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                            userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                            ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                            locale = locale,
                            ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"]
                           

                        };

                       


                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_CURRENCY"]}";

                        using (HttpContent content = new StringContent(json_data))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;

                            Website.Instance.logger.Info($"URL:{url},URL Response StatusCode:{response.StatusCode}");
                            //與API串接失敗 
                            if (response.StatusCode.ToString() != "OK")
                            {
                                throw new Exception(response.Content.ReadAsStringAsync().Result);
                            }
                            //串接成功
                            else
                            {
                                //rds.SetProdInfotoRedis(result, "bid:test:KKdayApi_getProdAirport" + query_lst.b2d_xid);
                            }
                        }


                        //}

                    }
                }

                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API GetCurrency Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        public static JObject GetProductCountryCity(KKdayApiCurrencyRQModel RQ)
        {

            var obj = new JObject();
            try
            {
                string result = "";

                using (var handler = new HttpClientHandler())
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

                        //KKdayApiCurrencyRQModel RQ = new KKdayApiCurrencyRQModel()
                        //{
                        //    apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                        //    userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                        //    ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                        //    locale = locale,
                        //    ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"]


                        //};




                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_COUNTRY_CITY"]}";

                        using (HttpContent content = new StringContent(json_data))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;

                            Website.Instance.logger.Info($"URL:{url},URL Response StatusCode:{response.StatusCode}");
                            //與API串接失敗 
                            if (response.StatusCode.ToString() != "OK")
                            {
                                throw new Exception(response.Content.ReadAsStringAsync().Result);
                            }
                            //串接成功
                            else
                            {
                                //rds.SetProdInfotoRedis(result, "bid:test:KKdayApi_getProdAirport" + query_lst.b2d_xid);
                            }
                        }


                        //}

                    }
                }

                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API GetProductCountryCity Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;

        }

      
    }

}
