using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using KKday.API.B2S.Gateway.Models.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KKday.API.B2S.Gateway.Controllers
{
    [Route("api/B2S/[controller]")]
    [ApiController]
    public class RouterController : Controller
    {

        [HttpPost]
        public string Post([FromBody]BookingRequestModel bookRQ)
        {
            BookingResponseModel bookRS = new BookingResponseModel();

            string result = "";

            HttpClientHandler handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };

            HttpClient client = new HttpClient(handler);

            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var xdoc = XDocument.Load(System.AppDomain.CurrentDomain.BaseDirectory + "//App_Data//RouteMapping.xml");

                if (xdoc.Descendants("item").Where(x => x.Element("kkday_pkg_oid").Value.Contains(bookRQ.order.packageOid)).Count() <= 0) 
                {
                    Metadata metadata = new Metadata();
                    metadata.status = "JTR-10002";
                    metadata.description = $"此套餐編號找不到串接的供應商，且不再即訂即付的商品項目中";
                    bookRS.metadata = metadata;

                    return Json(bookRS);

                }


                string sup = xdoc.Descendants("item").Where(x => x.Element("kkday_pkg_oid").Value.Contains(bookRQ.order.packageOid)).
                                            Select(x => x.Element("sup").Value).FirstOrDefault().ToString();
                string sup_url = Website.Instance.Configuration[$"{sup}:BOOK_URL"];
                string sup_id = Website.Instance.Configuration[$"{sup}:SUP_ID"];
                string sup_key = Website.Instance.Configuration[$"{sup}:SUP_KEY"];

                Website.Instance.logger.Info($"Booking URL :{sup_url}");

                bookRQ.sup_id = sup_id;
                bookRQ.sup_key = sup_key;

                string json = JsonConvert.SerializeObject(bookRQ);

                var contentData = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(sup_url, contentData).Result;

                result = response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception ex)
            {

                Metadata metadata = new Metadata();
                metadata.status = "JTR-10002";
                metadata.description = $"Gateway Error :{ex.Message}";
                bookRS.metadata = metadata;

            }

            return result;
        }

    }

}