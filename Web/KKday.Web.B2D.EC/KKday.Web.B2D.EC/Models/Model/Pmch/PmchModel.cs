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


    public class CallPmchReq
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
        public string riskStatus { get; set;}

    }

    #endregion


}
