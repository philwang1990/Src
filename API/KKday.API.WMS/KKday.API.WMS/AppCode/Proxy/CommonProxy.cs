using System;
using System.Net.Http;
using System.Net.Http.Headers;
using KKday.API.WMS.Models.DataModel.Common;
using KKday.API.WMS.Models.DataModel.Product;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Json = KKday.API.WMS.Models.DataModel.Product.Json;

namespace KKday.API.WMS.AppCode.Proxy
{
    public class CommonProxy
    {
        public CommonProxy()
        {

        }
        //

        //static RedisHelper rds = new RedisHelper();
        private static RedisHelper rds;

        //各種type的翻譯 ex:VOUCHER_EXCHANGE_TYPE
        public static JObject getCodeLang(QueryProductModel query_lst,string TYPE)
        {
            var obj = new JObject();
            try
            {
                string result = "";

                //redis取出資料
                //if (rds.getProdInfotoRedis("bid:test:KKdayApi_getCodeLang" + query_lst.b2d_xid) != null)
                //{
                //    result = rds.getProdInfotoRedis("bid:test:KKdayApi_getCodeLang" + query_lst.b2d_xid);
                //}
                //else
                //{
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
                                //與API串接失敗 
                                if (response.StatusCode.ToString() != "OK")
                                {
                                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                                }//串接成功
                                else
                                {
                                    //rds.SetProdInfotoRedis(result, "bid:test:KKdayApi_getCodeLang" + query_lst.b2d_xid);
                                }
                            }


                        }

                    }
                //}
                   
                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API getCodeLang  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }
        //取出國家代碼 國家名稱 local+english 電話國碼 
        public static JObject getCodeCountry(QueryProductModel query_lst)
        {
            var obj = new JObject();
            try
            {
                string result = "";

                //redis取出資料
                //if (rds.getProdInfotoRedis("bid:test:KKdayApi_getCodeCountry" + query_lst.b2d_xid) != null)
                //{
                //    result = rds.getProdInfotoRedis("bid:test:KKdayApi_getCodeCountry" + query_lst.b2d_xid);
                //}
                //else
                //{
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

                                //與API串接失敗 
                                if (response.StatusCode.ToString() != "OK")
                                {
                                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                                }
                                //串接成功
                                else
                                {
                                    //rds.SetProdInfotoRedis(result, "bid:test:KKdayApi_getCodeCountry" + query_lst.b2d_xid);
                                }

                            }

                        }

                    //}
                }
                  
                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API getCodeCountry Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }
        /// <summary>
        /// Gets the code area.
        /// </summary>
        /// <returns>The code area.</returns>
        /// <param name="query_lst">Query lst.</param>
        //取得 亞洲-日本-大阪|||A01-003-00002 ,亞洲-日本-東京|||A01-003-00001 洲-國家-城市
        public static JObject getCodeArea(QueryProductModel query_lst)
        {
            var obj = new JObject();
            try
            {
                string result = "";

                //redis取出資料
                //if (rds.getProdInfotoRedis("bid:test:KKdayApi_getCodeArea" + query_lst.b2d_xid) != null)
                //{
                //    result = rds.getProdInfotoRedis("bid:test:KKdayApi_getCodeArea" + query_lst.b2d_xid);
                //}
                //else
                //{
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
                            string url = $"{Website.Instance.Configuration["URL:KK_CODE_AREA"]}";

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
                                    //rds.SetProdInfotoRedis(result, "bid:test:KKdayApi_getCodeArea" + query_lst.b2d_xid);
                                }
                            }


                        //}

                    }
                }
                    
                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API getCodeArea Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        public static JObject getProdAirport(QueryProductModel query_lst)
        {
            var obj = new JObject();
            try
            {
                string result = "";

                //redis取出資料
                //if (rds.getProdInfotoRedis("bid:test:KKdayApi_getProdAirport" + query_lst.b2d_xid) != null)
                //{
                //    result = rds.getProdInfotoRedis("bid:test:KKdayApi_getProdAirport" + query_lst.b2d_xid);
                //}
                //else
                //{

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
                            string url = $"{Website.Instance.Configuration["URL:KK_AIRPORT"]}{query_lst.prod_no}";

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
                Website.Instance.logger.FatalFormat($"KKday API getProdAirport Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        public static JObject getCurrency(string locale)
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

                        KKdayApiCommonRQModel RQ = new KKdayApiCommonRQModel()
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
                Website.Instance.logger.FatalFormat($"KKday API getCurrency Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        public static JObject GetProductCountryCity(KKdayApiCommonRQModel RQ)
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



        public static JObject getGuideLang()
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

                        KKdayApiCommonRQModel RQ = new KKdayApiCommonRQModel()
                        {
                            apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                            userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                            ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                            locale = "en",
                            ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"]

                        };

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_GUIDE_LANG"]}";

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
                Website.Instance.logger.FatalFormat($"KKday API getGuideLang Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }


    }
}
