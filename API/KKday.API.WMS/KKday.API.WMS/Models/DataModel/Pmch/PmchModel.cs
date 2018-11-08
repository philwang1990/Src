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


    public class PmchSslRequest
    {
        public string apiKey { get; set; }
        public string userOid { get; set; }
        public string ver { get; set; }
        public string ipaddress { get; set; }
        public CallJson json { get; set; }
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
