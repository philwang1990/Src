using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using KKday.API.B2S.JTR.Models.Model;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.B2S.JTR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        [HttpPost("Booking")]
        public BookingResponseModel Booking([FromBody]BookingRequestModel bookRQ)
        {
            BookingResponseModel bookRS = new BookingResponseModel();
            Metadata metadata = new Metadata();
            Data data = new Data();

            List<Mappinginfo> info = new List<Mappinginfo>();

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //I.Prod的Mapping
                //bookRQ.pkg_oid+價格別(bookRQ.Price1Qty) 找出訂購JTR的商編
                string jtr_prod_no = "";

                try
                {
                    var xdoc = XDocument.Load(System.AppDomain.CurrentDomain.BaseDirectory + "//App_Data//ProdMapping.xml");
                    if (bookRQ.order.price1Qty > 0)
                    {
                        if (xdoc.Descendants("item").Where(x => x.Element("kkday_prod_pkg").Value.Contains(bookRQ.order.packageOid) && x.Element("kkday_price_type").Value.Equals("price1")).Count() <= 0) throw new Exception("此套餐編號找不到相對應JTR產編，請與BD確認產編再請it修改");


                        jtr_prod_no = xdoc.Descendants("item").Where(x => x.Element("kkday_prod_pkg").Value.Equals(bookRQ.order.packageOid) && x.Element("kkday_price_type").Value.Equals("price1")).
                                                 Select(x => x.Element("jtr_prod_no").Value).FirstOrDefault().ToString();

                        info.Add(new Mappinginfo()
                        {
                            price_type = "price1",
                            qty = bookRQ.order.price1Qty,
                            jtr_prod_no = jtr_prod_no//"10457115"//
                        });

                    }
                    if (bookRQ.order.price2Qty > 0)
                    {
                        if (xdoc.Descendants("item").Where(x => x.Element("kkday_prod_pkg").Value.Contains(bookRQ.order.packageOid) && x.Element("kkday_price_type").Value.Equals("price2")).Count() <= 0) throw new Exception("此套餐編號找不到相對應JTR產編，請與BD確認產編再請it修改");

                        jtr_prod_no = xdoc.Descendants("item").Where(x => x.Element("kkday_prod_pkg").Value.Equals(bookRQ.order.packageOid) && x.Element("kkday_price_type").Value.Equals("price2")).
                                                 Select(x => x.Element("jtr_prod_no").Value).FirstOrDefault().ToString();

                        info.Add(new Mappinginfo()
                        {
                            price_type = "price2",
                            qty = bookRQ.order.price2Qty,
                            jtr_prod_no = jtr_prod_no//"10457115"//
                        });

                    }
                    if (bookRQ.order.price3Qty > 0)
                    {
                        if (xdoc.Descendants("item").Where(x => x.Element("kkday_prod_pkg").Value.Contains(bookRQ.order.packageOid) && x.Element("kkday_price_type").Value.Equals("price3")).Count() <= 0) throw new Exception("此套餐編號找不到相對應JTR產編，請與BD確認產編再請it修改");

                        jtr_prod_no = xdoc.Descendants("item").Where(x => x.Element("kkday_prod_pkg").Value.Equals(bookRQ.order.packageOid) && x.Element("kkday_price_type").Value.Equals("price3")).
                                                 Select(x => x.Element("jtr_prod_no").Value).FirstOrDefault().ToString();

                        info.Add(new Mappinginfo()
                        {
                            price_type = "price3",
                            qty = bookRQ.order.price3Qty,
                            jtr_prod_no = jtr_prod_no//"10457115"//
                        });

                    }
                }
                catch(Exception ex)
                {

                    metadata.status = $"JTR-10002";
                    metadata.description = $"旅客購買的套餐身別不在即訂即付的約定內";
                    bookRS.metadata = metadata;
                    Website.Instance.logger.FatalFormat($"Mapping Error :旅客購買的套餐身別不在即訂即付的約定內");
                    return bookRS;

                }
               

                pay_result payRS = new pay_result();
                order_result bookingRS = new order_result();
                orderDetail_result orderRS = new orderDetail_result();

                List<JtrRsinfo> RS_info = new List<JtrRsinfo>();
                List<Orderinfo> OD_info = new List<Orderinfo>();

                int qty = 1;

                try
                {
                    //II.依照jtr_prod_no數量 呼叫JTR order API 及 pay API
                    foreach (var lst in info)
                    {
                        //訂單資訊！！因商品結構單純 放必須值即可 link_man 為旅客代表人orderCusList得第一位
                        order jtrorder = new order()
                        {
                            info_id = lst.jtr_prod_no ?? string.Empty,
                            cust_id = bookRQ.sup_id ?? string.Empty,
                            order_source_id = info.Count() > 1 ? $"{bookRQ.order.orderMid}-{qty.ToString()}" : bookRQ.order.orderMid,
                            travel_date = bookRQ.order.begLstGoDt,
                            num = lst.qty,
                            link_man = bookRQ.order.orderCusList[0].cusFirstname.ToUpper() + bookRQ.order.orderCusList[0].cusLastname.ToUpper(),
                            link_phone = string.Empty,
                            link_email = Website.Instance.Configuration["CONTENT_EMAIL"] ?? "op-data@kkday.com"
                        };

                        //需replace成UTF-8不然會有error
                        //JTR Booking 
                        string orderRSxmlData = XMLTool.XMLSerialize(jtrorder).Replace("utf-16", "utf-8").Replace("utf - 16", "utf - 8");
                        string bookUrl = $"{Website.Instance.Configuration["JTR_API_URL:ORDER_URL"]}?custId={bookRQ.sup_id}&apikey={bookRQ.sup_key}&param={orderRSxmlData}".Replace("\n","");
                        HttpResponseMessage bookResponse = client.GetAsync(bookUrl).Result;
                        bookingRS = (order_result)XMLTool.XMLDeSerialize(bookResponse.Content.ReadAsStringAsync().Result, bookingRS.GetType().ToString());

                        Website.Instance.logger.Info($"[ORDER]kkOrderNo:{bookRQ.order.orderMid},priceType:{lst.price_type},jtrOrderNo:{bookingRS.order_id},jtrErr:{bookingRS.error_msg}");

                        //JTR Payment
                        string payUrl = $"{Website.Instance.Configuration["JTR_API_URL:PAY_URL"]}?custId={bookRQ.sup_id}&apikey={bookRQ.sup_key}&orderId={bookingRS.order_id}";
                        //string payUrl = "http://jp.jtrchina.com/api/pay.jsp?custId=1588472&apikey=19B2837A1B41535D2E28C4AB7FAB592E&orderId=79860562";

                        HttpResponseMessage payRresponse = client.GetAsync(payUrl).Result;
                        payRS = (pay_result)XMLTool.XMLDeSerialize(payRresponse.Content.ReadAsStringAsync().Result, payRS.GetType().ToString());

                        //Website.Instance.logger.Info($"[PAY]kkOrderNo:{bookRQ.order.orderMid},priceType:{lst.price_type},jtrTktNo:{payRS.code},jtrErr:{payRS.error_msg}");

                        //JTR OrderDetail
                        string orderl = $"{Website.Instance.Configuration["JTR_API_URL:ORDER_DETAIL_URL"]}?custId={bookRQ.sup_id}&apikey={bookRQ.sup_key}&orderId={bookingRS.order_id}";
                        //string orderl = "http://jp.jtrchina.com/api/orderDetail.jsp?custId=1588472&apikey=19B2837A1B41535D2E28C4AB7FAB592E&orderId=79860562";

                        HttpResponseMessage orderRresponse = client.GetAsync(orderl).Result;
                        orderRS = (orderDetail_result)XMLTool.XMLDeSerialize(orderRresponse.Content.ReadAsStringAsync().Result, orderRS.GetType().ToString());

                        Website.Instance.logger.Info($"[DETAIL]kkOrderNo:{bookRQ.order.orderMid},priceType:{lst.price_type},jtUrl:{orderRS.Orders.Order.QrCodeUrl}");

                        RS_info.Add(
                            new JtrRsinfo()
                            {
                                kkOrder_no = jtrorder.order_source_id,
                                kkprice_type = lst.price_type,
                                kkprice_qty = lst.qty,

                                order_id = bookingRS.order_id ?? string.Empty,
                                order_error_msg = bookingRS.error_msg ?? string.Empty,
                                order_error_state = bookingRS.error_state ?? string.Empty,

                                code = payRS.code ?? string.Empty,
                                pay_error_msg = payRS.error_msg ?? string.Empty,
                                pay_error_state = payRS.error_state ?? string.Empty,
                                codeUrls = string.IsNullOrEmpty(orderRS.Orders.Order.QrCodeUrl)? null: orderRS.Orders.Order.QrCodeUrl.Split("http", StringSplitOptions.RemoveEmptyEntries).Select(s => String.Format("http{0}", s.TrimEnd(','))).ToList()
                            });

                        //RS 狀況一
                        //第一筆 訂單成立/付款失敗 直接跳出迴圈  
                        if (qty == 1 && (payRS.error_state != "10000" || (string.IsNullOrEmpty(orderRS.Orders.Order.QrCodeUrl) && string.IsNullOrEmpty(payRS.code)) || bookingRS.error_state != "10000" || string.IsNullOrEmpty(bookingRS.order_id)))
                        {
                            break;
                        }

                        qty++;

                    }

                    //RS 判斷Mapping
                    //result = "OK","OrdErr","PayErr","NoTicket"
                    foreach (var rs in RS_info)
                     {
                        data.isMuiltSupOrder = info.Count() > 1;
                        data.supTicketNumber += !string.IsNullOrEmpty(rs.code) ? rs.code + "<br/>" : "";
                        data.supQrUrl = data.supQrUrl.Add(rs.codeUrls);
                        OD_info.Add(new Orderinfo()
                        {
                            priceType = rs.kkprice_type,
                            qty = rs.kkprice_qty,
                            kkOrderNo = rs.kkOrder_no,
                            ticketNumber = rs.code,
                            QrUrl = rs.codeUrls,
                            result = !(string.IsNullOrEmpty(rs.code)&& string.IsNullOrEmpty(orderRS.Orders.Order.QrCodeUrl)) && rs.order_error_state == "10000" && rs.pay_error_state == "10000" ? "OK" :
                                            (string.IsNullOrEmpty(rs.code) && string.IsNullOrEmpty(orderRS.Orders.Order.QrCodeUrl)) && rs.order_error_state != "10000" && rs.pay_error_state != "10000" ? "OrdErr" :
                                            (string.IsNullOrEmpty(rs.code) && string.IsNullOrEmpty(orderRS.Orders.Order.QrCodeUrl)) && rs.order_error_state == "10000" && rs.pay_error_state != "10000" ? "PayErr" :
                                            (string.IsNullOrEmpty(rs.code) && string.IsNullOrEmpty(orderRS.Orders.Order.QrCodeUrl)) && rs.order_error_state == "10000" && rs.pay_error_state == "10000" ? "NoTicket" : string.Empty
                        });

                    }

                    data.orderinfo = OD_info;
                    bookRS.data = data;

                    //metadata status 跟 描述 mapping
                    //order error
                    string code = "", note = "";

                    if (OD_info.AsEnumerable().Any(x => x.result.Contains("OrdErr")))
                    {
                        //多筆
                        if (data.isMuiltSupOrder && OD_info.Count() > 1)
                        {
                            code = RS_info[1].order_error_state;
                            note = $"1對多訂單類型，其中第1筆{RS_info[0].kkOrder_no}支付成功，{RS_info[0].code}，但第2筆成立訂單失敗，請OP至JTR後台協助確認";
                        }
                        else if (data.isMuiltSupOrder && OD_info.Count() == 1)
                        {
                            code = RS_info[0].order_error_state;
                            note = $"1對多訂單類型，第1筆下單失敗,{RS_info[0].order_error_msg}";
                        }
                        else
                        {
                            code = RS_info[0].order_error_state;
                            note = RS_info[0].order_error_msg;

                        }

                    }
                    else if (OD_info.AsEnumerable().Any(x => x.result.Contains("PayErr")))
                    {
                        //多筆
                        if (data.isMuiltSupOrder && OD_info.Count() > 1)
                        {
                            code = RS_info[1].pay_error_state;
                            note = $"1對多訂單類型，其中第1筆{RS_info[0].kkOrder_no}，{RS_info[0].code}，但第2筆{RS_info[1].kkOrder_no}，{RS_info[1].pay_error_msg}，請OP至JTR後台協助確認";
                        }
                        else if (data.isMuiltSupOrder && OD_info.Count() > 1)
                        {
                            code = RS_info[0].pay_error_state;
                            note = $"1對多訂單類型，第1筆{RS_info[0].kkOrder_no}支付訂單失敗";
                        }
                        else
                        {
                            code = RS_info[0].pay_error_state;
                            note = RS_info[0].pay_error_msg;
                        }

                    }
                    else if (OD_info.AsEnumerable().Any(x => x.result.Contains("NoTicket")))
                    {
                        //多筆
                        if (data.isMuiltSupOrder && OD_info.Count() > 1)
                        {
                            code = "10091";
                            note = $"1對多訂單類型，其中第1筆{RS_info[0].kkOrder_no}交易支付成功{RS_info[0].code}，但第2筆{RS_info[1].kkOrder_no}支付成功未取得12碼供應商訂編或url，請OP至JTR後台協助確認";
                        }
                        else
                        {
                            code = "10091";
                            note = $"{RS_info[0].kkOrder_no}支付成功，但未取得12碼供應商訂編或url，請OP至JTR後台協助確認";

                        }

                    }
                    else 
                    {

                        code = RS_info[0].pay_error_state;
                        note = RS_info[0].pay_error_msg;
                    }


                    metadata.status = $"JTR-{code}";
                    metadata.description = note;

                    bookRS.metadata = metadata;


                }
                catch (Exception ex)
                {
                    //與JTR API串接
                    Website.Instance.logger.FatalFormat($"JTR Connect Error :{ex.Message},{ex.StackTrace}");
                    throw ex;
                }
               
            }
            catch (TimeoutException timeoutex)
            {
                metadata.status = $"JTR-10090";
                metadata.description = timeoutex.Message;
                bookRS.metadata = metadata;

                return bookRS;

            }
            catch (Exception ex)
            {

                metadata.status = $"JTR-00000";
                metadata.description = $"System Error :{ex.Message}";
                bookRS.metadata = metadata;

                return bookRS;

            }

            return bookRS;
        }
    }
}
