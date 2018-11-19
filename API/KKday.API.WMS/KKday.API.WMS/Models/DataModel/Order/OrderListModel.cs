using System;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Order
{
    public class OrderListModel
    {
        public string result { get; set; }
        public string result_msg { get; set; }
        public int order_qty { get; set; }
        public int current_page { get; set; }
        public List<Order> order { get; set; }

    }
    public class Order
    {
        public string orderNo { get; set; }
        public string orderOid { get; set; }
        public string orderMid { get; set; }
        public string crtDt { get; set; }
        public string userCrtDt { get; set; }
        public string userCrtDtGMTNm { get; set; }
        public string orderStatus { get; set; }
        public string orderStatusTxt { get; set; }
        //public int confirmHour { get; set; }
        public string lstStatus { get; set; }
        //public string takeStatus { get; set; }
        //public string opStatus { get; set; }
        //public string cashflowStatus { get; set; }
        //public string cancelStatus { get; set; }
        //public string rejectStatus { get; set; }
        //public string collectStatus { get; set; }
        //public string refundStatus { get; set; }
        //public string memberUuid { get; set; }
        //public string contactFirstname { get; set; }
        //public string contactLastname { get; set; }
        //public string contactEmail { get; set; }
        //public string telCountryCd { get; set; }
        //public string contactTel { get; set; }
        //public string contactCountryCd { get; set; }
        public string productOid { get; set; }
        public string productName { get; set; }
        public string packageOid { get; set; }
        public string packageName { get; set; }
        public int qtyTotal { get; set; }
        //public string cancelStatus1Dt { get; set; }
        //public string cancelStatus1DtGMTNm { get; set; }
        //public object cancelStatus1UserName { get; set; }
        //public string cancelStatus3Dt { get; set; }
        //public object cancelStatus3UserName { get; set; }
        //public string refundStatus3Dt { get; set; }
        //public string takeStatus2Dt { get; set; }
        //public string takeStatus2UserName { get; set; }
        //public string takeStatus3Dt { get; set; }
        //public string takeStatus3UserName { get; set; }
        //public string cancelTxt { get; set; }
        //public string cancelTxtSup { get; set; }
        //public string cancelTxtMem { get; set; }
        public string begLstGoDt { get; set; }
        public string begLstGoDtGMT { get; set; }
        public string begLstGoDtGMTNm { get; set; }
        public string endLstBackDt { get; set; }
        //public double price { get; set; }
        //public int price1Qty { get; set; }
        //public int price2Qty { get; set; }
        //public int price3Qty { get; set; }
        //public int price4Qty { get; set; }
        //public double pricePay { get; set; }
        //public int priceRefund { get; set; }
        //public int priceCoupon { get; set; }
        //public string couponUuid { get; set; }
        //public int unreadMsg { get; set; }
        //public double priceTotal { get; set; }
        //public string imgUrl { get; set; }
        //public string kkdayImgUrl { get; set; }
        //public int feeCancelSup { get; set; }
        //public int feeCancelKkday { get; set; }
        //public int? msgOid { get; set; }
        //public object recOid { get; set; }
        //public string priceType { get; set; }
        //public string timezone { get; set; }
        //public object pickupFrom { get; set; }
        //public object supplierName { get; set; }
        //public object dropoffTo { get; set; }
        //public string payMethod { get; set; }
        //public bool overdue { get; set; }
        //public string productNameMaster { get; set; }
        //public string packageNameMaster { get; set; }
        //public string productNameUserLang { get; set; }
        //public string packageNameUserLang { get; set; }
        //public string countryCd { get; set; }
        //public List<string> cityCd { get; set; }
        //public string cityName { get; set; }
        //public string countryName { get; set; }
        //public string countryShortNm { get; set; }
        //public double price1 { get; set; }
        //public double price2 { get; set; }
        //public int price3 { get; set; }
        //public int price4 { get; set; }
        //public object agencyNo { get; set; }
        //public object orderMemo { get; set; }
        //public object photoUrl { get; set; }
        //public string cancelStatus1DtAdd48h { get; set; }
        //public string cancelStatus1DtAdd48hGMTNm { get; set; }
        //public object rejectStatus1UserName { get; set; }
        //public object rejectStatus2UserName { get; set; }
        //public object rejectStatus1Dt { get; set; }
        //public object rejectStatus2Dt { get; set; }
        //public string lastProcessSupplierUserName { get; set; }
        //public string lastProcessDt { get; set; }
        //public int cancelApplyDifferHour { get; set; }
        //public int recScores { get; set; }
        //public string cancelCode { get; set; }
        //public string packageUnit { get; set; }
        //public string packageUnitTxt { get; set; }
        //public string pickupTpTxt { get; set; }
        //public object pickupTp { get; set; }
        //public object pickupInfo { get; set; }
        //public Guide guide { get; set; }
        //public List<OrderCusList> orderCusList { get; set; }
        //public string currCd { get; set; }
        //public double currRate { get; set; }
        //public int currPrice { get; set; }
        //public int currPricePay { get; set; }
        //public int currPriceRefund { get; set; }
        //public int currPriceCoupon { get; set; }
        //public int currPriceTotal { get; set; }
        //public int currPrice1 { get; set; }
        //public int currPrice2 { get; set; }
        //public int currPrice3 { get; set; }
        //public int currPrice4 { get; set; }
        //public int currFeeCancelSup { get; set; }
        //public int currFeeCancelKkday { get; set; }
        //public int currFeeCancel { get; set; }
        //public string pmgwCurrCd { get; set; }
        //public double? pmgwCurrRate { get; set; }
        //public int? pmgwCurrPriceTotal { get; set; }
        //public int? pmgwCurrPriceRefund { get; set; }
        //public int? pmgwCurrFeeCancel { get; set; }
        //public string prodMainCat { get; set; }
        //public List<string> tag { get; set; }
        //public string note { get; set; }
        //public string guideLang { get; set; }
        //public string isHasEvent { get; set; }
        //public string eventTime { get; set; }
        //public string eventBackup { get; set; }
        //public object cancelApplyNote { get; set; }
        //public object cancelReturnNote { get; set; }
        //public string hasAirInfo { get; set; }
        //public string asiaMileNo { get; set; }
        //public string asiaMileFirstName { get; set; }
        //public string asiaMileLastName { get; set; }
        //public bool canCancel { get; set; }
        //public bool hasVoucher { get; set; }
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


}
