using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.Web.B2D.EC.Models.Model.Pmch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Repostory.Booking;
using KKday.Web.B2D.EC.Models.Model.Booking;
using KKday.Web.B2D.EC.Models.Model.Product;
using Microsoft.AspNetCore.Http;
using KKday.Web.B2D.EC.Models.Repostory.Common;
using System.Security.Claims;
using KKday.Web.B2D.EC.Models.Model.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using KKday.Web.B2D.EC.Models;
using KKday.Web.B2D.BE.App_Code;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.Web.B2D.EC.Controllers
{
    [Authorize(Policy = "UserOnly")]
    public class FinalController : Controller
    {
        // GET: /<controller>
        private readonly IHttpContextAccessor httpContextAccessor;
        private static IRedisHelper RedisHelper;

        public FinalController(IHttpContextAccessor _httpContextAccessor, IRedisHelper _redisHelper)
        {
            httpContextAccessor = _httpContextAccessor;
            RedisHelper = _redisHelper;
        }
       
        [AllowAnonymous]
        public IActionResult Index(string id, string jsondata)
        {
            try
            {
                if (jsondata != null)
                {
                    var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
                    consentFeature.GrantConsent();

                    TempData[id+"forward"] = jsondata;
                    Website.Instance.logger.Debug($"bookingStep3_{id}_forward:{jsondata}");
                    return RedirectToAction("Step3", "Final", new { id = id });
                }
                else
                {
                    Website.Instance.logger.Debug($"bookingStep3_{id}_forward:nojasondata");
                    return RedirectToAction("Failure", "Final", new { id = id });
                }
            }
            catch( Exception ex)
            {
                Website.Instance.logger.Debug($"bookingStep3_index_err:"+ex.Message.ToString());
                //return RedirectToAction("Failure", "Final",new {id=id});
                ViewData["errMsg"] = ex.Message.ToString();
                Website.Instance.logger.Debug($"Final_Index_err:{ex.Message.ToString()}");
                //導到錯誤頁
                return RedirectToAction("Index", "Error", new ErrorViewModel { ErrorType = ErrorType.Order_Fail });

            }
        }

        //付款後導回
        public IActionResult Step3( string id)
        {
            try
            {
                BookingShowProdModel prodShow = null;
                DataModel orderData = null;
                Boolean chkSuccess = true;

                //B2d分銷商資料
                string jsondata = TempData[id + "forward"] as string;
                if (string.IsNullOrEmpty(jsondata)) { chkSuccess = false; }

                B2dAccount UserData = null;

                if (id != null && jsondata != null && chkSuccess == true)
                {
                    PmchSslResponse2 res = JsonConvert.DeserializeObject<PmchSslResponse2>(jsondata); //新版
                    Website.Instance.logger.Debug($",bookingStep3_id:{id},bookingStep3_jsondata:{jsondata}");
                    if (res.metadata.status != "0000") //授權失敗,直接跳付款失敗
                    {
                        chkSuccess = false;
                    }
                    else
                    {
                        Boolean chk = ApiHelper.PaymentValid(id, res);
                        if (chk == false) { chkSuccess = false; }
                    }

                    string prodShowStr = RedisHelper.getRedis("b2d:ec:order:final:prodShow:" + id);
                    if (prodShowStr != null) prodShow = JsonConvert.DeserializeObject<BookingShowProdModel>(prodShowStr);
                    string orderDataStr = RedisHelper.getRedis("b2d:ec:order:final:orderData:" + id);
                    if (orderDataStr != null) orderData = JsonConvert.DeserializeObject<DataModel>(orderDataStr);
                }
                else
                {
                    chkSuccess = false;
                }

                //取挖字
                //B2d分銷商資料
                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                Dictionary<string, string> uikey = CommonRepostory.getuiKey(RedisHelper, UserData.LOCALE);// RedisHelper.getuiKey(fakeContact.lang);
                ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(JsonConvert.SerializeObject(uikey));

                ViewData["chkSuccess"] = chkSuccess;
                ViewData["prodShow"] = prodShow;
                ViewData["orderData"] = orderData;
                ViewData["prodTitle"] = title;

                return View("Success");
            }
            catch(Exception ex)
            {
                ViewData["errMsg"] = ex.Message.ToString();
                Website.Instance.logger.Debug($"Final_Step3_err:{ex.Message.ToString()}");
                //導到錯誤頁
                return RedirectToAction("Index", "Error", new ErrorViewModel { ErrorType = ErrorType.Order_Fail });
            }
        }


        //付款中途停止導回
        //public IActionResult Failure(string id)
        public IActionResult Failure(string id)
        {
            try
            {
                BookingShowProdModel prodShow = null;
                DataModel orderData = null;
                Boolean chkSuccess = true;

                Website.Instance.logger.Debug($",bookingFailure_id:{id}");

                if (id != null)
                {
                    string prodShowStr = RedisHelper.getRedis("b2d:ec:order:final:prodShow:" + id);
                    if (prodShowStr != null) prodShow = JsonConvert.DeserializeObject<BookingShowProdModel>(prodShowStr);
                    string orderDataStr = RedisHelper.getRedis("b2d:ec:order:final:orderData:" + id);
                    if (orderDataStr != null) orderData = JsonConvert.DeserializeObject<DataModel>(orderDataStr);
                }
                else
                {
                    chkSuccess = false;
                }
                //B2d分銷商資料
                var aesUserData = User.Identities.SelectMany(i => i.Claims.Where(c => c.Type == ClaimTypes.UserData).Select(c => c.Value)).FirstOrDefault();
                var UserData = JsonConvert.DeserializeObject<B2dAccount>(AesCryptHelper.aesDecryptBase64(aesUserData, Website.Instance.AesCryptKey));

                //取挖字
                Dictionary<string, string> uikey = CommonRepostory.getuiKey(RedisHelper, UserData.LOCALE); ;//RedisHelper.getuiKey(fakeContact.lang);
                ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(JsonConvert.SerializeObject(uikey));

                ViewData["chkSuccess"] = chkSuccess;
                ViewData["prodShow"] = prodShow;
                ViewData["orderData"] = orderData;
                ViewData["prodTitle"] = title;
                return View("Success");
            }
            catch(Exception ex)
            {
                ViewData["errMsg"] = ex.Message.ToString();
                Website.Instance.logger.Debug($"Final_Step3_err:{ex.Message.ToString()}");
                //導到錯誤頁
                return RedirectToAction("Index", "Error", new ErrorViewModel { ErrorType = ErrorType.Order_Fail });
            }
        }
    }
}
