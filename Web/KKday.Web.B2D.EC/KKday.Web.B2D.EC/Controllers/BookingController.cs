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
//using KKday.Web.B2D.EC.Models.Repostory.Booking;
using Microsoft.Extensions.Primitives;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.EC.Controllers
{
    public class BookingController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(string guid)
        {
            try
            {
                //假分銷商
                distributorInfo fakeContact = DataSettingRepostory.fakeContact();

                //取挖字
                Dictionary<string, string> uikey = RedisHelper.getuiKey(fakeContact.lang);
                ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(JsonConvert.SerializeObject(uikey));

                if (guid == null) throw new Exception(title.common_data_error);

                confirmPkgInfo confirm = JsonConvert.DeserializeObject<confirmPkgInfo>(RedisHelper.getProdInfotoRedis("bid:ec:confirm:" + guid));
                if (confirm ==null) throw new Exception(title.common_data_error);

                //從 api取 
                ProductModuleModel module = ProductRepostory.getProdModule(fakeContact.companyXid,fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid,confirm.pkgOid,title);
                ProductModel prod = ProductRepostory.getProdDtl(fakeContact.companyXid,fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid,title);
                PackageModel pkgs = ProductRepostory.getProdPkg(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid,title);

                if (prod.result !="0000") {
                    Website.Instance.logger.Debug($"booking_index_getProdDtl_err:prodOid->{confirm.prodOid} ,msg-> {prod.result_msg}");
                    throw new Exception(title.result_code_9990);
                }
                if(pkgs.result!="0000"){
                    Website.Instance.logger.Debug($"booking_index_getProdPkg_err:prodOid->{confirm.prodOid},pkgOid ->{confirm.pkgOid} ,msg-> {prod.result_msg}");
                    throw new Exception(title.result_code_9990);
                } 

                string flightInfoType = "";
                string sendInfoType = "";
                PkgDetailModel pkg = null;
                PkgEventsModel pkgEvent = null;
                CusAgeRange cusAgeRange = null;
                string isEvent = "N";
                string isHl= "N";
                var pkgsTemp = pkgs.pkgs.Where(x => x.pkg_no == confirm.pkgOid).ToList();
                if (pkgsTemp.Count() > 0)
                {
                    foreach (PkgDetailModel p in pkgsTemp)
                    {
                        pkg = p;
                        flightInfoType = p.module_setting.flight_info_type.value;
                        sendInfoType = p.module_setting.send_info_type.value;
                        cusAgeRange = BookingRepostory.getCusAgeRange(confirm,p);

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
                if (isEvent =="Y" &&  string.IsNullOrEmpty(confirm.pkgEvent)) throw new Exception(title.common_data_error);

                if(isEvent =="Y")
                {
                    pkgEvent = ApiHelper.getPkgEvent(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid, confirm.pkgOid,title);

                }
                //必須要設定人數
                //var cusData = BookingRepostory.getCusDdate();
                int totalCus = 0;
                if (module.module_cust_data != null)
                {
                    if (module.module_cust_data.is_require == true) totalCus = (module.module_cust_data.cus_type == "01") ? 1 : Convert.ToInt32(confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty);
                }

                //將dataModel原型 以json str 帶到前台的hidden
                DataModel dm = DataSettingRepostory.getDefaultDataModel(totalCus,guid);
                dm = BookingRepostory.setDefaultBookingInfo(dm, prod, pkg, confirm, fakeContact);//這個地方接pmch要改

                String dataModelStr = JsonConvert.SerializeObject(dm);
                //dm.travelerData[0].meal.mealType
                ViewData["dataModelStr"] = dataModelStr;

                VenueInfo venue = module.module_venue_info;
                if (venue == null) { venue = new VenueInfo(); venue.is_require = false; }
                RentCar rentCar = module.module_rent_car;
                if (rentCar == null) { rentCar = new RentCar(); rentCar.is_require = false; }
                ViewData["confirmPkgInfo"] = confirm;
                ViewData["contactInfo"] = fakeContact;
                ViewData["cusData"] = module.module_cust_data;
                ViewData["guide"] = module.module_guide_lang_list;
                ViewData["wifi"] = module.module_sim_wifi;
                ViewData["exchange"] = module.module_exchange_location_list;
                ViewData["flightInfo"] = module.module_flight_info;
                ViewData["venue"] = venue;// module.module_venue_info;
                ViewData["useDate"] =DateTimeTool.yyyy_mm_dd(confirm.selDate);//DateTimeTool.yyyy_mm_dd(); 
                ViewData["rentCar"] = rentCar;// module.module_rent_car;
                ViewData["carPsgr"] = module.module_car_pasgr; //車輛資料
                ViewData["sendData"] = module.module_send_data;
                ViewData["contactData"] = module.module_contact_data;

                ViewData["guid"] = guid;
                ViewData["prodTitle"] = title;
                ViewData["totalCus"] = totalCus;
                ViewData["mainCat"] = prod.prod_type;
                ViewData["flightInfoType"] = flightInfoType;
                ViewData["sendInfoType"] = sendInfoType;
                ViewData["CutOfDay"] = prod.before_order_day;
                ViewData["cusAgeRange"] = cusAgeRange;
                BookingShowProdModel show = BookingRepostory.setBookingShowProd(prod, pkg, confirm, fakeContact.currency, pkgEvent,title);
                ViewData["prodShow"] = show;

                ViewData["isEvent"] = isEvent;//
                ViewData["isHl"] = isHl; //如果是N就不用做
                ViewData["pkgCanUseDate"] = (isHl=="Y" && isEvent=="Y")?BookingRepostory.getPkgEventDate(pkgEvent, confirm.pkgOid,(confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty)):"";//要把這個套餐可以用的日期全抓出來

                //測試取PMCHLsit
                //KKapiHelper kk = new KKapiHelper();
                //kk.PaymentListReq(prod.countries, prod.prod_no.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"),
                //DateTimeTool.yyyyMMdd2DateTime(confirm.selDate).ToString("yyyy-MM-dd"), DateTimeTool.yyyyMMdd2DateTime(confirm.selDate).ToString("yyyy-MM-dd"), fakeContact.countryCd, fakeContact.lang,
                //prod.prod_type, "127.0.0.1", prod.prod_hander, fakeContact.currency);

                //放到session
                TempData["prod_" + guid] = JsonConvert.SerializeObject(prod);
                TempData["pkgEvent_" + guid] = (isHl == "Y" && isEvent == "Y") ? JsonConvert.SerializeObject(pkgEvent) : "";
                TempData["module_" + guid] = JsonConvert.SerializeObject(module);
                TempData["confirm_" + guid] = JsonConvert.SerializeObject(confirm);
                TempData["ProdTitleKeep_" + guid] = JsonConvert.SerializeObject(title);
                TempData["pkg_" + guid] = JsonConvert.SerializeObject(pkg);
                TempData["pkgsDiscRule_" + guid] = JsonConvert.SerializeObject(pkgs.discount_rule);
                TempData["prodShow_"+guid] = JsonConvert.SerializeObject(show);

                return View();
            }
            catch( Exception ex)
            {
                ViewData["errMsg"] = ex.Message.ToString();
                Website.Instance.logger.Debug($"booking_index_err:{ex.ToString()}");
                //導到錯誤頁
                return RedirectToAction("Index", "Error", new ErrorViewModel { ErrorType = ErrorType.Invalid_Common });
                //return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
                //{ RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

                string titleJson = (string)TempData["ProdTitleKeep_"+ Eventday.guid];
                string pkgEventJson = (string)TempData["pkgEvent_" + Eventday.guid];
                string confirmJson = (string)TempData["confirm_" + Eventday.guid];

                if (string.IsNullOrEmpty(titleJson)){throw new Exception("資料錯誤，請重新讀取頁面");}
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
                Website.Instance.logger.Debug($"bookingStep1_inputdata:{ JsonConvert.SerializeObject(data)}");

                ApiSetting api = new ApiSetting();
                api.apiKey = "kkdayapi";
                api.userOid = "1";
                api.ver = "1.0.1";
                api.locale = "zh-tw";
                api.currency = "TWD";
                api.ipaddress = "61.216.90.96";

                //假分銷商
                distributorInfo fakeContact = DataSettingRepostory.fakeContact();

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

                string showStr = TempData["prodShow_" + data.guidNo] as string;
                if (string.IsNullOrEmpty(showStr)) { throw new Exception("資料錯誤，請重新讀取頁"); }
                //BookingShowProdModel show = JsonConvert.DeserializeObject<BookingShowProdModel>(showStr);

                TempData.Keep();

                //排除餐食 
                data = BookingRepostory.exculdeFood(prod, data, module);

                //DataSettingRepostory Ores = new DataSettingRepostory();
                //data = DataSettingRepostory.fakeDataModel(data);
                //string q = JsonConvert.SerializeObject(data);
                //string b2bOrder = BookingRepostory.insB2dOrder(title, prod, pkg, confirm, data, fakeContact,rule);

                //轉 ordermodel
                OrderRepostory res = new OrderRepostory();
                OrderModel ord = res.setOrderModel(data);
                api.json = ord;

                string orderModelStr = JsonConvert.SerializeObject(api);

                Website.Instance.logger.Debug($"bookingStep1_ordernewdata:{ JsonConvert.SerializeObject(orderModelStr)}");

                KKapiHelper kk = new KKapiHelper();
                JObject order =kk.crtOrder(api);

                string orderMid = "";
                string orderOid = "";
                returnStatus status = new returnStatus();
                //要先判斷是不是result＝'0000'
                if (order["content"]["result"].ToString()=="0000")
                {
                    orderMid = order["content"]["orderMid"].ToString();
                    orderOid = order["content"]["orderOid"].ToString();
                    status.pmchSslRequest = BookingRepostory.setPaymentInfo2(prod,ord, orderMid);
                    status.status = "OK";

                    //要把BookingShowProdModel 帶到訂購final頁
                    RedisHelper.SetProdInfotoRedis(showStr, "b2d:ec:order:final:prodShow:" + orderMid, 60);
                    RedisHelper.SetProdInfotoRedis(JsonConvert.SerializeObject(data), "b2d:ec:order:final:orderData:" + orderMid, 60);

                    //要存redis 付款主要資訊，最後訂單 upd時要使用,可和下面整合存一個就
                    string memUuid = "051794b8-db2a-4fe7-939f-31ab1ee2c719";
                    BookingRepostory.setPayDtltoRedis(ord, orderMid, memUuid);

                    //要存redis 因為付款後要從這個redis內容再進行payment驗證,可和上面整合存一個就好
                    //CallJsonPay rdsJson = (CallJsonPay)status.pmchSslRequest.json;
                    CallJsonPay2 rdsJson = (CallJsonPay2)status.pmchSslRequest.json;
                    string callPmchReq = JsonConvert.SerializeObject(status.pmchSslRequest.json);
                    RedisHelper.SetProdInfotoRedis(callPmchReq, "b2d:ec:pmchSslRequest:"+ orderMid, 60);
                }
                else 
                {
                    Website.Instance.logger.Debug($"bookingStep1:ordernew失敗");//要改
                    status.status = "Error";
                    status.msgErr = "error bookingSetp1_1";//要改
                }

                return Json(status);
            }
            catch (Exception ex)
            {
                //error
                Website.Instance.logger.Debug($"bookingStep1_err:ordernew失敗->{ex.ToString()}");
                returnStatus status = new returnStatus();
                status.status = "Error";
                status.msgErr = "error bookingSetp1_1";//要改

                return Json(status);
            }
        }
    }
}
