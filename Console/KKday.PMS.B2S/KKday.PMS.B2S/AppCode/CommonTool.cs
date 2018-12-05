using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.PMS.B2S.AppCode
{
    public class CommonTool
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly static ILog _log = LogManager.GetLogger(typeof(Program));

        public static void LoadLog4netConfig()
        {
            var repository = LogManager.CreateRepository(
                                Assembly.GetEntryAssembly(),
                                typeof(log4net.Repository.Hierarchy.Hierarchy)
                            );
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            //_log.Info("Application Start ------");
        }


        public static string GetData(string url)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                 delegate (object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                 { return true; };

                string result;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";


                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //result = "{\"reasult\":\"0000\",\"reasult_msg\":\"正確\",\"prod_type_name\":\"一日旅遊\",\"main_lang\":\"zh-tw\",\"cost_type\":\"NET\",\"prod_desction\":\"詳細內容\",\"prod_tips\":\"\",\"prod_map_note\":\"備註( to traveler)\",\"is_search\":\"Y\",\"apply_status\":\"4\",\"status\":\"00\",\"policy_no\":\"2\",\"policy_list\":[],\"is_tour\":\"N\",\"tour_list\":[],\"meal_list\":[],\"guide_lang_list\":[{\"lang_code\":\"en\",\"lang_name\":\"English\"},{\"lang_code\":\"ja\",\"lang_name\":\"日本語\"},{\"lang_code\":\"zh-tw\",\"lang_name\":\"中文\"}],\"arr_map_info_list\":[{\"photo_url\":\"//s1.kkday.com/images/product/20159/location/20181004014711_vKhFt.png\",\"photo_desc\":\"\",\"zoom\":14,\"latitude\":\"25.109187\",\"longitude\":\"121.8462979\",\"latlong_type\":\"ARR\",\"latlong_desc\":\"九份\"}],\"confirm_order_time\":24,\"online_s_date\":\"20181004000000\",\"online_e_date\":\"20190630000000\",\"before_order_day\":\"2\",\"img_list\":[{\"auth_name\":\"                                                                                                                                                                                                        \",\"is_main_img\":\"Y\",\"img_desc\":\"\",\"img_url\":\"//s1.kkday.com/images/product/20159/20181004014454_pvYSh.jpg\",\"img_kkday_url\":\"/image/get/s1.kkday.com/product_20159/20181004014454_pvYSh/jpg\",\"is_auth_cc\":\"N\",\"is_commerce\":\"Y\",\"share_type\":\"A\"}],\"finishStep\":\"PO,PT,PD,PS,PP,PC,SC,PG,CD,77568\",\"prod_comment_info\":{\"avg_scores\":\"0\",\"total_scores\":\"0\",\"click_count\":\"0\",\"comment_record\":\"0\",\"keyword\":\"水金九,金瓜石,九份\",\"sales_qty\":\"0\",\"prod_url_oid\":\"20159\"},\"order_email\":\"phil651105@gmail.com\",\"remind_list\":[{\"remind_desc\":\"當參加人數未達最少出團人數之2人時，將於使用日前1天發出取消旅遊的email通知。\"},{\"remind_desc\":\"因交通、天氣等不可抗力因素所引起的時間延誤，造成部份行程景點取消時，請您主動聯絡客服，我們將會為您辦理部份退款。\"},{\"remind_desc\":\"不建議患有下列疾病或其他不宜受到過度刺激的遊客參加此項目：高血壓,心臟病\"},{\"remind_desc\":\"落石\"},{\"remind_desc\":\"雷擊\"}],\"video_list\":[],\"cost_detail_list\":[{\"detail_desc\":\"費用不包含\",\"detail_type\":\"NO_INC\"},{\"detail_desc\":\"費用不包含2\",\"detail_type\":\"NO_INC\"},{\"detail_desc\":\"費用包含\",\"detail_type\":\"INC\"},{\"detail_desc\":\"費用包含2\",\"detail_type\":\"INC\"}],\"tkt_expire\":{\"exp_type\":\"\",\"exp_open_date\":\"\",\"exp_s_date\":\"\",\"exp_e_date\":\"\"},\"meeting_point_list\":[],\"voucher_locations\":[],\"voucher_desc\":\"\",\"prod_mkt\":{\"is_ec_show\":true,\"is_ec_sale\":true,\"purchase_type\":\"\",\"purchase_type_name\":\"\",\"is_search\":true,\"is_show\":true},\"prod_no\":20159,\"prod_name\":\"浪漫水金九 （正）\",\"b2c_price\":210.0,\"b2d_price\":204.0,\"prod_currency\":\"TWD\",\"prod_img_url\":\"浪漫水金九 （正）\",\"introduction\":\"商品概述\",\"prod_type\":\"M01\",\"tag\":[\"TAG_1_3\",\"TAG_3_1\",\"TAG_3_3\"],\"countries\":[{\"id\":\"A01-001\",\"name\":\"台灣\",\"cities\":[{\"id\":\"A01-001-00002\",\"name\":\"台中\"},{\"id\":\"A01-001-00001\",\"name\":\"台北\"}]}]}";
                //ProductforEcModel obj = JsonConvert.DeserializeObject<ProductforEcModel>(result);

                return result;

            }
            catch (Exception ex)
            {
                //Website.Instance.logger.Debug($"apiHelpler_getProdDtl_err:{ JsonConvert.SerializeObject(ex.Message.ToString())}");
                //throw new Exception(title.result_code_9990);
                throw ex;
            }
        }

        public static JObject GetDataPost(string url, string json_data)
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


                        //string json_data = JsonConvert.SerializeObject(RQ);
                        //string url = $"{Website.Instance.Configuration["URL:KK_MODEL"]}".Replace("{prod_no}", query_lst.prod_no);

                        using (HttpContent content = new StringContent(json_data))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            var response = client.PostAsync(url, content).Result;
                            result = response.Content.ReadAsStringAsync().Result;

                            //Website.Instance.logger.Info($"URL:{url},URL Response StatusCode:{response.StatusCode}");

                            //與API串接失敗 
                            if (response.StatusCode.ToString() != "OK")
                            {
                                throw new Exception(response.Content.ReadAsStringAsync().Result);
                            }
                            else
                            {

                                //rds.SetProdInfotoRedis(result, "bid:test:KKdayApi_getModule" + query_lst.b2d_xid);

                            }
                        }

                    }

                }

                //}

                obj = JObject.Parse(result);

            }
            catch (Exception ex)
            {
                //Website.Instance.logger.FatalFormat($"KKday API getMoudle Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return obj;
        }

        public static async Task<string> GetDataNew(string url)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                 delegate (object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                 { return true; };

                string result;

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "GET";

                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }

                return result;

            }
            catch (Exception ex)
            {
                _log.Error($"GetData:{ JsonConvert.SerializeObject(ex.Message)}");
                throw ex;
            }
        }
    }
}
