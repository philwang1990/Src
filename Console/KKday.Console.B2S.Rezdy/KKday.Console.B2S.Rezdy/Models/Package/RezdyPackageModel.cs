using System;
using System.Collections.Generic;

namespace KKday.Consoles.B2S.Rezdy.Models.Package
{
    public class RezdyPackageModel
    {
        public requestStatus RequestStatus { get; set; }
        public List<sessions> Sessions { get; set; }
    }

    public class requestStatus
    {
        public bool success { get; set; }
        public string version { get; set; }
    }

    public class sessions
    {
        public long id { get; set; }

        public string productCode { get; set; }
    }
}
