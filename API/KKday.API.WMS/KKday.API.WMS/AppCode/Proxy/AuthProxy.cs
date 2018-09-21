using System;
using System.Collections.Generic;
using System.Net.Http;
using KKday.API.WMS.Models.DataModel.Authorize;
using Newtonsoft.Json;

namespace KKday.API.WMS.AppCode.Proxy
{
    public class AuthProxy
    {
        static HttpClient client = new HttpClient();

        public static GetTokenResponseModel getToke (string account, string password)
        {
            GetTokenResponseModel RS = new GetTokenResponseModel();

            try
            {
                //IS4 application/ x - www - form - urlencoded 這個模式 抓取RQ
                //FormUrlEncodedContent 方法，將其轉換成為具有 application/x-www-form-urlencoded 編碼表示格式。
                client.DefaultRequestHeaders.Accept.Add(
                   new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                //  方法一： 使用字串名稱用法
                var formData = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("client_id", "socialnetwork"),
                    new KeyValuePair<string, string>("client_secret", "secret"),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", account),
                    new KeyValuePair<string, string>("password", password)
                });

                HttpResponseMessage response = client.PostAsync($"{Website.Instance.Configuration["URL:IS4"]}", formData).Result;
                RS = JsonConvert.DeserializeObject<GetTokenResponseModel>(response.Content.ReadAsStringAsync().Result);

            }
            catch(Exception ex)
            {
                Website.Instance.logger.FatalFormat($"getToke  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return RS;
        }
    }
}
