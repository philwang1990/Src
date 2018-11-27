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

        public IActionResult Index()
        {
            return View();
        }


        //付款後導回
        public IActionResult Step3(string id, string jsondata)
        {
            B2dAccount UserData = null;
            BookingShowProdModel prodShow = null;
            DataModel orderData = null;
            Boolean chkSuccess = true;
            if (id != null && jsondata != null)
            {
                ////回傳的連結有訂編 (記log)
                ////透過訂編將redis 的資料抓回送出去的資料
                ////取b2dredis 內的paymentDtl
                ////string payDtlStr = RedisHelper.getProdInfotoRedis("b2d:ec:payDtl:" + id);
                ////PaymentDtl payDtl = JsonConvert.DeserializeObject<PaymentDtl>(payDtlStr);

                ////從kkday redis 取出
                ////組出token res:pmgwTransNo, res:pmgwMethod ,res:pmch_resp ceil res:payAmount order_mid
                ////md5($pmgw_trans_no.$pmgw_method.$trans_curr_cd.$trans_amt.$pmch_ref_no.$key);
                ////PmchSslResponse res = JsonConvert.DeserializeObject<PmchSslResponse>(jsondata); //舊版
                PmchSslResponse2 res = JsonConvert.DeserializeObject<PmchSslResponse2>(jsondata); //新版
                //string transNo = GibberishAES.OpenSSLDecrypt(res.data.pmgw_trans_no, "pmgw@%#@trans*no");
                ////CallJsonPay req = JsonConvert.DeserializeObject<CallJsonPay>(RedisHelper.getProdInfotoRedis("b2d:ec:pmchSslRequest:" + id)); //using KKday.Web.B2D.EC.AppCode;
                //CallJsonPay2 req = JsonConvert.DeserializeObject<CallJsonPay2>(RedisHelper.getProdInfotoRedis("b2d:ec:pmchSslRequest:" + id)); //using KKday.Web.B2D.EC.AppCode;

                //string token = Website.Instance.Configuration["kkdayKey:pmgwValidToken"].ToString();
                //string pmgwMethod = res.data.pmgw_method;

                //string payCurrency = res.data.pay_currency;
                //string payAmount = Math.Ceiling(res.data.pay_amount).ToString();
                //string pmgwValidToken = MD5Tool.GetMD5(transNo + pmgwMethod + payCurrency + payAmount + id + token);

                //KKapiHelper helper = new KKapiHelper();
                //必須要再呼叫一次要讓FA 知道這個授權是kkday做的!而不是robot
                //string isSuccess = helper.PaymentValid(transNo, pmgwValidToken);

                //如果ok就upd

                //helper.PayUpdSuccessUpdOrder(id, transNo, payDtl, req, res, fakeContact);//舊版
                //helper.PayUpdSuccessUpdOrder2(id, transNo, payDtl, req, res, fakeContact); //新版

                Website.Instance.logger.Debug($",bookingStep3_id:{id},bookingStep3_jsondata:{jsondata}");

                if (res.metadata.status != "0000") //授權失敗,直接跳付款失敗
                {
                    chkSuccess = false;
                }
                else
                {

                    Boolean chk = ApiHelper.PaymentValid(id, jsondata);
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

            Dictionary<string, string> uikey = CommonRepostory.getuiKey(RedisHelper,UserData.LOCALE);// RedisHelper.getuiKey(fakeContact.lang);
            ProdTitleModel title = JsonConvert.DeserializeObject<ProdTitleModel>(JsonConvert.SerializeObject(uikey));

            ViewData["chkSuccess"] = chkSuccess;
            ViewData["prodShow"] = prodShow;
            ViewData["orderData"] = orderData;
            ViewData["prodTitle"] = title;

            return View("Success");
        }


        //付款中途停止導回
        public IActionResult Failure(string id)
        {
            Website.Instance.logger.Debug($",bookingFailure_id:{id}");
            BookingShowProdModel prodShow = null;
            DataModel orderData = null;
            Boolean chkSuccess = true;
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
    }
}
