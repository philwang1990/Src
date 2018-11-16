using System;
using System.Net.Http;
using System.Net.Http.Headers;
using KKday.API.WMS.Models.DataModel.Package;
using KKday.API.WMS.Models.DataModel.Product;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Json = KKday.API.WMS.Models.DataModel.Product.Json;

namespace KKday.API.WMS.AppCode.Proxy {
    public class PackageProxy {
        /// <summary>
        /// Gets the package lst.
        /// </summary>
        /// <returns>The package lst.</returns>
        /// <param name="query_lst">Query lst.</param>
        //取套餐列表
        public static JObject getPkgLst(QueryProductModel query_lst) {
            var obj = new JObject();
            try {
                string result = "";
                using (var handler = new HttpClientHandler()) {
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => {
                            return true;
                        };

                    using (var client = new HttpClient(handler)) {
                        client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        KKdayApiProdRQModel RQ = new KKdayApiProdRQModel() {
                            apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                            userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                            ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                            locale = query_lst.locale_lang,
                            currency = query_lst.current_currency,
                            ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"],
                            json = new Json() {
                                pkgStatus = Website.Instance.Configuration["KKAPI_INPUT:JSON:PKG_STATUS"],
                                pkgOid = query_lst.pkg_no,
                                multipricePlatform = Website.Instance.Configuration["KKAPI_INPUT:JSON:MULTIPRICE_PLATFORM"]
                            }

                        };


                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_PKG"]}{query_lst.prod_no}";

                        using (HttpContent content = new StringContent(json_data)) {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;

                        }
                    }

                }
                obj = JObject.Parse(result);

            } catch (Exception ex) {
                Website.Instance.logger.FatalFormat($"getProd  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        /// <summary>
        /// Gets the sale date.
        /// </summary>
        /// <returns>The sale date.</returns>
        /// <param name="query_lst">Query lst.</param>
        //取套餐可販售日期
        public static JObject getSaleDate(QueryProductModel query_lst) {
            var obj = new JObject();
            try {
                string result = "";
                using (var handler = new HttpClientHandler()) {
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => {
                            return true;
                        };

                    using (var client = new HttpClient(handler)) {
                        client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        KKdayApiProdRQModel RQ = new KKdayApiProdRQModel() {
                            apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                            userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                            ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                            locale = query_lst.locale_lang,
                            currency = query_lst.current_currency,
                            ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"],
                            json = new Json() {
                                prodOid = query_lst.prod_no,
                                rtnMonth = Website.Instance.Configuration["KKAPI_INPUT:JSON:RTN_MONTH"],//只出兩個月的可售日期
                           
                            }
                        };

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = Website.Instance.Configuration["URL:KK_PKG_DAY"];

                        using (HttpContent content = new StringContent(json_data)) {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;
                            }
                        }
                    }
                obj = JObject.Parse(result);

            } catch (Exception ex) {
                Website.Instance.logger.FatalFormat($"getProd  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns>The events.</returns>
        /// <param name="query_lst">Query lst.</param>
        //取套餐場次
        public static JObject getEvents(QueryProductModel query_lst) {
            var obj = new JObject();
            try {
                string result = "";
                using (var handler = new HttpClientHandler()) {
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => {
                            return true;
                        };

                    using (var client = new HttpClient(handler)) {
                        client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        KKdayApiProdRQModel RQ = new KKdayApiProdRQModel() {
                            apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"],
                            userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"],
                            ver = Website.Instance.Configuration["KKAPI_INPUT:VER"],
                            locale = query_lst.locale_lang,
                            currency = query_lst.current_currency,
                            ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"],
                            json = new Json() {
                                prodOid = query_lst.prod_no,
                                pkgOid = query_lst.pkg_no,
                                begDt = "",
                                endDt = ""
                             
                            }

                        };

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = Website.Instance.Configuration["URL:KK_PKG_EVENT"];

                        using (HttpContent content = new StringContent(json_data)) {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
                obj = JObject.Parse(result);

            } catch (Exception ex) {
                Website.Instance.logger.FatalFormat($"getProd  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }
    }
}
