﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using KKday.PMS.B2S.Models.Package;
//
//    var RezdyPackageModel = RezdyPackageModel.FromJson(jsonString);

namespace KKday.PMS.B2S.Models.Package
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using KKday.PMS.B2S.Models.Shared;

    public partial class RezdyPackageModel
    {
        [JsonProperty("requestStatus")]
        public RequestStatus RequestStatus { get; set; }

        [JsonProperty("sessions")]
        public List<Session> Sessions { get; set; }
    }

    public partial class RequestStatus
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public partial class Session
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("productCode")]
        public string ProductCode { get; set; }

        [JsonProperty("startTime")]
        public DateTimeOffset StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTimeOffset EndTime { get; set; }

        [JsonProperty("startTimeLocal")]
        public DateTimeOffset StartTimeLocal { get; set; }

        [JsonProperty("endTimeLocal")]
        public DateTimeOffset EndTimeLocal { get; set; }

        [JsonProperty("allDay")]
        public bool AllDay { get; set; }

        [JsonProperty("seats")]
        public long Seats { get; set; }

        [JsonProperty("seatsAvailable")]
        public long SeatsAvailable { get; set; }

        [JsonProperty("priceOptions")]
        public List<PriceOption> PriceOptions { get; set; }
    }

    public partial class PriceOption
    {
        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("seatsUsed")]
        public long SeatsUsed { get; set; }

        [JsonProperty("productCode")]
        public string ProductCode { get; set; }
    }

    public partial class RezdyPackageModel
    {
        public static RezdyPackageModel FromJson(string json) => JsonConvert.DeserializeObject<RezdyPackageModel>(json, KKday.PMS.B2S.Models.Package.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RezdyPackageModel self) => JsonConvert.SerializeObject(self, KKday.PMS.B2S.Models.Package.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
