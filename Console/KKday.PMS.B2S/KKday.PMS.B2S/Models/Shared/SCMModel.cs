﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using KKday.PMS.B2S.Models.Shared;
//
//    var scmBaseModel = ScmModel.FromJson(jsonString);

namespace KKday.PMS.B2S.Models.Shared
{
    using Newtonsoft.Json;

    public partial class ScmBaseModel
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; } = "kkdayapi";

        [JsonProperty("userOid")] 
        public long UserOid { get; set; } = 3;

        [JsonProperty("ver")]
        public string Ver { get; set; } = "1.0.1";

        [JsonProperty("locale")]
        public string Locale { get; set; } = "en";

        [JsonProperty("currency")]
        public string Currency { get; set; } = "AUD";

        [JsonProperty("ipaddress")]
        public string Ipaddress { get; set; } = "168.129.2.2";
    }
}
