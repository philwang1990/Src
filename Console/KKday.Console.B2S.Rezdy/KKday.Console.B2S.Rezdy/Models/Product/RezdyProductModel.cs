using System;
using System.Collections.Generic;

namespace KKday.Consoles.B2S.Rezdy.Models.Produvt
{
    public class RezdyProductModel
    {
        public requestStatus RequestStatus { get; set; }
        public List<products> products { get; set; }
    }

    public class requestStatus
    {
        public bool success { get; set; }
        public string version { get; set; }
    }

    public class products
    {
        public string productType { get; set; }

        public string name { get; set; }
    }
}
