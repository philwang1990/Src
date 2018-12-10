using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using KKday.Web.B2D.BE.App_Code;
using KKday.Web.B2D.BE.Models.Model.Package;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.Web.B2D.BE.Proxy
{
    public class PackageProxy
    {
        public static List<KKdayPackage> GetProdPackages(Int64 comp_xid, string locale, string currency, string prod_no, string state)
        {
            List<KKdayPackage> pkg_list = new List<KKdayPackage>();

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
                            { "company_xid",  comp_xid.ToString() },
                            { "locale_lang",  locale },
                            { "current_currency", currency },
                            { "prod_no", prod_no },
                            { "state", state }
                        };
                       
                        string json_data = JsonConvert.SerializeObject(_params);
                        string url =string.Format("{0}{1}",Website.Instance.Configuration["WMS_API:URL"],"Product/Querypackage");

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
                pkg_list = ((JArray)jsonObject["pkgs"]).ToObject<List<KKdayPackage>>();
            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"KKday API getCodeLang  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return pkg_list;
        }
    }
}
