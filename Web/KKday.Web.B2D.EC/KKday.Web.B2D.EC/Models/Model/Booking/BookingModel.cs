using System;
using System.Collections.Generic;
namespace KKday.Web.B2D.EC.Models.Model.Booking
{
    public class BookingModel
    {

    }

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
