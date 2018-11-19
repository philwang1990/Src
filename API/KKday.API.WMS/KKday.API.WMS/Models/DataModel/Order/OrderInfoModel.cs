using System;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Order
{
    public class OrderInfoModel
    {
        public string result { get; set; }
        public string result_msg { get; set; }
        public string order_handler { get; set; }

        //行程介紹
        //地圖&行程表
        public List<modules> order_modules { get; set; } //訂購資料
        public List<CusDataInfo> order_cusList { get; set; } //旅客資料 OMDL_CUST_DATA

        public OrderDtl order_info { get; set; }
    }


    public abstract class modules
    {
        public string module_type { get; set; }
    }


    public class omdlFlightInfo : modules
    {
        public FilghtInfoModules module_data { get; set; }
    }

    public class FilghtInfoModules : modules
    {
        public arrivalInfo arrival { get; set; } //抵達資訊
        public departureInfo departure { get; set; } //出發資訊
    }

    public class omdlShuttleDate : modules
    {
        public ShuttleModules module_data { get; set; }
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
        public RentCarModules module_data { get; set; }
    }

    public class RentCarModules : modules
    {
        //OMDL_RENT_CAR
        public pickUpInfo_forCar pickup { get; set; } //取車營業所 
        public dropOffInfo_forCar dropff { get; set; }//還車營業所
        public Boolean? isNeedFreeWiFi { get; set; } //是否需要免費Wifi機
        public Boolean? isNeedFreeGPS { get; set; } //是否需要免費GPS
    }

    public class omdlPsgrData : modules
    {
        public PsgDataModules module_data { get; set; }
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
        public SendDataModules module_data { get; set; }

    }

    public class SendDataModules
    {
        //OMDL_SEND_DATA
        public receiverNameInfo receiverName { get; set; } //收件人姓名
        public receiverTelInfo receiverTel { get; set; } //收件人聯絡電話
        public receiveAddressInfo sendToCountry { get; set; } //寄送國家
        public sendToHotelInfo sendToHotel { get; set; } //寄送飯店
        public shipInfoInfo shipInfo { get; set; } //寄送資料(僅供 Be2 使用)

    }


    public class omdlOtherData : modules
    {

        public OtherDataModules module_data { get; set; }
    }

    public class OtherDataModules
    {
        //OMDL_OTHER_DATA
        public string mobileModelNumber { get; set; } //手機型號
        public string mobileIMEI { get; set; } //IMEI
        public string activationDate { get; set; } //開通日期 
        public string exchangeLocationID { get; set; } //取票櫃檯ID 
        public string exchangeLocationName { get; set; } //取票櫃檯ID 
        public string exchangeLocationAddress { get; set; } //取票櫃檯ID 
        public string exchangeLocationNote { get; set; } //取票櫃檯ID 

    }


    public class omdlContactData : modules
    {
        public ContactDataModules module_data { get; set; }
    }

    public class ContactDataModules
    {
        //OMDL_CONTACT_DATA
        public contactNameInfo contactName { get; set; } //聯絡人姓名
        public contactTelInfo contactTel { get; set; } //旅遊期間聯絡電話
        public contactAppInfo contactApp { get; set; } //APP聯絡方式

    }


    #region
   

    public class OrderDtl
    {
        public string orderNo { get; set; } //b2d order_no
        public string orderOid { get; set; } //kkday orderOid
        public string orderMid { get; set; } //kkday orderMid
        public string crtDt { get; set; }
        public string userCrtDt { get; set; }
        public string userCrtDtGMTNm { get; set; }
        public string orderStatus { get; set; }
        public string orderStatusTxt { get; set; }
        public int confirmHour { get; set; }
        public string lstStatus { get; set; }
        public string takeStatus { get; set; }
        public string opStatus { get; set; }
        public string cashflowStatus { get; set; }
        public string cancelStatus { get; set; }
        public string rejectStatus { get; set; }
        public string collectStatus { get; set; }
        public string refundStatus { get; set; }
        public string memberUuid { get; set; }
        public string contactFirstname { get; set; }
        public string contactLastname { get; set; }
        public string contactEmail { get; set; }
        public string telCountryCd { get; set; }
        public string contactTel { get; set; }
        public string contactCountryCd { get; set; }
        public string productOid { get; set; }
        public string productName { get; set; }
        public string packageOid { get; set; }
        public string packageName { get; set; }
        public int qtyTotal { get; set; }
        public string cancelStatus1Dt { get; set; }
        public string cancelStatus1DtGMTNm { get; set; }
        public object cancelStatus1UserName { get; set; }
        public string cancelStatus3Dt { get; set; }
        public object cancelStatus3UserName { get; set; }
        public string refundStatus3Dt { get; set; }
        public string takeStatus2Dt { get; set; }
        public string takeStatus2UserName { get; set; }
        public string takeStatus3Dt { get; set; }
        public string takeStatus3UserName { get; set; }
        public string cancelTxt { get; set; }
        public string cancelTxtSup { get; set; }
        public string cancelTxtMem { get; set; }
        public string begLstGoDt { get; set; }
        public string begLstGoDtGMT { get; set; }
        public string begLstGoDtGMTNm { get; set; }
        public string endLstBackDt { get; set; }
        public double price { get; set; }
        public int price1Qty { get; set; }
        public int price2Qty { get; set; }
        public int price3Qty { get; set; }
        public int price4Qty { get; set; }
        public double pricePay { get; set; }
        public int priceRefund { get; set; }
        public int priceCoupon { get; set; }
        public string couponUuid { get; set; }
        public int unreadMsg { get; set; }
        public double priceTotal { get; set; }
        public string imgUrl { get; set; }
        public string kkdayImgUrl { get; set; }
        public int feeCancelSup { get; set; }
        public int feeCancelKkday { get; set; }
        public object msgOid { get; set; }
        public object recOid { get; set; }
        public string priceType { get; set; }
        public string timezone { get; set; }
        public object pickupFrom { get; set; }
        public string supplierName { get; set; }
        public object dropoffTo { get; set; }
        public string payMethod { get; set; }
        public bool overdue { get; set; }
        public string productNameMaster { get; set; }
        public string packageNameMaster { get; set; }
        public string productNameUserLang { get; set; }
        public string packageNameUserLang { get; set; }
        public string countryCd { get; set; }
        public List<string> cityCd { get; set; }
        public string cityName { get; set; }
        public string countryName { get; set; }
        public string countryShortNm { get; set; }
        public double price1 { get; set; }
        public int price2 { get; set; }
        public int price3 { get; set; }
        public int price4 { get; set; }
        public object agencyNo { get; set; }
        public object orderMemo { get; set; }
        public object photoUrl { get; set; }
        public string cancelStatus1DtAdd48h { get; set; }
        public string cancelStatus1DtAdd48hGMTNm { get; set; }
        public object rejectStatus1UserName { get; set; }
        public object rejectStatus2UserName { get; set; }
        public object rejectStatus1Dt { get; set; }
        public object rejectStatus2Dt { get; set; }
        public string lastProcessSupplierUserName { get; set; }
        public string lastProcessDt { get; set; }
        public int cancelApplyDifferHour { get; set; }
        public int recScores { get; set; }
        public string cancelCode { get; set; }
        public string packageUnit { get; set; }
        public string packageUnitTxt { get; set; }
        public string pickupTpTxt { get; set; }
        public object pickupTp { get; set; }
        public object pickupInfo { get; set; }
        public Guide guide { get; set; }
        //public List<OrderCusList> orderCusList { get; set; }
        public string currCd { get; set; }
        public double currRate { get; set; }
        public int currPrice { get; set; }
        public int currPricePay { get; set; }
        public int currPriceRefund { get; set; }
        public int currPriceCoupon { get; set; }
        public int currPriceTotal { get; set; }
        public int currPrice1 { get; set; }
        public int currPrice2 { get; set; }
        public int currPrice3 { get; set; }
        public int currPrice4 { get; set; }
        public int currFeeCancelSup { get; set; }
        public int currFeeCancelKkday { get; set; }
        public int currFeeCancel { get; set; }
        public string pmgwCurrCd { get; set; }
        public double pmgwCurrRate { get; set; }
        public int pmgwCurrPriceTotal { get; set; }
        public int pmgwCurrPriceRefund { get; set; }
        public int pmgwCurrFeeCancel { get; set; }
        public string prodMainCat { get; set; }
        public List<string> tag { get; set; }
        public string note { get; set; }
        public string guideLang { get; set; }
        public string isHasEvent { get; set; }
        public string eventTime { get; set; }
        public string eventBackup { get; set; }
        public object cancelApplyNote { get; set; }
        public object cancelReturnNote { get; set; }
        public string hasAirInfo { get; set; }
        public string asiaMileNo { get; set; }
        public string asiaMileFirstName { get; set; }
        public string asiaMileLastName { get; set; }
        public bool canCancel { get; set; }
        public bool hasVoucher { get; set; }
    }

    public class Guide
    {
        public string guideName { get; set; }
        public string guideTelArea { get; set; }
        public string guideTel { get; set; }
    }

    //public class OrderCusList
    //{
    //    public string cusLastname { get; set; }
    //    public string cusFirstname { get; set; }
    //    public string cusGender { get; set; }
    //    public string passportId { get; set; }
    //    public string cusBirthday { get; set; }
    //    public object cusEmail { get; set; }
    //    public object cusTel { get; set; }
    //    public string countryCd { get; set; }
    //    public object countryName { get; set; }
    //}

    #endregion





}
