using System;
using System.Collections.Generic;

namespace KKday.PMS.B2S.Models.Product
{
    public class RezdyProductListModel
    {
        public requestStatus RequestStatus { get; set; }
        public List<product> Products { get; set; }
    }

    public class requestStatus
    {
        public bool success { get; set; }
        public string version { get; set; }
    }

    public class product
    {
        public string productType { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public string description { get; set; }
        public string productCode { get; set; }
        public string internalCode { get; set; }
        public long supplierId { get; set; }
        public string supplierAlias { get; set; }
        public string supplierName { get; set; }
        public string timezone { get; set; }
        public double advertisedPrice { get; set; }
        public List<priceOptions> PriceOptions { get; set; }

        public string currency { get; set; }
        public string unitLabel { get; set; }
        public string unitLabelPlural { get; set; }
        public bool quantityRequired { get; set; }
        public long quantityRequiredMin { get; set; }
        public long nquantityRequiredMaxame { get; set; }
        public List<images> Images { get; set; }

        public List<String> videos { get; set; }
        public string bookingMode { get; set; }
        public bool charter { get; set; }
        public string terms { get; set; }
        public string generalTerms { get; set; }
        public List<bookingFields> BookingFields { get; set; }

        public double latitude { get; set; }
        public double longitude { get; set; }
        public string confirmMode { get; set; }
        public long confirmModeMinParticipants { get; set; }
        public string agentPaymentType { get; set; }
        public long maxCommissionPercent { get; set; }
        public bool commissionIncludesExtras { get; set; }
        public DateTime dateCreated { get; set; }
        public long minimumNoticeMinutes { get; set; }
        public double durationMinutes { get; set; }
        public DateTime dateUpdated { get; set; }
        public long pickupId { get; set; }
        public locationAddress LocationAddress { get; set; }

        public List<String> Languages { get; set; }
        public List<String> Tags { get; set; }

    }

    public class priceOptions
    {
        public long price { get; set; }
        public string label { get; set; }
        public long id { get; set; }
        public long seatsUsed { get; set; }
        public string productCode { get; set; }
    }

    public class images
    {
        public long id { get; set; }
        public string itemUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string mediumSizeUrl { get; set; }
        public string largeSizeUrl { get; set; }
    }

    public class bookingFields
    {
        public string label { get; set; }
        public bool requiredPerParticipant { get; set; }
        public bool requiredPerBooking { get; set; }
        public bool visiblePerParticipant { get; set; }
        public bool visiblePerBooking { get; set; }
        public string fieldType { get; set; }
    }

    public class locationAddress
    {
        public string addressLine { get; set; }
        public string city { get; set; }
        public string countryCode { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

   
}
