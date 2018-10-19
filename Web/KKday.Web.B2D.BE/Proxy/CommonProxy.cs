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
        public static List<CountryArea> GetCountryList(string locale)
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
                        string url = Website.Instance.Configuration["KKdayAPI:URL:CountryAPI"];

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
    }
}