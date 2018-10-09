using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KKday.API.WMS.Models.DataModel.Product;
using KKday.Web.B2D.EC.AppCode;
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

        static RedisHelper rds = new RedisHelper();

        //取得商品detail
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
                            apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                            userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                            ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                            locale = query_lst.locale_lang,
                            currency = query_lst.current_currency,
                            ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"],
                            json = new Json()
                            {
                                infoType = Website.Instance.Configuration["KKAPI_INPUT:JSON:INFO_TYPE"],
                                cleanCache = "Y",
                                multipricePlatform = Website.Instance.Configuration["KKAPI_INPUT:JSON:MULTIPRICE_PLATFORM"]
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

                            //與API串接失敗 
                            if (response.StatusCode.ToString() != "OK")
                            {
                                throw new Exception(response.Content.ReadAsStringAsync().Result);
                            }

                        }


                    }

                }
                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API getProd Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        //取得商品旅規
        public static JObject getModule(QueryProductModel query_lst)
        {
            var obj = new JObject();
            try
            {
                string result = "";

                //redis取出資料
                if (rds.getProdInfotoRedis("bid:test:KKdayApi_getModule" + query_lst.b2d_xid) != null)
                {
                    result = rds.getProdInfotoRedis("bid:test:KKdayApi_getModule" + query_lst.b2d_xid);
                }
                else
                {
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
                                apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                                userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                                ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                                locale = query_lst.locale_lang,
                                currency = query_lst.current_currency,
                                ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"],
                                json = new Json()
                                {
                                    deviceId = Website.Instance.Configuration["KKAPI_INPUT:JSON:DEVICE_ID"],
                                    tokenKey = Website.Instance.Configuration["KKAPI_INPUT:JSON:TOKEN_KEY"],
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

                                //與API串接失敗 
                                if (response.StatusCode.ToString() != "OK")
                                {
                                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                                }
                                else
                                {
                                    rds.SetProdInfotoRedis(result, "bid:test:KKdayApi_getModule" + query_lst.b2d_xid);
                                }
                            }

                        }

                    }

                }

                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API getMoudle Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }



    }
}
