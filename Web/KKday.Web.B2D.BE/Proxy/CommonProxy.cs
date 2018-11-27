using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.Web.B2D.BE.Commons
{
    public class CommonProxy
    {

        // 取得國家與國碼
        public static List<CountryArea> GetCountryAreas(string locale)
        {
            List<CountryArea> country_list = new List<CountryArea>();

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
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        Dictionary<string, string> _params = new Dictionary<string, string>() {
                            { "apiKey", Website.Instance.Configuration["KKdayAPI:Body:ApiKey"]},
                            { "userOid", Website.Instance.Configuration["KKdayAPI:Body:ApiKey"]},
                            { "ver", Website.Instance.Configuration["KKdayAPI:Body:Ver"]},
                            { "ipaddress", Website.Instance.Configuration["KKdayAPI:Body:IPAddress"]},
                            { "locale", locale }
                            //currency =  _currency, 
                        };

                        string json_data = JsonConvert.SerializeObject(_params);
                        string url = Website.Instance.Configuration["KKdayAPI:URL:CountryAreaAPI"];

                        using (HttpContent content = new StringContent(json_data))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;

                            // HTTP Status Code須為"OK", 否則與API串接失敗 
                            if (response.StatusCode.ToString() != "OK")
                            {
                                throw new Exception(response.Content.ReadAsStringAsync().Result);
                            }
                        }
                    }

                }
                 
                JObject jsonObject = JObject.Parse(result);

                foreach(JToken item in jsonObject["content"]["countryList"].AsJEnumerable())
                {
                    country_list.Add(new CountryArea(){
                        telArea = item["country"]["telArea"].ToString(),
                        countryCode = item["country"]["countryCd"].ToString(),
                        countryName = item["country"]["countryName"].ToString(),
                        countryEngName = item["country"]["countryEngName"].ToString(),
                    });
                }
              
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API getCodeLang  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return country_list;
        }

        // 取得國家與語系
        public static List<CountryLocale> GetCountryLocales()
        {
            List<CountryLocale> locales = new List<CountryLocale>();

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
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        Dictionary<string, string> _params = new Dictionary<string, string>() {
                            { "apiKey", Website.Instance.Configuration["KKdayAPI:Body:ApiKey"]},
                            { "userOid", Website.Instance.Configuration["KKdayAPI:Body:ApiKey"]},
                            { "ver", Website.Instance.Configuration["KKdayAPI:Body:Ver"]},
                            { "ipaddress", Website.Instance.Configuration["KKdayAPI:Body:IPAddress"]},
                            { "locale", "en" } 
                        };

                        string json_data = JsonConvert.SerializeObject(_params);
                        string url = Website.Instance.Configuration["KKdayAPI:URL:CountryLangAPI"];

                        using (HttpContent content = new StringContent(json_data))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;

                            // HTTP Status Code須為"OK", 否則與API串接失敗 
                            if (response.StatusCode.ToString() != "OK")
                            {
                                throw new Exception(response.Content.ReadAsStringAsync().Result);
                            }
                        }
                    }

                } 

                JObject jsonObject = JObject.Parse(result);

                foreach (JToken item in jsonObject["content"]["countryLangList"].AsJEnumerable())
                {
                    locales.Add(new CountryLocale()
                    {
                        countryCode = item["countryCd"].ToString(),
                        countryName = item["countryName"].ToString(),
                        localeCode = item["langList"][0]["langCd"].ToString(),
                        localeName = item["langList"][0]["langName"].ToString()
                    });

                }

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API getCodeLang  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return locales;
        } 

        // 取得幣別
        public static Dictionary<string, string> GetCurrencies(string locale)
        {
            try
            {
                Dictionary<string, string> currency_dict = new Dictionary<string, string>();
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
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        string url = Website.Instance.Configuration["WMS_API:URL:CurrencyAPI"];
                        url += "?locale=" + locale;

                        var response = client.GetAsync(url).Result;
                        result = response.Content.ReadAsStringAsync().Result;

                        // HTTP Status Code須為"OK", 否則與API串接失敗 
                        if (response.StatusCode.ToString() != "OK")
                        {
                            throw new Exception(response.Content.ReadAsStringAsync().Result);
                        }
                    }

                }

                JObject jsonObject = JObject.Parse(result);
                foreach (JToken item in jsonObject["currencyList"].AsJEnumerable())
                {
                    if (!currency_dict.ContainsKey(item["currency"].ToString()))
                    {
                        currency_dict.Add(item["currency"].ToString(), item["name"].ToString());
                    }
                }

                return currency_dict;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API getCodeLang  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

        }

        // 取得APItoken
        public static GetTokenResponseModel GetApiToken(string account,string password)
        {
            HttpClient client = new HttpClient();
            GetTokenResponseModel RS = new GetTokenResponseModel();

            try
            {
                //IS4 application/ x - www - form - urlencoded 這個模式 抓取RQ
                //FormUrlEncodedContent 方法，將其轉換成為具有 application/x-www-form-urlencoded 編碼表示格式。
                client.DefaultRequestHeaders.Accept.Add(
                   new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                //  方法一： 使用字串名稱用法
                var formData = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("client_id", "KKDAY_B2D"),
                new KeyValuePair<string, string>("client_secret", "secret"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", account),
                new KeyValuePair<string, string>("password", password)
            });

            HttpResponseMessage response = client.PostAsync($"{Website.Instance.Configuration["WMS_API:URL:GetApiToken"]}", formData).Result;
                RS = JsonConvert.DeserializeObject<GetTokenResponseModel>(response.Content.ReadAsStringAsync().Result);

            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"getToke  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return RS;
        }

    }
}