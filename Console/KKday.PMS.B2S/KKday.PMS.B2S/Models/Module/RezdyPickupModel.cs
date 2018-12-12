using System;
using System.Collections.Generic;

namespace KKday.PMS.B2S.Models.Module
{
    public class RezdyPickupModel
    {
        public RequestStatus requestStatus { get; set; }
        public List<PickupLocation> pickupLocations { get; set; }
    }

    public class RequestStatus
    {
        public bool success { get; set; }
        public string version { get; set; }
    }

    public class PickupLocation
    {
        public string locationName { get; set; }
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int minutesPrior { get; set; }
        public string additionalInstructions { get; set; }
    }
}
