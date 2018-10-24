using System;
using KKday.Web.B2D.EC.Models.Model.Product;
using System.Collections.Generic;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Model.Booking;
using KKday.Web.B2D.EC.Models.Model.Pmch;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace KKday.Web.B2D.EC.Models.Repostory.Booking
{
    public static class BookingRepostory
    {

        public static DataModel setDefaultBookingInfo(DataModel data ,ProductModel prod, PkgDetailModel pkg, confirmPkgInfo confirm, distributorInfo distributor)
        {
            data.productOid = confirm.prodOid;
            data.packageOid = confirm.pkgOid;
            data.contactFirstname = distributor.firstName;
            data.contactLastname = distributor.lastName;
            data.contactEmail = distributor.email;
            data.telCountryCd = distributor.areatel;
            data.contactTel = distributor.tel;
            data.contactCountryCd = distributor.countryCd;
            data.lstGoDt = confirm.selDate;
            if (confirm.pkgEvent != null) data.eventOid = confirm.pkgEvent;

            data.price1Qty = confirm.price1Qty;
            data.price2Qty = confirm.price2Qty == null ? 0 : confirm.price2Qty;
            data.price3Qty = confirm.price3Qty==null?0: confirm.price3Qty;
            data.price4Qty = confirm.price4Qty == null ? 0 : confirm.price4Qty;
            data.payMethod = "ONLINE_CITI";
            data.hasRank = pkg.is_unit_pirce == "RANK" ? "Y" : "N";
            //data.productUrlOid = 
            data.productName = prod.prod_name;
            string[] citys = new string[prod.countries[0].cities.Count];

            int i = 0;
            foreach(City c in prod.countries[0].cities)
            {
                citys[i] = c.id;
                i++;
            }

            data.productCity = citys;
            data.productCountry = prod.countries[0].id;
            data.productMainCat = prod.prod_type;
            data.productOrderHandler = prod.prod_hander;
            data.payPmchOid = "1";
            data.currency = distributor.currency;
            data.currPriceTotal = 263;// (pkg.price1 * confirm.price1Qty) +(pkg.price2 * confirm.price2Qty) +(pkg.price3 * confirm.price3Qty) + (pkg.price4 * confirm.price4Qty);
            data.crtDevice = "Macintosh";
            data.crtBrowser = "Safari";
            data.crtBrowserVersion = "12.0";
            data.memberUuid = "051794b8-db2a-4fe7-939f-31ab1ee2c719";
            data.riskStatus = "01";
            data.tokenKey = "897af29c45ed180451c2e6bfa81333b6";
            data.deviceId = "3c2ab71448224d1d7148350f7972e96e";
            data.multipricePlatform = "01";
            data.sourceCode = "WEB";
            data.sourceParam1 = "";
            data.allowedCardNumberArray = new string[]{};

            //senddata
            data.modules.sendData.moduleData.receiverTel.telCountryCode = distributor.areatel;
            data.modules.sendData.moduleData.receiverTel.telNumber = distributor.tel;

            //contact
            data.modules.contactData.moduleData.contactTel.telCountryCode = distributor.areatel;

            //rendCar 寫在 js

            return data;

        }


        public static string  setPaymentInfo(ProductModel prod,OrderModel OrderModel,string orderMid)
        {
            CallPmchReq pmch = new CallPmchReq();

            pmch.apiKey = "kkdayapi";
            pmch.userOid = "1";
            pmch.ver = "1.0.1";
            pmch.ipaddress = "127.0.0.1";

            CallJsonPay json = new CallJsonPay();

            json.pmchOid = OrderModel.payPmchOid;
            json.is3D = "0";
            json.payCurrency = OrderModel.currency;
            json.payAmount = Convert.ToDouble(OrderModel.currPriceTotal) ;
            json.returnURL = "https://localhost:5001/Home";
            json.cancelURL = "https://localhost:5001/Product/20159";
            json.userLocale = "zh-tw";
            json.paymentParam1 = "";
            json.paymentParam2 = "";

            PaymentSourceInfo pay = new PaymentSourceInfo();
            pay.sourceType = "KKDAY";
            pay.orderMid = orderMid;

            json.paymentSourceInfo = pay;

            CreditCardInfo credit = new CreditCardInfo();
            credit.cardHolder = "phil";
            credit.cardNo = "4093240835103617";
            credit.cardType = "VISA";
            credit.cardCvv = "143";
            credit.cardExp = "202310";

            json.creditCardInfo = credit;

            PayerInfo payer = new PayerInfo();
            payer.firstName = "ming";
            payer.lastName = "chen";
            payer.phone = "0939650222";
            payer.email = "phil.chang@kkday.com";

            json.payerInfo = payer;

            PayProductInfo prodInfo = new PayProductInfo();
            prodInfo.prodName = prod.prod_name;
            prodInfo.prodOid = prod.prod_no.ToString();

            json.productInfo = prodInfo;

            PayMember member = new PayMember();
            member.memberUuid = OrderModel.memberUuid;
            member.riskStatus = "01";

            json.member = member;

            pmch.json = json;

            return JsonConvert.SerializeObject(pmch);

        }

    }
}
