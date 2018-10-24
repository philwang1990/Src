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
//using KKday.Web.B2D.EC.Models.Repostory.Booking;

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
                if (guid == null) throw new Exception("err");

                confirmPkgInfo confirm = JsonConvert.DeserializeObject<confirmPkgInfo>(RedisHelper.getProdInfotoRedis("bid:ec:confirm:" + guid)); 

                if (confirm ==null) throw new Exception("err");

                //假分銷商
                distributorInfo fakeContact = DataSettingRepostory.fakeContact();

                //取挖字
                Dictionary<string, string> uikey = RedisHelper.getuiKey(fakeContact.lang);

                //從 api取 
                ProductModuleModel module = ApiHelper.getProdModule(fakeContact.companyXid,fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid,confirm.pkgOid);
                ProductModel prod = ApiHelper.getProdDtl(fakeContact.companyXid,fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid);
                PackageModel pkgs = ApiHelper.getProdPkg(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid);
                PkgEventsModel pkgEvent = ApiHelper.getPkgEvent(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, confirm.prodOid, confirm.pkgOid);

                string flightInfoType = "";
                string sendInfoType = "";
                PkgDetailModel pkg = null;
                var pkgsTemp = pkgs.pkgs.Where(x => x.pkg_no == confirm.pkgOid).ToList();
                if (pkgsTemp.Count() > 0)
                {
                    foreach (PkgDetailModel p in pkgsTemp)
                    {
                        pkg = p;
                        flightInfoType = p.module_setting.flight_info_type.value;
                        sendInfoType = p.module_setting.send_info_type.value;
                    }
                }
                else
                {
                    //丟錯誤頁
                    throw new Exception("err");
                }

                //必須要設定人數
                //var cusData = BookingRepostory.getCusDdate();
                int totalCus = 0;
                if (module.module_cust_data != null)
                {
                    if (module.module_cust_data.is_require == true) totalCus = (module.module_cust_data.cus_type == "01") ? 1 : Convert.ToInt32(confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty);
                }

                //將dataModel 以json str 帶到前台的hidden
                DataModel dm = DataSettingRepostory.getDefaultDataModel(totalCus);

                dm = BookingRepostory.setDefaultBookingInfo(dm, prod, pkg, confirm, fakeContact);

                String dataModelStr = JsonConvert.SerializeObject(dm);
                //dm.travelerData[0].meal.mealType
                ViewData["dataModelStr"] = dataModelStr;

                ProdTitleModel title = ProductRepostory.getProdTitle(uikey);

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
                ViewData["useDate"] = confirm.selDate;
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


                //測試取PMCHLsit
                //KKapiHelper kk = new KKapiHelper();
                //kk.PaymentListReq(prod.countries, prod.prod_no.ToString(), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"),
                                  //DateTimeTool.yyyyMMdd2DateTime(confirm.selDate).ToString("yyyy-MM-dd"), DateTimeTool.yyyyMMdd2DateTime(confirm.selDate).ToString("yyyy-MM-dd"), fakeContact.countryCd, fakeContact.lang,
                                  //prod.prod_type, "127.0.0.1", prod.prod_hander, fakeContact.currency);

                    return View();
            }
            catch( Exception ex)
            {
                //導到錯誤頁
                Website.Instance.logger.Debug($"booking_index_err:{ex.ToString()}");
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
                { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        [HttpPost]
        public IActionResult bookingStep1([FromBody]DataModel data)
        {
            try
            {
                //重新決定排除的餐食-還沒有做
                //'0002': ['0001', '0002', '0003', '0004', '0005', '0006'], //素食
                //'0003': ['0002'], //猶太餐
                //'0004': ['0002', '0005'] //穆斯林餐

                ApiSetting api = new ApiSetting();
                api.apiKey = "kkdayapi";
                api.userOid = "1";
                api.ver = "1.0.1";
                api.locale = "zh-tw";
                api.currency = "TWD";
                api.ipaddress = "61.216.90.96";

                //假分銷商
                distributorInfo fakeContact = DataSettingRepostory.fakeContact();
                ProductModel prod = ApiHelper.getProdDtl(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, data.productOid);

                //DataSettingRepostory Ores = new DataSettingRepostory();
                //data = DataSettingRepostory.fakeDataModel(data);

                //string q = JsonConvert.SerializeObject(data);

                //轉 ordermodel
                OrderRepostory res = new OrderRepostory();
                OrderModel ord = res.setOrderModel(data);
                api.json = ord;

                string qq = JsonConvert.SerializeObject(api);

                KKapiHelper kk = new KKapiHelper();

                JObject order =kk.crtOrder(api);

                string orderMid = "";
                string orderOid = "";
                string json = "";
                returnStatus status = new returnStatus();
                //要先判斷是不是result＝'0000'
                if (order["content"]["result"].ToString()=="0000")
                {
                    orderMid = order["content"]["orderMid"].ToString();
                    orderOid = order["content"]["orderOid"].ToString();
                    json = BookingRepostory.setPaymentInfo(prod,ord, orderMid);
                    status.status = "OK";
                    status.jsonStr = json;
                }
                else 
                {
                    Website.Instance.logger.Debug($"bookingStep1:qq");//要改
                    status.status = "Error";
                    status.jsonStr = json;
                    status.msgErr = "error bookingSetp1_1";//要改
                }

                return Json(status);
            }
            catch (Exception ex)
            {
                //error
                Website.Instance.logger.Debug($"bookingStep1:{ex.ToString()}");
                returnStatus status = new returnStatus();
                status.status = "Error";
                status.msgErr = "error bookingSetp1_1";//要改

                return Json(status);
            }
        }
    }
}
