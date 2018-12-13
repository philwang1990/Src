using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KKday.API.B2S.JTR.Models.Model
{
    public class BookingResponseModel
    {
        public Metadata metadata { get; set; }
        public Data data { get; set; }
    }

    public class Metadata
    {
        public string status { get; set; }
        public string description { get; set; }
    }

    public class Data
    {
        public bool isMuiltSupOrder { get; set; }
        public string supTicketNumber { get; set; }
        public List<string> supQrUrl { get; set; }
        public List<Orderinfo> orderinfo { get; set; }
    }

    public class Orderinfo
    {
        public string priceType { get; set; }
        public int qty { get; set; }
        public string kkOrderNo { get; set; }
        public string ticketNumber { get; set; }
        public List<string> QrUrl { get; set; }
        public string result { get; set; }

    }


}
