using System;
using System.Collections.Generic;
namespace KKday.Web.B2D.EC.Models.Model.Pmch
{
    public class PmchModel
    {
        //public PmchModel()
        //{
        //}
    }

    public abstract class CallJson
    {

    }


    #region 找出可使用的pmch list

    public class CallJsonGetPayList : CallJson
    {

        public List<payTypeValue> conditionList { get; set; }
    }

    public class payTypeValue
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    #endregion


    #region pmchlist回傳值

    public class PmchLstResponse
    {
        public Boolean isSuccess { get; set; }
        public List<Pmgw> pmchlist { get; set; }
    }

    public class Pmgw
    {
        public string pmchOid { get; set; }
        public string pmchCode { get; set; }
        public string pmchPayURL { get; set; }
        public string is3D { get; set; }
        public string acctdocReceiveMethod { get; set; }
        public string version { get; set; }
        public InterfaceSetting2 interfaceSetting { get; set; }
    }

    public class InterfaceSetting2
    {
        public string isNeedCardInput { get; set; }
        public List<LogoList2> logoList { get; set; }
        public List<string>acceptedCardTypeList { get; set; }
        public string acceptedCurrency { get; set; }
        public List<string> otherInfoList { get; set; }
    }

    public class LogoList2
    {
        public string logoName { get; set; }
        public string logoUrl { get; set; }
        public List<string> AcceptedCardTypeList { get;set; }
    }





    #endregion 


    public class PmchSslRequest
    {
        public string apiKey { get; set; }
        public string userOid { get; set; }
        public string ver { get; set; }
        public string ipaddress { get; set; }
        public CallJson json { get; set; }
    }

    #region 組出可以送出授權的json

    public class CallJsonPay : CallJson
    {
        public string pmchOid { get; set; }
        public string is3D { get; set; }
        public string payCurrency { get; set; }
        public double payAmount { get; set; }
        public string returnURL { get; set; }
        public string cancelURL { get; set; }
        public string userLocale { get; set; }
        public string paymentParam1 { get; set; }
        public string paymentParam2 { get; set; }
        public PaymentSourceInfo paymentSourceInfo { get; set; }
        public CreditCardInfo creditCardInfo { get; set; }
        public PayerInfo payerInfo { get; set; }
        public PayProductInfo productInfo { get; set; }
        public PayMember member { get; set; }

    }


    public class PmchSslRequest3  //新版
    {
        public string api_key { get; set; }
        public string user_oid { get; set; }
        public string ver { get; set; }
        public string lang_code { get; set; } //新版的有語言環境
        public string ipaddress { get; set; }
        public CallJson json { get; set; }
    }


    public class CallJsonPay2 : CallJson //新版
    {
        public string pmch_oid { get; set; }
        public string is_3d { get; set; }
        public string pay_currency { get; set; }
        public double pay_amount { get; set; }
        public string return_url { get; set; }
        public string cancel_url { get; set; }
        public string user_locale { get; set; }
        public string logo_url { get; set; }
        public string paymentParam1 { get; set; }
        public string paymentParam2 { get; set; }
        public payment_source_info payment_source_info { get; set; }
        public credit_card_info credit_card_info { get; set; }
        public payer_info payer_info { get; set; }
        public product_info product_info { get; set; }
        public member member { get; set; }

    }





    public class PaymentSourceInfo
    {
        public string sourceType { get; set; }
        public string orderMid { get; set; }
    }

    public class CreditCardInfo
    {
        public string cardHolder { get; set; }
        public string cardType { get; set; }
        public string cardNo { get; set; }
        public string cardCvv { get; set; }
        public string cardExp { get; set; }
    }

