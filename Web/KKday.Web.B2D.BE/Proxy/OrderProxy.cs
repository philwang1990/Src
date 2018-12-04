using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.DataModel;
using KKday.Web.B2D.BE.Models.Model.Order;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.Web.B2D.BE.Proxy
{
    public class OrderProxy
    {
        public static string GetOrderList(OrderRQModel rq)
        {
            try
            {
                string jsonResult;

                string uri= Website.Instance.Configuration["WMS_API:URL:GetOrderList"];
                //建立連線到WMS API

                using (var handler = new HttpClientHandler())
                {
                    // Ignore Certificate Error!!
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                    using (var client = new HttpClient(handler))
                    {
                        #region Uri with QueryStrings

                        //轉換JSON格式
                        var content = JsonConvert.SerializeObject(rq);

                        #endregion Uri with QueryStrings

                        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri))
                        {
                            //Get使用
                            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            //Post使用(Add body content)
                            request.Content = new StringContent(content, System.Text.Encoding.UTF8);
                            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                            var response = client.SendAsync(request).Result;
                            jsonResult = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                }
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
