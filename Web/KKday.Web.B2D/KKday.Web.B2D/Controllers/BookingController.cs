using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKday.Web.B2D.EC.AppCode;
using Microsoft.AspNetCore.Mvc;
using KKday.Web.B2D.EC.Models.Model.Product;
using KKday.Web.B2D.EC.Models.Repostory.Product;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KKday.Web.B2D.EC.Models.Model.Booking;
using KKday.Web.B2D.EC.Models.Repostory.Booking;
using static KKday.Web.B2D.EC.Controllers.ProductController;
using KKday.Web.B2D.EC.Models;
using System.Diagnostics;
using KKday.Web.B2D.EC.Models.Model.Pmch;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using KKday.Web.B2D.EC.Models.Repostory.Common;
using System.Security.Claims;
using KKday.Web.B2D.EC.Models.Model.Account;
using Microsoft.AspNetCore.Authorization;
using KKday.Web.B2D.EC.Models.Model.UserAgent;
using KKday.Web.B2D.BE.App_Code;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.EC.Controllers
{
    [Authorize(Policy = "UserOnly")]
    public class BookingController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private static IRedisHelper RedisHelper;

        public BookingController(IHttpContextAccessor _httpContextAccessor, IRedisHelper _redisHelper)
        {
            httpContextAccessor = _httpContextAccessor;
            RedisHelper = _redisHelper;
        }

        // GET: /<controller>/
        public IActionResult Index(string guid)
        {
            try
            {
                //B2d分銷商資料
                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                string ip = httpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString().Replace("::1", "127.0.0.1");

                //取挖字
                Dictionary<string, string> uikey = CommonRepostory.getuiKey(RedisHelper, UserData.LOCALE);// RedisHelper.getuiKey(fakeContact.lang);
                ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(JsonConvert.SerializeObject(uikey));

                if (guid == null) throw new Exception(title.common_data_error);

                confirmPkgInfo confirm = JsonConvert.DeserializeObject<confirmPkgInfo>(RedisHelper.getRedis("bid:ec:confirm:" + guid));
                if (confirm == null) throw new Exception(title.common_data_error);

                //從 api取 
                ProductModuleModel module = ProductRepostory.getProdModule( UserData.COMPANY_XID, UserData.COUNRTY_CODE, UserData.LOCALE, UserData.CURRENCY, confirm.prodOid, confirm.pkgOid, title);
                ProductModel prod = ProductRepostory.getProdDtl(UserData.COMPANY_XID, UserData.COUNRTY_CODE, UserData.LOCALE, UserData.CURRENCY, confirm.prodOid, title);
                PackageModel pkgs = ProductRepostory.getProdPkg(UserData.COMPANY_XID, UserData.COUNRTY_CODE, UserData.LOCALE, UserData.CURRENCY, confirm.prodOid, title);

                if (prod.result != "0000")
                {
                    Website.Instance.logger.Debug($"booking_index_getProdDtl_err:prodOid->{confirm.prodOid} ,msg-> {prod.result_msg}");
                    throw new Exception(title.result_code_9990);
                }
                if (pkgs.result != "0000")
                {
                    Website.Instance.logger.Debug($"booking_index_getProdPkg_err:prodOid->{confirm.prodOid},pkgOid ->{confirm.pkgOid} ,msg-> {prod.result_msg}");
                    throw new Exception(title.result_code_9990);
                }

                string flightInfoType = "";
                string sendInfoType = "";
                PkgDetailModel pkg = null;
                PkgEventsModel pkgEvent = null;
                CusAgeRange cusAgeRange = null;
                string isEvent = "N";
                string isHl = "N";
                var pkgsTemp = pkgs.pkgs.Where(x => x.pkg_no == confirm.pkgOid).ToList();
                if (pkgsTemp.Count() > 0)
                {
                    foreach (PkgDetailModel p in pkgsTemp)
                    {
                        pkg = p;
                        flightInfoType = p.module_setting.flight_info_type.value;
                        sendInfoType = p.module_setting.send_info_type.value;
                        cusAgeRange = BookingRepostory.getCusAgeRange(confirm, p);

                        isEvent = p.is_event;
                        isHl = p.is_hl;
                    }
                }
                else
                {
                    //丟錯誤頁
                    Website.Instance.logger.Debug($"booking_index_err:商編->{confirm.prodOid}即有pkgs找不到對應的pkgoid->{ confirm.pkgOid}");
                    throw new Exception(title.common_data_error);
                }

                //如果有event 但沒有傳 event id ,就error
                if (isEvent == "Y" && string.IsNullOrEmpty(confirm.pkgEvent)) throw new Exception(title.common_data_error);

                if (isEvent == "Y")
                {
                    pkgEvent = ApiHelper.getPkgEvent(UserData.COMPANY_XID, UserData.COUNRTY_CODE, UserData.LOCALE, UserData.CURRENCY, confirm.prodOid, confirm.pkgOid, title);
                }

                //pmgw
                PmchLstResponse pmchRes = ApiHelper.getPaymentListRes(prod.countries, prod.prod_no.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"),
                DateTimeTool.yyyyMMdd2DateTime(confirm.selDate).ToString("yyyy-MM-dd"), DateTimeTool.yyyyMMdd2DateTime(confirm.selDate).ToString("yyyy-MM-dd"), UserData.COUNRTY_CODE, UserData.LOCALE,
                                                                    prod.prod_type, ip, prod.prod_hander, UserData.CURRENCY, title);

                Pmgw pmgw = null;
                if (UserData.CURRENCY == "TWD")
                {
                    pmgw = pmchRes.pmchlist.Where(x => x.acctdocReceiveMethod == "ONLINE_CITI" && x.pmchCode == "B2D_CITI_TWD").FirstOrDefault();
                }
                else
                {
                    pmgw = pmchRes.pmchlist.Where(x => x.acctdocReceiveMethod == "ONLINE_HK_ADYEN").FirstOrDefault();
                }
                //必須要設定人數
                //var cusData = BookingRepostory.getCusDdate();
                int totalCus = 0;
                if (module.module_cust_data != null)
                {
                    if (module.module_cust_data.is_require == true) totalCus = (module.module_cust_data.cus_type == "01") ? 1 : Convert.ToInt32(confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty);
                }

                //滿足國家
                List<Country> country = prod.countries;
                string nationName ="";
                if (country.Count > 0) nationName= country[0].name;

                //將dataModel原型 以json str 帶到前台的hidden
                DataModel dm = DataSettingRepostory.getDefaultDataModel(totalCus, guid);
                dm.guidNo = guid;
                String dataModelStr = JsonConvert.SerializeObject(dm);
                //dm.travelerData[0].meal.mealType
                ViewData["dataModelStr"] = dataModelStr;

                VenueInfo    venue = module.module_venue_info;
                if (venue == null) { venue = new VenueInfo(); venue.is_require = false; }
                RentCar rentCar = module.module_rent_car;
                if (rentCar == null) { rentCar = new RentCar(); rentCar.is_require = false; }
                ViewData["confirmPkgInfo"] = confirm;
                ViewData["contactInfo"] = UserData;
                ViewData["cusData"] = module.module_cust_data;
                ViewData["guide"] = module.module_guide_lang_list;
                ViewData["wifi"] = module.module_sim_wifi;
                ViewData["exchange"] = module.module_exchange_location_list;
                ViewData["flightInfo"] = module.module_flight_info;
                ViewData["venue"] = venue;// module.module_venue_info;
                ViewData["useDate"] = DateTimeTool.yyyy_mm_dd(confirm.selDate);//DateTimeTool.yyyy_mm_dd(); 
                ViewData["rentCar"] = rentCar;// module.module_rent_car;
                ViewData["carPsgr"] = module.module_car_pasgr; //車輛資料
                ViewData["sendData"] = module.module_send_data;
                ViewData["contactData"] = module.module_contact_data;
                ViewData["nationName"] = nationName;

                ViewData["guid"] = guid;
                ViewData["prodTitle"] = title;
                ViewData["totalCus"] = totalCus;
                ViewData["mainCat"] = prod.prod_type;
                ViewData["flightInfoType"] = flightInfoType;
                ViewData["sendInfoType"] = sendInfoType;
                ViewData["CutOfDay"] = prod.before_order_day;
                ViewData["cusAgeRange"] = cusAgeRange;
                BookingShowProdModel show = BookingRepostory.setBookingShowProd(prod, pkg, confirm,UserData.CURRENCY, pkgEvent, title);
                ViewData["prodShow"] = show;

                ViewData["isEvent"] = isEvent;//
                ViewData["isHl"] = isHl; //如果是N就不用做
                ViewData["pkgCanUseDate"] = (isHl == "Y" && isEvent == "Y") ? BookingRepostory.getPkgEventDate(pkgEvent, confirm.pkgOid, (confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty)) : "";//要把這個套餐可以用的日期全抓出來
                ViewData["pmgw"] = pmgw;

                //放到session
                TempData["prod_" + guid] = JsonConvert.SerializeObject(prod);
                TempData["pkgEvent_" + guid] = (isHl == "Y" && isEvent == "Y") ? JsonConvert.SerializeObject(pkgEvent) : "";
                TempData["module_" + guid] = JsonConvert.SerializeObject(module);
                TempData["confirm_" + guid] = JsonConvert.SerializeObject(confirm);
                TempData["ProdTitleKeep_" + guid] = JsonConvert.SerializeObject(title);
                TempData["pkg_" + guid] = JsonConvert.SerializeObject(pkg);
                TempData["pkgsDiscRule_" + guid] = JsonConvert.SerializeObject(pkgs.discount_rule);
                TempData["prodShow_" + guid] = JsonConvert.SerializeObject(show);
                TempData["pmgw_" + guid] = JsonConvert.SerializeObject(pmgw);

                return View();
            }
            catch (Exception ex)
            {
                ViewData["errMsg"] = ex.Message.ToString();
                Website.Instance.logger.Debug($"booking_index_err:{ex.Message.ToString()}");
                //導到錯誤頁
                return RedirectToAction("Index", "Error", new ErrorViewModel { ErrorType = ErrorType.Invalid_Common });
            }
        }


        //取得候補的場次
        [HttpPost]
        public IActionResult getEvent([FromBody] EventQury Eventday)
        {
            returnBookingEventStatus status = new returnBookingEventStatus();

            try
            {
                List<string> dayevent = new List<string>();
                string day = Eventday.day.Replace("-", "");

                string titleJson = (string)TempData["ProdTitleKeep_" + Eventday.guid];
                string pkgEventJson = (string)TempData["pkgEvent_" + Eventday.guid];
                string confirmJson = (string)TempData["confirm_" + Eventday.guid];

                if (string.IsNullOrEmpty(titleJson)) { throw new Exception("資料錯誤，請重新讀取頁面"); }
                if (string.IsNullOrEmpty(pkgEventJson)) { throw new Exception("資料錯誤，請重新讀取頁面"); }
                if (string.IsNullOrEmpty(confirmJson)) { throw new Exception("資料錯誤，請重新讀取頁面"); }

                ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(titleJson);
                PkgEventsModel pkgEvent = JsonConvert.DeserializeObject<PkgEventsModel>(pkgEventJson);
                confirmPkgInfo confirm = JsonConvert.DeserializeObject<confirmPkgInfo>(confirmJson);
                var eTemp = pkgEvent.events.Where(x => x.day.Equals(day));

                TempData.Keep();

                int? BookingQty = (confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty);
                if (eTemp.Count() > 0)
                {
                    foreach (Event e in eTemp)
                    {
                        string[] eventTime = e.event_times.Split(',');

                        foreach (string s in eventTime)
                        {
                            string id = s.Split("_")[0];
                            int Qty = Convert.ToInt32(s.Split("_")[2]);

                            if (Qty >= BookingQty && ((day != confirm.selDate) || (day == confirm.selDate && confirm.pkgEvent != id))) dayevent.Add(s);
                        }
                    }

                    status.status = "OK";
                    status.msgErr = "";
                    status.dayevent = dayevent;
                    return Json(status);
                }
                else
                {
                    //再補回傳的格式
                    status.status = "FAIL";
                    status.msgErr = title.product_index_no_event_avalible;
                    return Json(status);
                }
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"bookingStep1_getEventerr:eventTime->{JsonConvert.SerializeObject(Eventday)} ,{ex.ToString()}");
                //再補回傳的格式
                status.status = "FAIL";
                status.msgErr = "資料錯誤，請重新讀取頁面";
                return Json(status);
            }
        }


        [HttpPost]
        public IActionResult bookingStep1([FromBody]DataModel data)
        {
            try
            {
                string memUuid = Website.Instance.Configuration["kkdayKey:uuid"];

                string userAgent = Request.Headers["User-Agent"].ToString();
                UserAgent ua = new UserAgent(userAgent);

                //B2d分銷商資料
                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                string ip = httpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString().Replace("::1", "127.0.0.1");
                data = BookingRepostory.setCardEncrypt(data);
                //log時把卡號移除
                DataModel dataTemp = data.Clone();
                dataTemp.card = null;
                Website.Instance.logger.Debug($"bookingStep1_inputdata:{ JsonConvert.SerializeObject(dataTemp)}");

                string prodStr = TempData["prod_" + data.guidNo] as string;
                if (string.IsNullOrEmpty(prodStr)) { throw new Exception("資料錯誤，請重新讀取頁"); }
                ProductModel prod = JsonConvert.DeserializeObject<ProductModel>(prodStr);

                string moduleStr = TempData["module_" + data.guidNo] as string;
                if (string.IsNullOrEmpty(moduleStr)) { throw new Exception("資料錯誤，請重新讀取頁"); }
                ProductModuleModel module = JsonConvert.DeserializeObject<ProductModuleModel>(moduleStr);

                string pkgStr = TempData["pkg_" + data.guidNo] as string;
                if (string.IsNullOrEmpty(pkgStr)) { throw new Exception("資料錯誤，請重新讀取頁"); }
                PkgDetailModel pkg = JsonConvert.DeserializeObject<PkgDetailModel>(pkgStr);

                string pkgConfirmStr = TempData["confirm_" + data.guidNo] as string;
                if (string.IsNullOrEmpty(moduleStr)) { throw new Exception("資料錯誤，請重新讀取頁"); }
                confirmPkgInfo confirm = JsonConvert.DeserializeObject<confirmPkgInfo>(pkgConfirmStr);

                string titleStr = TempData["ProdTitleKeep_" + data.guidNo] as string;
                if (string.IsNullOrEmpty(moduleStr)) { throw new Exception("資料錯誤，請重新讀取頁"); }
                ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(titleStr);

                string discRuleStr = TempData["pkgsDiscRule_" + data.guidNo] as string;
                if (string.IsNullOrEmpty(discRuleStr)) { throw new Exception("資料錯誤，請重新讀取頁"); }
                DiscountRuleModel rule = JsonConvert.DeserializeObject<DiscountRuleModel>(discRuleStr);

                string pmgwStr = TempData["pmgw_" + data.guidNo] as string;
                if (string.IsNullOrEmpty(pmgwStr)) { throw new Exception("資料錯誤，請重新讀取頁"); }
                Pmgw pmgw = JsonConvert.DeserializeObject<Pmgw>(pmgwStr);

                string showStr = TempData["prodShow_" + data.guidNo] as string;
                if (string.IsNullOrEmpty(showStr)) { throw new Exception("資料錯誤，請重新讀取頁"); }
                //BookingShowProdModel show = JsonConvert.DeserializeObject<BookingShowProdModel>(showStr);

                TempData.Keep();
                data = BookingRepostory.setDefaultBookingInfo(memUuid,ua, data, prod, pkg, confirm, UserData, pmgw);

                //排除餐食 
                data = BookingRepostory.exculdeFood(prod, data, module);

                data.company_xid = UserData.COMPANY_XID.ToString();
                data.channel_oid = UserData.KKDAY_CHANNEL_OID;
                data.locale = UserData.LOCALE;
                data.ip = ip;
                JObject order = ApiHelper.orderNew(data, title);

                string orderMid = "";
                string orderOid = "";
                returnStatus status = new returnStatus();

                Website.Instance.logger.Debug($"bookingStep1_ordernewresponse:" + JsonConvert.SerializeObject(order));//要改
                //要先判斷是不是result＝'0000'
                if (order["result"].ToString() == "0000")
                {
                    orderMid = order["order_mid"].ToString();
                    orderOid = order["order_oid"].ToString();
                    //upd B2bOrder
                    //BookingRepostory.updB2dOrder(UserData.COMPANY_XID, orderOid, orderMid, b2bOrder, title);

                    status.pmchSslRequest = BookingRepostory.setPaymentInfo2(prod, data, orderMid, UserData, pmgw, memUuid,ip);
                    status.status = "OK";
                    status.url = Website.Instance.Configuration["kkUrl:pmchUrl"].ToString() + pmgw.pmchPayURL; //pmchUrl

                    //要把BookingShowProdModel 帶到訂購final頁
                    RedisHelper.SetRedis(showStr, "b2d:ec:order:final:prodShow:" + orderMid, 60);
                    RedisHelper.SetRedis(JsonConvert.SerializeObject(data), "b2d:ec:order:final:orderData:" + orderMid, 60);

                    //要存redis 付款主要資訊，最後訂單 upd時要使用,可和下面整合存一個就
                    BookingRepostory.setPayDtltoRedis(data, orderMid, UserData.UUID, RedisHelper);

                    //要存redis 因為付款後要從這個redis內容再進行payment驗證,可和上面整合存一個就好
                    //CallJsonPay rdsJson = (CallJsonPay)status.pmchSslRequest.json;
                    CallJsonPay2 rdsJson = (CallJsonPay2)status.pmchSslRequest.json;
                    string callPmchReq = JsonConvert.SerializeObject(status.pmchSslRequest.json);
                    RedisHelper.SetRedis(callPmchReq, "b2d:ec:pmchSslRequest:" + orderMid, 60);
                }
                else
                {
                    status.status = "Error";
                    status.msgErr = order["content"]["result"].ToString() + order["content"]["msg"].ToString();//要改
                }

                return Json(status);
                //v1/channel/citi/auth
                //https://pmch.sit.kkday.com/citi/payment/auth
                //https://payment.kkday.com/v1/channel/adyen/auth
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"bookingStep1_err_ordernew失敗:{ex.Message.ToString()}");
                ViewData["errMsg"] = ex.Message.ToString();
                Website.Instance.logger.Debug($"booking_index_err:{ex.Message.ToString()}");

                //導到錯誤頁
                return RedirectToAction("Index", "Error", new ErrorViewModel { ErrorType = ErrorType.Order_Fail ,ErrorMessage= ex.Message.ToString() });
            }
        }
    }
}
