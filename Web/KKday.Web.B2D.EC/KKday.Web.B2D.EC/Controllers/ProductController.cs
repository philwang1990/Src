using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Model.Product;
using KKday.Web.B2D.EC.Models.Repostory.Product;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using static KKday.Web.B2D.EC.Controllers.ProductController;
using Newtonsoft.Json;
using KKday.Web.B2D.EC.Models.Model.Booking;
using KKday.Web.B2D.EC.Models.Repostory.Booking;
using KKday.Web.B2D.EC.Models;
using System.Diagnostics;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace KKday.Web.B2D.EC.Controllers
{
    public class ProductController : Controller
    {
        ICompositeViewEngine ViewEngine;
        public ProductController(ICompositeViewEngine viewEngine)
        {
            ViewEngine = viewEngine;
        }

        // GET: /<controller>/
        public IActionResult Index(string id)
        {
            try
            {
                //還沒有做的區塊
                //productIntro 
                //product_intruduction_tab 行程表 憑證區塊

                //呼叫api 取資料   https://api.sit.kkday.com/api/product/info/fe/v1/17379
                //判斷是否有資料，如果沒有要跳 alert 並導頁
                //資料取得後要揉資料
                //要把挖字存成一個物件並帶到view去
                //logs Website.Instance.logger.Info($"[PAY]kkOrderNo:{bookRQ.order.orderMid},priceType:{lst.price_type},jtrTktNo:{payRS.code},jtrErr:{payRS.error_msg}");
                //假分銷商
                distributorInfo fakeContact = DataSettingRepostory.fakeContact();

                //取挖字
                Dictionary<string, string> uikey = RedisHelper.getuiKey(fakeContact.lang);
                //ProdTitleModel title = ProductRepostory.getProdTitle(uikey);
                ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(JsonConvert.SerializeObject(uikey));

                if (id == null) throw new Exception("商品不存在");

                //從 api取 
                ProductforEcModel prod = ProductRepostory.getProdDtl(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, id,title);

                if (prod.result != "0000") throw new Exception(prod.result_msg); //不正確就導錯誤頁,但api還未處理怎麼回傳

                PackageModel pkgs = ProductRepostory.getProdPkg(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, id,title);

                if (pkgs.result != "0000") throw new Exception(prod.result_msg);//不正確就導錯誤頁,但api還未處理怎麼回傳

                //判斷是不是可以可以秀可以賣 ,但api 未決定錯誤怎麼給
                if (prod.prod_mkt.is_ec_sale == false) //不能秀就導錯誤頁
                {
                    throw new Exception("商品不顯示！");
                }

                string guid = System.Guid.NewGuid().ToString();

                string allCanUseDate = "";


                prod = ProductRepostory.getProdOtherInfo(prod, id, fakeContact.lang, fakeContact.currency, uikey); prod.guidNo = guid;
                List<PkgDateforEcModel> prodPkgDateList = ProductRepostory.getProdPkgDate(pkgs, fakeContact.lang, fakeContact.currency, uikey, out allCanUseDate);

                TempData["ProdTitleKeep"] = JsonConvert.SerializeObject(uikey);

                //取消政策排序
                if (prod.policy_list != null && prod.policy_list.Any())
                {
                    prod.policy_list = prod.policy_list.OrderByDescending(o => o.is_over).OrderByDescending(o => o.days).ToList();
                }

                ProductModuleModel module = ProductRepostory.getProdModule(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, id, "",title);
                if(module != null && module.module_venue_info != null){
                    if(module.module_venue_info.venue_type == "01")
                    {
                        ViewData["moduleVenue"] = module.module_venue_info;
                    }
                }


                ViewData["prodTitle"] = title;
                ViewData["prod"] = prod;
                ViewData["pkgs"] = pkgs;
                ViewData["allCanUseDate"] = allCanUseDate;
                ViewData["pkgDate"] = prodPkgDateList;
                ViewData["currency"] = fakeContact.currency;

                return View();
            }
            catch (Exception ex)
            {
                //導到錯誤頁
                Website.Instance.logger.Debug($"product_index_err:{ex.ToString()}");
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
                { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]//[AcceptVerbs("Post")]
        public IActionResult reflashPkg([FromBody]prodQury prodQury)
        {
            try
            {
                distributorInfo fakeContact = DataSettingRepostory.fakeContact();
                //先確認是否有selDate ,如果沒有表示是第一次
                //如果有 selDate要依selDate決定可用的套餐


                string allCanUseDate = "";

                //取挖字
                Dictionary<string, string> uikey = RedisHelper.getuiKey(fakeContact.lang);

                ProdTitleModel title = ProductRepostory.getProdTitle(uikey);
                ViewData["prodTitle"] = title;
                ViewData["currency"] = fakeContact.currency;//先從這裡取得幣別就不用再抓prod

                //ProductforEcModel prod = ApiHelper.getProdDtl(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, prodQury.prodOid);
                //prod = prodRep.getProdInfo(prod, prodQury.prodOid, fakeContact.lang, fakeContact.currency, uikey); //prod.guidNo = guid;
                //ViewData["prodInfo"] = prod;

                PackageModel pkgs = ProductRepostory.getProdPkg(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, prodQury.prodOid,title);
                List<PkgDateforEcModel> prodPkgDateList = ProductRepostory.getProdPkgDate(pkgs, fakeContact.lang, fakeContact.currency, uikey, out allCanUseDate);

                //設定每個pkg裡面可以使用的日期有那些
                pkgs = ProductRepostory.InitPkg(prodQury, title, pkgs, prodPkgDateList);

                return Content(this.RenderPartialViewToString(ViewEngine, "_prodPkg", pkgs));
            }
            catch (Exception ex)
            {
                //error
                Website.Instance.logger.Debug($"product_reflashPkg_err:{ex.ToString()}");
                returnStatus status = new returnStatus();
                status.status = "Error";
                status.msgErr = "資料錯誤，請重新讀取頁";//要改

                return Json(status);
            }
        }

        [HttpPost]//[AcceptVerbs("Post")]
        public IActionResult confirmPkg([FromBody]confirmPkgInfo confirm)
        {
            try
            {
                //要存redis往後帶
                string guid = confirm.guid;
                string confirmStr = JsonConvert.SerializeObject(confirm);
                RedisHelper.SetProdInfotoRedis(confirmStr, "bid:ec:confirm:" + guid, 30);
                //confirmStr = "";
                //confirm = JsonConvert.DeserializeObject<confirmPkgInfo>(RedisHelper.getProdInfotoRedis("bid:ec:confirm:" + guid)); 

                //金額要再確認....
                returnStatus status = new returnStatus();
                status.status = "OK";

                return Json(status);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"product_confirmPkg_err:{ex.ToString()}");
                returnStatus status = new returnStatus();
                status.status = "Error";
                status.msgErr = "資料錯誤，請重新讀取頁";//要改

                return Json(status);
            }
        }


        //Put in BaseController
        public string RenderPartialViewToString(ICompositeViewEngine viewEngine, string viewName, object model)
        {
            viewName = viewName ?? ControllerContext.ActionDescriptor.ActionName;
            ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                IView view = viewEngine.FindView(ControllerContext, viewName, false).View;
                ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw, new HtmlHelperOptions());
                view.RenderAsync(viewContext).Wait();
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Get Event Time
        /// </summary>
        /// <returns>The package.</returns>
        /// <param name="prodEvent">prodEvent.</param>
        [HttpPost]//[AcceptVerbs("Post")]
        public IActionResult GetEventTime([FromBody]prodEvent prodEvent)
        {
            try
            {
                distributorInfo fakeContact = DataSettingRepostory.fakeContact();
                //取挖字
                String json = TempData["ProdTitleKeep"] as string;
                if (string.IsNullOrEmpty(json))
                {
                    throw new Exception("資料錯誤，請重新讀取頁面");
                }
                ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(json);
                TempData.Keep();

                PkgEventsModel getEventTime = ProductRepostory.getEvent(fakeContact.companyXid, fakeContact.state, fakeContact.lang, fakeContact.currency, prodEvent.prodno, prodEvent.pkgno,title);
                if(getEventTime.result == "0000"){
                    var result = getEventTime.events.Where(x => x.day == prodEvent.DateSelected);
                    getEventTime.events = result.ToList();
                    //return Json(result.ToList());
                    return Json(new { errMsg = "", data = result.ToList() });
                }
                else{
                    return Json(new { errMsg = "false" });
                }
                //return Content(this.RenderPartialViewToString(ViewEngine, "_prodPkg", pkgs));

            }
            catch (Exception ex)
            {
                //error
                Website.Instance.logger.Debug($"product_eventtime_err:{ex.ToString()}");
                returnStatus status = new returnStatus();
                status.status = "Error";
                status.msgErr = "資料錯誤，請重新讀取頁面";//要改

                return Json(status);
            }
        }

        [HttpPost]//[AcceptVerbs("Post")]
        public IActionResult getKlingon(string key, string replace)
        {
            try
            {
                string replaceWord = "%s";
                string msg = "";
                String json = TempData["ProdTitleKeep"] as string;
                if(string.IsNullOrEmpty(json)){
                    throw new Exception("");
                }
                ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(json);
                TempData.Keep();

                switch(key)
                {
                    case "product_index_min_event_qty_alert":
                        msg = title.product_index_min_event_qty_alert.Replace(replaceWord, replace);
                        break;
                    case "product_index_min_order_adult_qty_alert":
                        msg = title.product_index_min_order_adult_qty_alert.Replace(replaceWord, replace);
                        break;
                    case "product_index_min_order_qty_alert":
                        msg = title.product_index_min_order_qty_alert.Replace(replaceWord, replace);
                        break;
                    case "product_index_max_order_qty_alert":
                        msg = title.product_index_max_order_qty_alert.Replace(replaceWord, replace);
                        break;
                    default:
                        break;
                }

                return Json(new { flag = true, errMsg = "", msgreturn = msg });
            }
            catch (Exception ex){
                Website.Instance.logger.Debug($"product_getKlingon_err:{ex.ToString()}");
                return Json(new { flag = false, errMsg = "資料錯誤，請重新讀取頁" });
            }
        }
    }
}
