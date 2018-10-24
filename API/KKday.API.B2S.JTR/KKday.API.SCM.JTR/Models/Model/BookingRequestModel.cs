using System;
using System.Collections.Generic;

namespace KKday.API.B2S.JTR.Models.Model
{
    public class BookingRequestModel
    {
        public string sup_id { get; set; } //登入JTR帳號
        public string sup_key { get; set; } //登入JTR密碼

        public string source { get; set; }
        public Order order { get; set; }

    }


    public class EventBackupData
    {
        public int eventSort { get; set; }
        public string eventDate { get; set; }
        public string eventTime { get; set; }
    }

    public class OrderCusList
    {
        public string cusLastname { get; set; }
        public string cusFirstname { get; set; }
        public string cusGender { get; set; }
        public object passportId { get; set; }
        public object cusBirthday { get; set; }
        public object countryCd { get; set; }
    }

    public class Order
    {
        public string packageOid { get; set; }
        public string orderMid { get; set; }
        public string contactTelCd { get; set; }
        public string contactTel { get; set; }
        public string contactFirstname { get; set; }
        public string contactLastname { get; set; }
        public string crtDt { get; set; }
        public string begLstGoDt { get; set; }
        public string endLstBackDt { get; set; }
        public int price1Qty { get; set; }
        public int price2Qty { get; set; }
        public int price3Qty { get; set; }
        public int price4Qty { get; set; }
        public double currPrice1 { get; set; }
        public double currPrice2 { get; set; }
        public double currPrice3 { get; set; }
        public double currPrice4 { get; set; }
        public string eventTime { get; set; }
        public List<EventBackupData> eventBackupData { get; set; }
        public List<OrderCusList> orderCusList { get; set; }
    }



}
