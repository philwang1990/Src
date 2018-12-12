﻿using System;
using System.Collections.Generic;
namespace KKday.Web.B2D.EC.Models.Model.Booking
{

    //前端接收 fill data的model
    public class DataModel
    {
        public string guidNo { get; set; }
        public string productOid { get; set; }
        public string packageOid { get; set; }
        public string contactFirstname { get; set; }
        public string contactLastname { get; set; }
        public string contactEmail { get; set; }
        public string telCountryCd { get; set; }
        public string contactTel { get; set; }
        public string contactCountryCd { get; set; }
        public string lstGoDt { get; set; }
        public string eventOid { get; set; }
        public string eventBackupData { get; set; }
        public int? price1Qty { get; set; }
        public int? price2Qty { get; set; }
        public int? price3Qty { get; set; }
        public int? price4Qty { get; set; }
        public string payMethod { get; set; }
        public string guideLang { get; set; }
        public string note { get; set; }
        public string hasRank { get; set; }
        public int? productUrlOid { get; set; }
        public string productName { get; set; }
        public string[] productCity { get; set; }
        public string productCountry { get; set; }
        public string productMainCat { get; set; }
        public string productOrderHandler { get; set; }
        public string payPmchOid { get; set; }
        public string useCoupon { get; set; }
        public string couponUuid { get; set; }
        public float? priceCoupon { get; set; }
        public float? priceCouponUSD { get; set; }
        public string couponFailureCode { get; set; }
        public string[] allowedCardNumberArray { get; set; }
        public Boolean alsoUpdateMember { get; set; }
        public string accumulatedAisaMile { get; set; }
        public string asiaMileMemberNo { get; set; }
        public string asiaMileMemberFirstName { get; set; }
        public string asiaMileMemberLastName { get; set; }
        public string adCampaign { get; set; }
        public cardInfo card { get; set; }
        public string currency { get; set; }
        public double? currPriceTotal { get; set; }
        public string crtDevice { get; set; }
        public string crtBrowser { get; set; }
        public string crtBrowserVersion { get; set; }
        public string memberUuid { get; set; }
        public string riskStatus { get; set; }
        public string tokenKey { get; set; }
        public string deviceId { get; set; }
        public string multipricePlatform { get; set; }
        public string sourceCode { get; set; }
        public string sourceParam1 { get; set; }
        public string ip { get; set; }  //原本主檔 json要帶的
        public string locale { get; set; } //原本主檔 json要帶的

        public List<CusDataInfo> travelerData { get; set; }
        public modulesData modules { get; set; }


        public DataModel Clone()
        {
            return (DataModel)this.MemberwiseClone();
        }

    }


    public class modulesData
    {
        public flightInfoDataM flightInfoData { get; set; }
        public shuttleDataM shuttleData { get; set; }
        public carRentingDataM carRentingData { get; set; }
        public passengerDataM passengerData { get; set; }
        public sendDataM sendData { get; set; }
        public contactDataM contactData { get; set; }
        public otherDataM otherData { get; set; }
    }


    public class flightInfoDataM
    {
        public string moduleType { get; set; } //OMDL_FLIGHT_INFO
        public moduleData_FlightInfo moduleData { get; set; }
    }

    public class moduleData_FlightInfo
    {
        public arrivalInfo arrival { get; set; }
        public departureInfo departure { get; set; }
    }

    public class shuttleDataM
    {
        public string moduleType { get; set; } //OMDL_SHUTTLE
        public moduleData_Shuttle moduleData { get; set; }

    }

    public class moduleData_Shuttle
    {
        public string shuttleDate { get; set; }
        public designatedLocationInfo designatedLocation { get; set; }
        public designatedByCustomerInfo designatedByCustomer { get; set; }
        public charterRouteInfo charterRoute { get; set; }
    }

    public class carRentingDataM
    {
        public string moduleType { get; set; } //
        public moduleData_CarRent moduleData { get; set; }

    }

    public class moduleData_CarRent
    {
        public pickUpInfo_forCar pickUp { get; set; }
        public dropOffInfo_forCar dropOff { get; set; }
        public Boolean isNeedFreeWiFi { get; set; }
        public Boolean isNeedFreeGPS { get; set; }

    }

    public class passengerDataM
    {
        public string moduleType { get; set; } //OMDL_PSGR_DATA
        public moduleData_passenger moduleData { get; set; }

    }

