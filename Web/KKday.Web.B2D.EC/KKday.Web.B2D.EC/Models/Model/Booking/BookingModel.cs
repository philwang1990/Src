﻿using System;
using System.Collections.Generic;
namespace KKday.Web.B2D.EC.Models.Model.Booking
{
    public class BookingModel
    {

    }


    //分銷商資料（暫時）
    //public class distributorInfo
    //{
    //    public string companyXid { get; set; }
    //    public string userid { get; set; } //UserUUID
    //    public string firstName { get; set; }
    //    public string lastName { get; set; }
    //    public string areatel { get; set; }
    //    public string tel { get; set; }
    //    public string countryCd { get; set; }
    //    public string email { get; set; }
    //    public string lang { get; set; }
    //    public string currency { get; set; }
    //    public string state { get; set; }
    //    public string channelOid { get; set; }

    //    public string memberUuid { get; set; }
    //    public string tokenKey { get; set; }
    //    public string deviceId  {get;set;}
    //}


    public class EventQury
    {
        public string day { get; set; }
        public string guid { get; set; }
    }

    public class returnBookingEventStatus
    {
        public string status { get; set; }
        public List<string> dayevent { get; set; }
        public string msgErr { get; set; }
    }
}
