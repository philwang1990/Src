using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.Models.Repository.Booking;
using KKday.API.WMS.Models.Repository.Product;
using KKday.API.WMS.Models.DataModel.Booking;
using KKday.API.WMS.Models.DataModel.Product;
using KKday.API.WMS.Models.DataModel.Package;
using KKday.API.WMS.Models.DataModel.Pmch;
using KKday.API.WMS.AppCode;
using KKday.API.WMS.Models;
using System.Diagnostics;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {

    [Route("api/[controller]")]
    public class BookingController : Controller {
        RedisHelper rds = new RedisHelper();
        //private static RedisHelper rds;

        [HttpPost("InsertOrder")]
        public OrderNoModel InsertOrder([FromBody]OrderModel queryRQ)
        {
            //Website.Instance.logger.Info($"WMS QueryProduct Start! B2D Xid:{queryRQ.company_xid},KKday ProdOid:{queryRQ.prod_no}");

            //var prod_dtl = new ProductModel();

            //prod_dtl = ProductRepository.GetProdDtl(queryRQ);

            //BookingRepository.InsertOrder(queryRQ);

            Website.Instance.logger.Info($"WMS InsertOrder Start! ");

            return BookingRepository.InsertOrder(queryRQ);
        }

        [HttpPost("UpdateOrder")]
        public OrderNoModel UpdateOrder([FromBody]UpdateOrderModel queryRQ)
        {

            Website.Instance.logger.Info($"WMS UpdateOrder Start! ");

            return BookingRepository.UpdateOrder(queryRQ);
        }

        // GET: /<controller>/
        public IActionResult Index(string guid)
        {
            try
            {
                if (guid == null) throw new Exception("err");

                confirmPkgInfo confirm = JsonConvert.DeserializeObject<confirmPkgInfo>(rds.getRedis("bid:ec:confirm:" + guid));
                //confirm.pkgEvent = "1";
                if (confirm == null) throw new Exception("err");

                //假分銷商
                distributorInfo fakeContact = DataSettingRepository.fakeContact();

                //取挖字
                Dictionary<string, string> uikey = RedisHelper.getuiKey(fakeContact.lang);

                //從 api取 
                ProductModuleModel module = ApiHelper.getProdModule(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid, confirm.pkgOid);
                ProductModel prod = ApiHelper.getProdDtl(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid);
                PackageModel pkgs = ApiHelper.getProdPkg(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid);

                PkgEventsModel pkgEvent = null;

                string flightInfoType = "";
                string sendInfoType = "";
                PkgDetailModel pkg = null;
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
                        cusAgeRange = BookingRepository.getCusAgeRange(confirm, p);

                        isEvent = p.is_event;
                        isHl = p.is_hl;
                    }
                }
                else
                {
                    //丟錯誤頁
                    throw new Exception("err");
                }

                //如果有event 但沒有傳 event id ,就error
                if (isEvent == "Y" && string.IsNullOrEmpty(confirm.pkgEvent)) throw new Exception("err");

                if (isEvent == "Y")
                {
                    pkgEvent = ApiHelper.getPkgEvent(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid, confirm.pkgOid);

                }

                //必須要設定人數
                //var cusData = BookingRepostory.getCusDdate();
                int totalCus = 0;
                if (module.module_cust_data != null)
                {
                    if (module.module_cust_data.is_require == true) totalCus = (module.module_cust_data.cus_type == "01") ? 1 : Convert.ToInt32(confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty);
                }

                //將dataModel 以json str 帶到前台的hidden
                DataKKdayModel dm = DataSettingRepository.getDefaultDataModel(totalCus);
                dm = BookingRepository.setDefaultBookingInfo(dm, prod, pkg, confirm, fakeContact);

                String dataModelStr = JsonConvert.SerializeObject(dm);
                //dm.travelerData[0].meal.mealType
                ViewData["dataModelStr"] = dataModelStr;

                ProdTitleModel title = ProductRepository.getProdTitle(uikey);

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
                ViewData["useDate"] = DateTimeTool.yyyy_mm_dd(confirm.selDate);//DateTimeTool.yyyy_mm_dd(); 
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
                ViewData["prodShow"] = BookingRepository.setBookingShowProd(prod, pkg, confirm, fakeContact.currency, pkgEvent);

                ViewData["isEvent"] = isEvent;//
                ViewData["isHl"] = isHl; //如果是N就不用做
                ViewData["pkgCanUseDate"] = (isHl == "Y" && isEvent == "Y") ? BookingRepository.getPkgEventDate(pkgEvent, confirm.pkgOid, (confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty)) : "";//要把這個套餐可以用的日期全抓出來


                //測試取PMCHLsit
                //KKapiHelper kk = new KKapiHelper();
                //kk.PaymentListReq(prod.countries, prod.prod_no.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"),
                //DateTimeTool.yyyyMMdd2DateTime(confirm.selDate).ToString("yyyy-MM-dd"), DateTimeTool.yyyyMMdd2DateTime(confirm.selDate).ToString("yyyy-MM-dd"), fakeContact.countryCd, fakeContact.lang,
                //prod.prod_type, "127.0.0.1", prod.prod_hander, fakeContact.currency);

                //放到session
                TempData["qq"] = "test";
                TempData["pkgs"] = JsonConvert.SerializeObject(pkgs);
                TempData["pkgEvent"] = (isHl == "Y" && isEvent == "Y") ? JsonConvert.SerializeObject(pkgEvent) : "";
                TempData["confirm"] = JsonConvert.SerializeObject(confirm);
                return View();
            }
            catch (Exception ex)
            {
                //導到錯誤頁
                Website.Instance.logger.Debug($"booking_index_err:{ex.ToString()}");
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
                { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        //取得候補的場次
        [HttpPost]
        public IActionResult getEvent([FromBody] EventQury Eventday)
        {
            List<string> dayevent = new List<string>();
            string day = Eventday.day.Replace("-", "");
            string dd = TempData["qq"] as string;
            string qq = TempData["pkgEvent"] as string;
            PkgEventsModel pkgEvent = JsonConvert.DeserializeObject<PkgEventsModel>((string)TempData["pkgEvent"]);
            confirmPkgInfo confirm = JsonConvert.DeserializeObject<confirmPkgInfo>((string)TempData["confirm"]);
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
                return Json(dayevent);
            }
            else
            {
                //再補回傳的格式
                return Json("");
            }
        }


        [HttpPost("bookingStep1")]
        public BookingRSModel bookingStep1([FromBody]DataKKdayModel data)
        {
            BookingRSModel bookingRS = new BookingRSModel();
            double  calAmt = 0; // 計算金額
            double  b2cAmt = 0; // 計算直客價金額

            try
            {
                //先查看價格是否正確
                if(rds.getRedis("b2d:pkgsPrice:" + data.guidNo) == null)
                {
                    Website.Instance.logger.Debug($"getRedis:error");//要改
                    bookingRS.result = "10001";
                    bookingRS.result_msg = "在redis上找不到資料"; //要改
                    return bookingRS;
                }


                PkgPriceModel pkgPrice = JsonConvert.DeserializeObject<PkgPriceModel>(rds.getRedis("b2d:pkgsPrice:" + data.guidNo));
                //if (pkgPrice.discount_rule.isRule == true) // 有中折扣規則
                //{
                //    foreach (var i in pkgPrice.pkgs)
                //    {
                //        if( i.pkg_no == data.packageOid )
                //        {
                //            calAmt += (double)data.price1Qty * (double)i.price1;
                //            calAmt += (double)data.price2Qty * (double)i.price2;
                //            calAmt += (double)data.price3Qty * (double)i.price3;
                //            calAmt += (double)data.price4Qty * (double)i.price4;
                //            b2cAmt += (double)data.price1Qty * (double)i.price1_b2c;
                //            b2cAmt += (double)data.price2Qty * (double)i.price2_b2c;
                //            b2cAmt += (double)data.price3Qty * (double)i.price3_b2c;
                //            b2cAmt += (double)data.price4Qty * (double)i.price4_b2c;
                //        }
                //    }
                //}
                //else
                //{
                //    foreach (var i in pkgPrice.pkgs)
                //    {
                //        if (i.pkg_no == data.packageOid)
                //        {
                //            calAmt += (double)data.price1Qty * (double)i.price1_b2c;
                //            calAmt += (double)data.price2Qty * (double)i.price2_b2c;
                //            calAmt += (double)data.price3Qty * (double)i.price3_b2c;
                //            calAmt += (double)data.price4Qty * (double)i.price4_b2c;
                //        }
                //    }
                //}

                foreach (var i in pkgPrice.pkgs)
                {
                    if (i.pkg_no == data.packageOid)
                    {
                        calAmt += (double)data.price1Qty * (double)i.price1;
                        calAmt += (double)data.price2Qty * (double)i.price2;
                        calAmt += (double)data.price3Qty * (double)i.price3;
                        calAmt += (double)data.price4Qty * (double)i.price4;
                        b2cAmt += (double)data.price1Qty * (double)i.price1_b2c;
                        b2cAmt += (double)data.price2Qty * (double)i.price2_b2c;
                        b2cAmt += (double)data.price3Qty * (double)i.price3_b2c;
                        b2cAmt += (double)data.price4Qty * (double)i.price4_b2c;
                    }
                }


                if (calAmt != data.currPriceTotal)
                {
                    Website.Instance.logger.Debug($"currPriceTotal:error");//要改
                    bookingRS.result = "10001";
                    bookingRS.result_msg = "金額有誤"; //要改
                    return bookingRS;
                }

                //if (pkgPrice.discount_rule.isRule == true) //  currPriceTotal 要換成直客價 不然order new 會有問題
                //data.currPriceTotal = b2cAmt;
                data.currPriceTotal = b2cAmt; // currPriceTotal 要換成直客價 不然order new 會有問題


                //重新決定排除的餐食-還沒有做
                //'0002': ['0001', '0002', '0003', '0004', '0005', '0006'], //素食
                //'0003': ['0002'], //猶太餐
                //'0004': ['0002', '0005'] //穆斯林餐


                ApiSetting api = new ApiSetting();
                api.apiKey = Website.Instance.Configuration["KKAPI_INPUT:API_KEY"];
                api.userOid = Website.Instance.Configuration["KKAPI_INPUT:USER_OID"];
                api.ver = Website.Instance.Configuration["KKAPI_INPUT:VER"];
                api.locale = data.locale;
                api.currency = data.currency;
                api.ipaddress = data.ipaddress ;

                //假分銷商
                //distributorInfo fakeContact = DataSettingRepository.fakeContact();
                //ProductModel prod = ApiHelper.getProdDtl(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, data.productOid);

                //DataSettingRepostory Ores = new DataSettingRepostory();
                 //= DataSettingRepostory.fakeDataModel(data);
                string q = JsonConvert.SerializeObject(data);

                //轉 ordermodel
                OrderRepository res = new OrderRepository();
                OrderKKdayModel ord = res.setOrderModel(data);
                api.json = ord;

                string qq = JsonConvert.SerializeObject(api);
                KKapiHelper kk = new KKapiHelper();
                JObject order =kk.crtOrder(api);

                //string orderMid = "";
                //string orderOid = "";
                //returnStatus status = new returnStatus();
                //要先判斷是不是result＝'0000'
                 if (order["content"]["result"].ToString()=="0000")
                {
                    //orderMid = order["content"]["orderMid"].ToString();
                    //orderOid = order["content"]["orderOid"].ToString();
                    //status.pmchSslRequest = BookingRepository.setPaymentInfo2(prod,ord, orderMid);
                    //status.status = "OK";

                    ////要存redis 付款主要資訊，最後訂單 upd時要使用,可和下面整合存一個就
                    //string memUuid = Website.Instance.Configuration["KKAPI_INPUT:JSON:MEMBER_UUID"];
                    //BookingRepository.setPayDtltoRedis(ord, orderMid, memUuid);

                    ////要存redis 因為付款後要從這個redis內容再進行payment驗證,可和上面整合存一個就好
                    //CallJsonPay2 rdsJson = (CallJsonPay2)status.pmchSslRequest.json;
                    //string callPmchReq = JsonConvert.SerializeObject(status.pmchSslRequest.json);
                    //rds.SetProdInfotoRedis(callPmchReq, "b2d:ec:pmchSslRequest:"+ orderMid, 60);

                    //轉 ordermodel
                    OrderModel ordModel = BookingRepository.setOrderModel(data, pkgPrice, calAmt);
                    OrderNoModel ordNoModel = InsertOrder(ordModel);
                    if (ordNoModel.result == "0000")
                    {
                        UpdateOrderModel UpdOrdModel = BookingRepository.setUpdOrdModel(data, ordNoModel.order_no,order);

                        OrderNoModel updResult = UpdateOrder(UpdOrdModel);


                        if (updResult.result == "0000")
                        {
                            bookingRS.result = "0000";
                            bookingRS.result_msg = "OK"; //要改
                            bookingRS.order_oid = UpdOrdModel.order_oid;
                            bookingRS.order_mid = UpdOrdModel.order_mid;
                            bookingRS.order_no = UpdOrdModel.order_no;
                            return bookingRS;
                        }
                        else
                        {
                            Website.Instance.logger.Debug($"UpdateOrder:error");//要改
                            bookingRS.result = "10001";
                            bookingRS.result_msg = updResult.ToString(); //要改
                            return bookingRS;
                        }

                    }
                    else 
                    {
                        Website.Instance.logger.Debug($"InsertOrder:error");//要改
                        bookingRS.result = "10001";
                        bookingRS.result_msg = ordNoModel.result_msg; //要改
                        return bookingRS;
                    }



                    //return order.ToString();
                }
                else 
                {
                    Website.Instance.logger.Debug($"bookingStep1:error");//要改
                    bookingRS.result = "10001";
                    bookingRS.result_msg = order.ToString(); //要改
                    return bookingRS;
                }

            }
            catch (Exception ex)
            {
                //error
                Website.Instance.logger.Debug($"bookingStep1:{ex.ToString()}");
                bookingRS.result = "10001";
                bookingRS.result_msg = ex.ToString();//要改

                return bookingRS;
            }
        }

        [HttpPost("paymentList")]
        public string paymentList([FromBody]List<payTypeValue> payTypeList)
        {
            return BookingRepository.paymentList(payTypeList);
        }
    }
}
