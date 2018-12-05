using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.SearchProd.AppCode
{
    public class CountryProxy
    {
        public static string Post(string uri, Dictionary<string, object> query)
        {
            try
            {
                string jsonResult;

                //建立連線到WMS API

                using (var handler = new HttpClientHandler())
                {
                    // Ignore Certificate Error!!
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                    using (var client = new HttpClient(handler))
                    {
                        #region Uri with QueryStrings

                        //轉換JSON格式
                        var content = JsonConvert.SerializeObject(query);

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

        public static string Get(string uri, Dictionary<string, object> query)
        {
            try
            {
                string jsonResult;

                //建立連線到WMS API

                using (var handler = new HttpClientHandler())
                {
                    // Ignore Certificate Error!!
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                    using (var client = new HttpClient(handler))
                    {
                        #region Uri with QueryStrings

                        //轉換JSON格式
                        var content = JsonConvert.SerializeObject(query);

                        #endregion Uri with QueryStrings

                        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri))
                        {
                            //Get使用
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            //Post使用(Add body content)
                            //request.Content = new StringContent(content, System.Text.Encoding.UTF8);
                            //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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
