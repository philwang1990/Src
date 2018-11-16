using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using KKday.API.WMS.Models.DataModel.Order;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.AppCode.Proxy
{
    public class OrderProxy
    {
        //取得商品分銷商訂單列表
        public static JObject getOrders(QueryOrderModel query_lst)
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

                        KKdayApiOrderRQModel RQ = new KKdayApiOrderRQModel();

                        RQ.apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"];
                        RQ.userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"];
                        RQ.ver = Website.Instance.Configuration["KKAPI_INPUT:VER"];
                        RQ.locale = query_lst.locale_lang;
                        RQ.ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"];

                        Json j = new Json();
                        j.pageSize = query_lst.option.page_size != 0 ? query_lst.option.page_size : 20;//預設20筆
                        j.currentPage = query_lst.option.current_page;
                        j.memberUuid = Website.Instance.Configuration["KKAPI_INPUT:JSON:MEMBER_UUID"];
                        j.deviceId = Website.Instance.Configuration["KKAPI_INPUT:JSON:DEVICE_ID"];
                        j.tokenKey = Website.Instance.Configuration["KKAPI_INPUT:JSON:TOKEN_KEY"];

                        //j.channelOid = query_lst.channel_oid;
                        //j.begCrtDt = query_lst.option.order_Sdate;
                        //j.endCrtDt = query_lst.option.order_Edate;
                        //j.begLstGoDt = query_lst.option.prod_Sdate;
                        //j.endLstGoDt = query_lst.option.prod_Edate;
                        //j.timeZone = query_lst.option.time_zone;
                        //j.orderMids = query_lst.option.kkday_orders.ToArray();

                        RQ.json = j;

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_ORDERS"]}";

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
                Website.Instance.logger.FatalFormat($"getOrders Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        //取得商品分銷商訂單明細
        public static JObject getOrderInfo(QueryOrderModel query_lst,string order_no)
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

                        KKdayApiOrderRQModel RQ = new KKdayApiOrderRQModel();

                        RQ.apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"];
                        RQ.userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"];
                        RQ.ver = Website.Instance.Configuration["KKAPI_INPUT:VER"];
                        RQ.locale = query_lst.locale_lang;
                        RQ.ipaddress = Website.Instance.Configuration["KKAPI_INPUT:IPADDRESS"];

                        Json j = new Json();
                        j.memberUuid = Website.Instance.Configuration["KKAPI_INPUT:JSON:MEMBER_UUID"];
                        j.deviceId = Website.Instance.Configuration["KKAPI_INPUT:JSON:DEVICE_ID"];
                        j.tokenKey = Website.Instance.Configuration["KKAPI_INPUT:JSON:TOKEN_KEY"];

                        RQ.json = j;

                        string json_data = JsonConvert.SerializeObject(RQ);
                        string url = $"{Website.Instance.Configuration["URL:KK_ORDER_INFO"]}{order_no}";

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
                Website.Instance.logger.FatalFormat($"getOrderInfo Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }
    }
}
