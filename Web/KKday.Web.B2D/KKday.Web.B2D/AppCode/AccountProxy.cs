using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KKday.Web.B2D.BE.App_Code;

namespace KKday.SearchProd.AppCode
{
    public class AccountProxy
    {
        public static string GetUserAccount(string email, string password)
        {
            try
            {
                string jsonResult;
                //Dictionary<string, object> query = new Dictionary<string, object>{
                //    {"email",email},
                //    {"password",password}
                //};

                //建立連線到WMS API

                using (var handler = new HttpClientHandler())
                {
                    // Ignore Certificate Error!!
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                    using (var client = new HttpClient(handler))
                    {
                        #region Uri with QueryStrings

                        //轉換JSON格式
                        //var content = JsonConvert.SerializeObject(query);

                        #endregion Uri with QueryStrings

                        string url = string.Format("{0}{1}",Website.Instance.Configuration["WMS_API:URL"], "Authorize/AuthUser");

                        url += string.Format("?email={0}&password={1}", email, password);

                        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
                        {                                                                                                                                                  
                            //Get使用
                            //request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