    public class moduleData_passenger
    {
        public int qtyAdult { get; set; }
        public int qtyChild { get; set; }
        public int qtyInfant { get; set; }
        public int qtyCarryLuggage { get; set; }
        public int qtyCheckedLuggage { get; set; }
        public qtyChildSeatInfo qtyChildSeat { get; set; }
        public qtyInfantSeatInfo qtyInfantSeat { get; set; }

    }

    public class sendDataM
    {
        public string moduleType { get; set; } //OMDL_SEND_DATA
        public moduleData_sendData moduleData { get; set; }
    }

    public class moduleData_sendData
    {
        public receiverNameInfo receiverName { get; set; }
        public receiverTelInfo receiverTel { get; set; }
        public sendToCountryInfo sendToCountry { get; set; }
        public sendToHotelInfo sendToHotel { get; set; }
        public shipInfoInfo shipInfo { get; set; }
    }

    public class contactDataM
    {
        public string moduleType { get; set; } //OMDL_CONTACT_DATA
        public moduleData_contactData moduleData { get; set; }

    }

    public class moduleData_contactData
    {
        public contactNameInfo contactName { get; set; }
        public contactTelInfo contactTel { get; set; }
        public contactAppInfo contactApp { get; set; }

    }

    public class otherDataM
    {
        public string moduleType { get; set; } //OMDL_OTHER_DATA
        public moduleData_otherData moduleData { get; set; }

    }

    public class moduleData_otherData
    {
        public string mobileModelNumber { get; set; }
        public string mobileIMEI { get; set; }
        public string activationDate { get; set; }
        public string exchangeLocationID { get; set; }
    }



    #region OMDL_FLIGHT_INFO


    public class arrivalInfo
    {
        public string flightType { get; set; } //航班類別
        public string airport { get; set; } //抵達機場
        public string terminalNo { get; set; } //航廈
        public string airline { get; set; } //航空公司
        public string flightNo { get; set; } //航班編號
        public arrivalDatetimeInfo arrivalDatetime { get; set; } //抵達時間
        public Boolean? isNeedToApplyVisa { get; set; } //是否辦理落地簽

    }

    public class departureInfo
    {
        public string flightType { get; set; } //航班類別
        public string airport { get; set; } //出發機場
        public string terminalNo { get; set; } //航廈
        public string airline { get; set; } //航空公司
        public string flightNo { get; set; } //航班編號
        public departureDatetimeInfo departureDatetime { get; set; } //出發時間
        public Boolean? haveBeenInCountry { get; set; } //已在商品所在國家

    }

    public class arrivalDatetimeInfo
    {
        public string date { get; set; } //日期 yyyy-MM-dd
        public int? hour { get; set; } //時
        public int? minute { get; set; } //分
    }

    public class departureDatetimeInfo
    {
        public string date { get; set; }//日期 yyyy-MM-dd
        public int? hour { get; set; } //時
        public int? minute { get; set; } //分
    }

    #endregion

    #region OMDL_SHUTTLE

    public class designatedLocationInfo //指定接駁車可接送地點
    {
        public string locationID { get; set; } //接送地點ID

    }

    public class designatedByCustomerInfo //指定接駁車可接送範圍（客人自行輸入上下車地址）

    {
        public pickUpInfo pickUp { get; set; }  //上車資料
        public dropOffInfo dropOff { get; set; } //下車資料
    }

    public class pickUpInfo
    {
        public timeInfo time { get; set; } //接送時間
        public string location { get; set; }  //地點
    }

    public class timeInfo
    {
        public Boolean? isCustom { get; set; } //是否自訂
        public string timeID { get; set; } //時間ＩＤ
        public int? hour { get; set; } //時
        public int? minute { get; set; } //分

    }

    public class dropOffInfo
    {
        public string location { get; set; } //下車資料

    }

    public class charterRouteInfo //包車路線
    {
        public Boolean? isCustom { get; set; } //旅客是否自訂行程
        public string routesID { get; set; } //路線 ID
        public string[] customRoutes { get; set; } //地點陣列,[“北門”,”東門”,“西門”]

    }

    #endregion

    #region OMDL_RENT_CAR

    public class pickUpInfo_forCar //取車營業所
    {
        public string officeID { get; set; } //營業所ID
        public dateTimeInfo datetime { get; set; } //時間


    }

    public class dateTimeInfo
    {
        public string date { get; set; }
        public int? hour { get; set; }
        public int? minute { get; set; }

    }

    public class dropOffInfo_forCar //還車營業所
    {
        public string officeID { get; set; } //營業所ID
        public dateTimeInfo datetime { get; set; } //時間
    }
    #endregion

    #region OMDL_PSGR_DATA

