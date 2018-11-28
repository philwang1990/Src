using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKday.API.WMS.Models.DataModel.Pmch ;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KKday.API.WMS.AppCode;
using KKday.API.WMS.Models.Repository.Booking;
using KKday.API.WMS.Models.DataModel.Booking;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers
{
    [Route("api/[controller]")]
    public class FinalController : Controller
    {

        static RedisHelper rds = new RedisHelper();
        //private static RedisHelper rds;
        // GET: /<controller>/

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Step3")]
        //付款後導回
        public String Step3(string mid,string jsondata)
        {
            //jsondata = jsondata.Replace(@"\","");
            //回傳的連結有訂編 (記log)
            //透過訂編將redis 的資料抓回送出去的資料
            //取b2dredis 內的paymentDtl
            string payDtlStr = rds.getRedis("b2d:ec:payDtl:" + mid);
            PaymentDtl  payDtl= JsonConvert.DeserializeObject<PaymentDtl>(payDtlStr);

            //從kkday redis 取出
            //組出token res:pmgwTransNo, res:pmgwMethod ,res:pmch_resp ceil res:payAmount order_mid
            //md5($pmgw_trans_no.$pmgw_method.$trans_curr_cd.$trans_amt.$pmch_ref_no.$key);
            //PmchSslResponse res = JsonConvert.DeserializeObject<PmchSslResponse>(jsondata); //舊版
            PmchSslResponse2 res = JsonConvert.DeserializeObject<PmchSslResponse2>(jsondata); //新版
            res.data.pmgw_trans_no = res.data.pmgw_trans_no.Replace(" ", "+");
            string transNo = GibberishAES.OpenSSLDecrypt(res.data.pmgw_trans_no, Website.Instance.Configuration["PMCH:TRANS_NO"]);
            //CallJsonPay req = JsonConvert.DeserializeObject<CallJsonPay>(RedisHelper.getProdInfotoRedis("b2d:ec:pmchSslRequest:" + id)); //using KKday.Web.B2D.EC.AppCode;
            CallJsonPay2 req = JsonConvert.DeserializeObject<CallJsonPay2>(rds.getRedis("b2d:ec:pmchSslRequest:" + mid)); //using KKday.Web.B2D.EC.AppCode;

            string token = Website.Instance.Configuration["PMCH:TOKEN"];
            string pmgwMethod = res.data.pmgw_method;

            string payCurrency = res.data.pay_currency;
            string payAmount = Math.Ceiling(res.data.pay_amount).ToString();
            string pmgwValidToken =  MD5Tool.GetMD5(transNo + pmgwMethod + payCurrency + payAmount + mid + token);

            KKapiHelper helper = new KKapiHelper();
            //必須要再呼叫一次要讓FA 知道這個授權是kkday做的!而不是robot
            string result = helper.PaymentValid(transNo, pmgwValidToken);

            var obj = JObject.Parse(result);
            if (obj["isSuccess"].ToString() == "True")
            {
                //如果ok就upd
                distributorInfo fakeContact = DataSettingRepository.fakeContact();
                //helper.PayUpdSuccessUpdOrder(id, transNo, payDtl, req, res, fakeContact);//舊版
                helper.PayUpdSuccessUpdOrder2(mid, transNo, payDtl, req, res, fakeContact); //新版
            } 

            return result;
        }


        [HttpGet("AuthFailure")]
        public String AuthFailure(string mid)
        {
            OrderRepository rep = new OrderRepository();
            JObject obj = rep.AuthFailure(mid) as JObject;
            return obj["content"]["result"].ToString();


        }
    }
}
