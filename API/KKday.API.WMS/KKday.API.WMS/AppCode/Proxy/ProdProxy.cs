using System;
using System.Collections.Generic;
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

        //module type
        private static string _PMDL_CUST_DATA = "PMDL_CUST_DATA";
        private static string _PMDL_RENT_CAR = "PMDL_RENT_CAR";
        private static string _PMDL_CAR_PSGR = "PMDL_CAR_PSGR";
        private static string _PMDL_SEND_DATA = "PMDL_SEND_DATA";
        private static string _PMDL_SIM_WIFI = "PMDL_SIM_WIFI";
        private static string _PMDL_CONTACT_DATA = "PMDL_CONTACT_DATA";
        private static string _PMDL_FLIGHT_INFO = "PMDL_FLIGHT_INFO";
        private static string _PMDL_VENUE = "PMDL_VENUE";
        private static string _PMDL_EXCHANGE = "PMDL_EXCHANGE";

        //code lang type
        private static string _VOUCHER_EXCHANGE_TYPE = "VOUCHER_EXCHANGE_TYPE";

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

                        KKdayApiProdRQModel RQ = new KKdayApiProdRQModel()
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

                            Website.Instance.logger.Info($"URL:{url},URL Response StatusCode:{response.StatusCode}");
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

        public static JObject getModule(QueryProductModel query_lst)
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

                        List<string> typeList = new List<string>();
                        typeList.Add(ProdProxy._PMDL_CAR_PSGR);
                        typeList.Add(ProdProxy._PMDL_CONTACT_DATA);
                        typeList.Add(ProdProxy._PMDL_CUST_DATA);
                        typeList.Add(ProdProxy._PMDL_EXCHANGE);
                        typeList.Add(ProdProxy._PMDL_FLIGHT_INFO);
                        typeList.Add(ProdProxy._PMDL_RENT_CAR);
                        typeList.Add(ProdProxy._PMDL_SEND_DATA);
                        typeList.Add(ProdProxy._PMDL_SIM_WIFI);
                        typeList.Add(ProdProxy._PMDL_VENUE);


                        KKdayApiProdRQModel RQ = new KKdayApiProdRQModel()
                        {
                            apiKey = "kkdayapi",
                            userOid = "1",
                            ver = "1.0.1",
                            locale = query_lst.locale_lang,
                            currency = query_lst.current_currency,
                            ipaddress = "1",
                            json = new Json()
                            {
                                deviceId = "1122334455",
                                tokenKey = "2d0d8efc4f175fb55076a87f0c1897ef",
                                moduleTypes = typeList.ToArray()
                            }

                        };

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_MODEL"]}".Replace("{prod_no}", query_lst.prod_no);

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
                Website.Instance.logger.FatalFormat($"getProd  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }


        public static JObject getCodeLang(QueryProductModel query_lst)
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
                            currency = query_lst.current_currency,
                            ipaddress = "1",
                            json = new Json()
                            {

                            }

                        };

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_CODE_LANG"]}{ProdProxy._VOUCHER_EXCHANGE_TYPE}";

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
                Website.Instance.logger.FatalFormat($"getProd  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }
    }
}
