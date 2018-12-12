using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.EC.Models.Model.Booking
{
    public class ApiSetting
    {
        public string apiKey { get; set; }
        public string userOid { get; set; }
        public string ver { get; set; }
        public string locale { get; set; }
        public string ipaddress { get; set; }
        public string currency { get; set; }
        public OrderModel json { get; set; }

    }

    //轉為java api order/new 的model
    public class OrderModel
    {
        public string productOid { get; set; } //商品編號
        public string packageOid { get; set; }  //套餐編號
        public string memberUuid { get; set; } //會員編號
        public string contactFirstname { get; set; } //聯絡人名
        public string contactLastname { get; set; } //聯絡人姓
        public string contactEmail { get; set; } //聯絡人Email
        public string telCountryCd { get; set; } //聯絡人電話國碼
        public string contactTel { get; set; } //聯絡人電話
        public string contactCountryCd { get; set; } //聯絡人國籍
        public string lstGoDt { get; set; } //出發日期 yyyyMMdd
        public string eventOid { get; set; } //場次編號
        public string eventBackupData { get; set; } //候補場次資料候補序/日期/場次編號，用逗號隔開1/20150105/1,2/20150105/11
        public int? price1Qty { get; set; } //大人數量
        public int? price2Qty { get; set; } //小孩數量
        public int? price3Qty { get; set; } //嬰兒數量
        public int? price4Qty { get; set; } //老人數量
        public string payMethod { get; set; } //付款方式
        public string deviceId { get; set; } //設備ID，網站sessionID
        public string tokenKey { get; set; } //使用者專屬認證用金鑰
        public string crtDevice { get; set; } //裝置
        public string crtBrowser { get; set; } //瀏覽器
        public string crtBrowserVersion { get; set; } //瀏覽器版本
        //public string couponUuid { get; set; } //折價券號
        //public float? priceCoupon { get; set; } //折價金額
        public string adCampaign { get; set; } //廣告活動
        public string sourceCode { get; set; } //訂單來源代號
        public string sourceParam1 { get; set; } //傳入參數1
        //public string sourceParam2 { get; set; } //傳入參數2
        //public string sourceParam3 { get; set; } //傳入參數3
        public string guideLang { get; set; } //使用者選擇的導覽語言
        public string note { get; set; } //訂單備註
        public string hasRank { get; set; } //pricetype
        //public cardInfo card { get; set; }  //信用卡資料
        //public string asiaMileMemberNo { get; set; } //亞洲萬里通編號
        //public string asiaMileMemberFirstName { get; set; } //亞洲萬里通旅客姓
        //public string asiaMileMemberLastName { get; set; } //亞洲萬里通旅客名
        public string multipricePlatform { get; set; } //多幣別平台01:WEB02:APP預設為01
        public string currency { get; set; } //付款幣別
        public string currPriceTotal { get; set; } //付款金額
        public List<CusDataInfo> orderCusList { get; set; } //旅客資料
        public List<modules> modules { get; set; } //商品模組

        public int? productUrlOid { get; set; }
        public string productName { get; set; }
        public string[] productCity { get; set; }
        public string productCountry { get; set; }
        public string productMainCat { get; set; }
        public string productOrderHandler { get; set; }
        public string payPmchOid { get; set; }
        public string couponFailureCode { get; set; }
        public string[] allowedCardNumberArray { get; set; }
        public Boolean alsoUpdateMember { get; set; }
        public string riskStatus { get; set; }

        public Pmch pmch { get; set; }
    }

   


    public class Pmch
    {
        public string pmchOid { get; set; }
        public string pmchCode { get; set; }
        public string pmchPayURL { get; set; }
        public string is3D { get; set; }
        public string acctdocReceiveMethod { get; set; }
        public InterfaceSetting interfaceSetting { get; set; }
        public string title { get; set; }
        public object bg_class { get; set; }
        public List<string> card_class_list { get; set; }
    }

    public class InterfaceSetting
    {
        public string isNeedCardInput { get; set; }
        public List<LogoList> logoList { get; set; }
        public List<string> acceptedCardTypeList { get; set; }
        public string acceptedCurrency { get; set; }
        public List<string> otherInfoList { get; set; }
    }

    public class LogoList
    {
        public string logoName { get; set; }
        public string logoUrl { get; set; }
    }


    #region  新商品模組

    //
    public abstract class modules
    {
        public string moduleType { get; set; }
    }

    public class omdlFlightInfo : modules
    {
        public FilghtInfoModules moduleData { get; set; }
    }

    public class FilghtInfoModules
    {
        public arrivalInfo arrival { get; set; } //抵達資訊
        public departureInfo departure { get; set; } //出發資訊
    }
    //

    public class omdlShuttleDate : modules
    {
        public ShuttleModules moduleData { get; set; }
    }

    public class ShuttleModules
    {
        //OMDL_SHUTTLE
        public string shuttleDate { get; set; } //接送日期 
        public designatedLocationInfo designatedLocation { get; set; } //指定接駁車可接送地點
        public designatedByCustomerInfo designatedByCustomer { get; set; } //指定接駁車可接送範圍（客人自行輸入上下車地址）
        public charterRouteInfo charterRoute { get; set; } //包車路線
    }

    public class omdlRentCar : modules//取車營業所
    {
        public RentCarModules moduleData { get; set; }
    }

    public class RentCarModules
    {
        //OMDL_RENT_CAR
        public pickUpInfo_forCar pickup { get; set; } //取車營業所 
        public dropOffInfo_forCar dropff { get; set; }//還車營業所
        public Boolean? isNeedFreeWiFi { get; set; } //是否需要免費Wifi機
        public Boolean? isNeedFreeGPS { get; set; } //是否需要免費GPS
    }

    public class omdlPsgrData : modules
    {
        public PsgDataModules moduleData { get; set; }
    }

    public class PsgDataModules
    {
        //OMDL_PSGR_DATA
        public int? qtyAdult { get; set; } //成人數量
        public int? qtyChild { get; set; } //孩童數量
        public int? qtyInfant { get; set; } //嬰兒數量
        public qtyChildSeatInfo qtyChildSeat { get; set; } //兒童座椅
        public qtyInfantSeatInfo qtyInfantSeat { get; set; } //嬰兒座椅
        public int? qtyCarryLuggage { get; set; }//手提行李件數
        public int? qtyCheckedLuggage { get; set; } //托運行李件數

    }

    public class omdlSendData : modules
    {
        public SendDataModules moduleData { get; set; }

    }

    public class SendDataModules
    {
        //OMDL_SEND_DATA
        public receiverNameInfo receiverName { get; set; } //收件人姓名
        public receiverTelInfo receiverTel { get; set; } //收件人聯絡電話
        public sendToCountryInfo sendToCountry { get; set; } //寄送國家
        public sendToHotelInfo sendToHotel { get; set; } //寄送飯店
        public shipInfoInfo shipInfo { get; set; } //寄送資料(僅供 Be2 使用)

    }


    public class omdlOtherData : modules
    {

        public OtherDataModules moduleData { get; set; }
    }

    public class OtherDataModules
    {
        //OMDL_OTHER_DATA
        public string mobileModelNumber { get; set; } //手機型號
        public string mobileIMEI { get; set; } //IMEI
        public string activationDate { get; set; } //開通日期 
        public string exchangeLocationID { get; set; } //取票櫃檯ID 

    }


    public class omdlContactData : modules
    {
        public ContactDataModules moduleData { get; set; }
    }

    public class ContactDataModules
    {
        //OMDL_CONTACT_DATA
        public contactNameInfo contactName { get; set; } //聯絡人姓名
        public contactTelInfo contactTel { get; set; } //旅遊期間聯絡電話
        public contactAppInfo contactApp { get; set; } //APP聯絡方式

    }


    #endregion
}
