using System;
using System.Net.Http;
using System.Net.Http.Headers;
using KKday.API.WMS.Models.DataModel.Product;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.AppCode.Proxy
{
    public class CommonProxy
    {
        public CommonProxy()
        {
        }
        //
        public static JObject getCodeLang(QueryProductModel query_lst,string TYPE)
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

                        KKdayApiProdRQModel RQ = new KKdayApiProdRQModel()
                        {
                            apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                            userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                            ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                            locale = query_lst.locale_lang,
                            currency = query_lst.current_currency,
                            ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"],
                            json = new Json()
                            {

                            }

                        };

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_CODE_LANG"]}{TYPE}";

                        using (HttpContent content = new StringContent(json_data))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;

                            Website.Instance.logger.Info($"URL:{url},URL Response StatusCode:{response.StatusCode}");
                        }


                    }

                }
                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"getCodeLang  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        public static JObject getCodeCountry(QueryProductModel query_lst)
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

                        KKdayApiProdRQModel RQ = new KKdayApiProdRQModel()
                        {
                            apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                            userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                            ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                            locale = query_lst.locale_lang,
                            ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"],
                            json = new Json()
                            {

                            }

                        };

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_CODE_COUNTRY"]}";

                        using (HttpContent content = new StringContent(json_data))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;

                            Website.Instance.logger.Info($"URL:{url},URL Response StatusCode:{response.StatusCode}");
                        }


                    }

                }
                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"getCodeCountry  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        public static JObject getCodeArea(QueryProductModel query_lst)
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

                        KKdayApiProdRQModel RQ = new KKdayApiProdRQModel()
                        {
                            apiKey = "kkdayapi",
                            userOid = "1",
                            ver = "1.0.1",
                            locale = query_lst.locale_lang,
                            ipaddress = "1",
                            json = new Json()
                            {

                            }

                        };

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_CODE_AREA"]}";

                        using (HttpContent content = new StringContent(json_data))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;

                            Website.Instance.logger.Info($"URL:{url},URL Response StatusCode:{response.StatusCode}");
                        }


                    }

                }
                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"getCodeArea  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }
    }
}