    public class PayerInfo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

    }

    public class PayProductInfo
    {

        public string prodOid { get; set; }
        public string prodName { get; set; }
    }

    public class PayMember
    {
        public string memberUuid { get; set; }
        public string riskStatus { get; set; }

    }


    //新版
    public class payment_source_info
    {
        public string source_type { get; set; } //KKDAY (本站)CHANNEL(分銷)ONLINE_POST(線上刷卡機)
        public string order_mid { get; set; }
    }

    public class credit_card_info //信用卡資訊
    {
        public string card_holder { get; set; } //卡片持有人
        public string card_type { get; set; } //卡別
        public string card_no { get; set; } //卡號(加密過的)
        public string card_cvv { get; set; } //CVV
        public string card_exp { get; set; } //到期年月yyyymm

    }

    public class payer_info //付款人資訊
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }

    public class member
    {
        public string member_uuid { get; set; } //會員編號
        public string risk_status { get; set; } //風險狀態代碼另外想1.白名單012.一般02 defult3.可疑03 4.黑名單04
        public string ip { get; set; }
    }

    public class product_info
    {
        public string prod_oid { get; set; }
        public string prod_name { get; set; }

    }




    public class PaymentDtl
    {
        public string paymentToken { get; set; } //md5 ordermid + member+uuid + "kk%$#@pay"
        public string orderMid { get; set; }
        public string currency { get; set; }
        public double currTotalPrice { get; set; }
        public string payMethod { get; set; }
    }

    #endregion

    #region 授權回傳

    public class PmchSslResponse
    {
        public Boolean isSuccess { get; set; }
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
        public string pmgwTransNo { get; set; }
        public string pmgwMethod { get; set; }
        public string transactionCode { get; set; }
        public string payCurrency { get; set; }
        public decimal payAmount { get; set; }
        public Boolean is3D { get; set; }
        public PmchSslMemberInfo memberInfo { get; set; }
        public string isFraud { get; set; }
        public string riskNote { get; set; }
    }

    public class PmchSslMemberInfo
    {
        public string encodeCardNo { get; set; }
    }


    public class PmchValid
    {
        public string mid { get; set; }
        public PmchSslResponse2 jsondata { get; set; }

    }

    public class PmchSslResponse2
    {
        public metadata metadata { get; set; }
        public Jsondata data { get; set; }
     }

    public class metadata
    {
        public string status { get; set; }
        public string desc { get; set; }
    }

    public class Jsondata
    {
        public string pmgw_trans_no { get; set; }
        public string pmgw_method { get; set; }
        public string transaction_code { get; set; }  
        public string pay_currency { get; set; }
        public decimal pay_amount { get; set; }
        public Boolean is_3d { get; set; }
        public member_info member_info { get; set; }
        public string is_fraud { get; set; }
        public string risk_note { get; set; }

    }

    public  class member_info
    {
        public string encode_card_no { get; set; }
    }


    #endregion


    #region 驗證
    public class PmchSslRequest2
    {
        public string apiKey { get; set; }
        public string userOid { get; set; }
        public string ver { get; set; }
        public string ipaddress { get; set; }
        public CallPmchValidJson json { get; set; }
    }

    public class CallPmchValidJson
    {
        public string pmgwTransNo { get; set; }
        public string pmgwValidToken { get; set; }

    }
    #endregion


    #region  付款成功，變更訂單狀態為已付款可處理


    public class PaySuccessUpdOrderMst
    {
        public string apiKey { get; set; }
        public string userOid { get; set; }
        public string ver { get; set; }
        public string ipaddress { get; set; }
        public string locale { get; set; }
        public PaySuccessUpdOrder json { get; set; }
    }

    public class PaySuccessUpdOrder
    { 
        public string memberUuid { get; set; }
        public string tokenKey { get; set; }
        public string deviceId { get; set; }
        public string pmgwTransNo { get; set; }
        public string currency { get; set; }
        public string currTotalPrice { get; set; }
        public string payMethod { get; set; }
        public Boolean is3D { get; set; }
        public string pmgwMethod { get; set; }
        public string isFraud { get; set; }
    }

    #endregion




}