    public class qtyChildSeatInfo //兒童座椅
    {
        public int? supplierProvided { get; set; } //廠商提供數量
        public int? selfProvided { get; set; } //自備數量

    }

    public class qtyInfantSeatInfo //嬰兒座椅
    {
        public int? supplierProvided { get; set; } //廠商提供數量
        public int? selfProvided { get; set; } //自備數量
    }

    #endregion

    #region OMDL_SEND_DATA

    public class receiverNameInfo //收件人姓名
    {
        public string firstName { get; set; } //姓
        public string lastName { get; set; } //名

    }

    public class receiverTelInfo //收件人聯絡電話
    {
        public string telCountryCode { get; set; } //國碼
        public string telNumber { get; set; } //電話號碼


    }

    public class sendToCountryInfo //電話號碼
    {
        public receiveAddressInfo receiveAddress { get; set; } //收件地址

    }

    public class receiveAddressInfo
    {
        public string countryCode { get; set; } //國碼
        public string cityCode { get; set; } //城市代碼
        public string zipCode { get; set; } //郵遞區號
        public string address { get; set; } //地址

    }

    public class sendToHotelInfo //寄送飯店
    {
        public string hotelName { get; set; } //飯店名稱
        public string hotelAddress { get; set; } //飯店地址
        public string hotelTel { get; set; } //飯店電話
        public buyerPassportEnglishNameInfo buyerPassportEnglishName { get; set; } //訂房人護照英文姓名
        public buyerLocalNameInfo buyerLocalName { get; set; } //訂房人本國籍姓名

        public string bookingOrderNo { get; set; } //訂房編號
        public string bookingWebsite { get; set; } //訂房網站
        public string checkInDate { get; set; } //入住日期
        public string checkOutDate { get; set; } //退房日期
    }

    public class buyerPassportEnglishNameInfo //訂房人護照英文姓名
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class buyerLocalNameInfo //訂房人本國籍姓名
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class shipInfoInfo //寄送資料(僅供 Be2 使用)
    {
        public string shipDate { get; set; }
        public string trackingNumber { get; set; }
    }


    #endregion

    #region OMDL_CONTACT_DATA

    public class contactNameInfo //聯絡人姓名
    {
        public string firstName { get; set; } //姓
        public string lastName { get; set; } //名

    }

    public class contactTelInfo //旅遊期間聯絡電話
    {
        public Boolean? haveTel { get; set; } //是否有電話
        public string telCountryCode { get; set; } //國碼
        public string telNumber { get; set; } //電話號碼

    }

    public class contactAppInfo //APP聯絡方式
    {

        public Boolean? haveApp { get; set; } //是否有APP聯絡方式
        public string appType { get; set; } //APP型別
        public string appAccount { get; set; } //APP帳號
    }


    #endregion






    public class cardInfo
    {
        public string cardNo { get; set; }
        public string cardHolder { get; set; }
        public string expiry { get; set; }
        public string cardCvv { get; set; }
        public string cardType { get; set; }

    }

    //最後要寫入booking的內容
    public class CusDataInfo
    {
        public bool isSave { get; set; }
        public string friendOid { get; set; }
        public englishNameInfo englishName { get; set; }
        public string gender { get; set; }
        public nationalityInfo nationality { get; set; }
        public string birthday { get; set; }
        public passportInfo passport { get; set; }
        public localNameInfo localName { get; set; }
        public heightInfo height { get; set; }
        public weightInfo weight { get; set; }
        public shoeSizeInfo shoeSize { get; set; }
        public mealInfo meal { get; set; }
        public double? glassDiopter { get; set; }


    }

    public class englishNameInfo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class nationalityInfo
    {
        public string nationalityCode { get; set; }
        public string TWIdentityNumber { get; set; }
        public string HKMOIdentityNumber { get; set; }
        public string MTPNumber { get; set; }
    }

    public class passportInfo
    {
        public string passportNo { get; set; }
        public string passportExpDate { get; set; }
    }

    public class localNameInfo
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class heightInfo
    {
        public string unit { get; set; }
        public double? value { get; set; }
    }

    public class weightInfo
    {
        public string unit { get; set; }
        public double? value { get; set; }
    }

    public class shoeSizeInfo
    {
        public string type { get; set; }
        public string unit { get; set; }
        public double? value { get; set; }
    }

    public class mealInfo
    {
        public string mealType { get; set; }
        public string[] excludeFoodType { get; set; }
        public foodAllergyInfo foodAllergy { get; set; }
    }

    public class foodAllergyInfo
    {
        public Boolean isFoodAllergy { get; set; }
        public string allergenList { get; set; }
    }


}

