﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using KKday.PMS.B2S.Models.Package. SCMPackageEventModel;
//
//    var scmPackageEventModel = ScmPackageEventModel.FromJson(jsonString);

namespace KKday.PMS.B2S.Models.Package.SCMPackageEventModel
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using KKday.PMS.B2S.Models.Shared;

    public partial class ScmPackageEventModel : ScmBaseModel
    {

        [JsonProperty("json")]
        public Json Json { get; set; }
    }

    public partial class Json
    {
        [JsonProperty("prodOid")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ProdOid { get; set; }

        [JsonProperty("packageOid")]
        public long PackageOid { get; set; }

        [JsonProperty("beginDate")]
        public string BeginDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("weekDay")]
        public string WeekDay { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("pkgOid")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long PkgOid { get; set; }

        [JsonProperty("supplierOid")]
        public long SupplierOid { get; set; }

        [JsonProperty("supplierUserUuid")]
        public Guid SupplierUserUuid { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("tokenKey")]
        public string TokenKey { get; set; }
    }

    public partial class ScmPackageEventModel
    {
        public static ScmPackageEventModel FromJson(string json) => JsonConvert.DeserializeObject<ScmPackageEventModel>(json, KKday.PMS.B2S.Models.Package.SCMPackageEventModel.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ScmPackageEventModel self) => JsonConvert.SerializeObject(self, KKday.PMS.B2S.Models.Package.SCMPackageEventModel.Converter.Settings);
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

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}

// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using KKday.PMS.B2S.Models.Package. SCMPackageEventStatusModel;
//
//    var scmPackageEventStatusModel = ScmPackageEventStatusModel.FromJson(jsonString);

namespace KKday.PMS.B2S.Models.Package.SCMPackageEventStatusModel
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using KKday.PMS.B2S.Models.Shared;

    public partial class ScmPackageEventStatusModel : ScmBaseModel
    {
        [JsonProperty("json")]
        public Json Json { get; set; }
    }

    public partial class Json
    {
        [JsonProperty("prodOid")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ProdOid { get; set; }

        [JsonProperty("packageOid")]
        public long PackageOid { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("pkgOid")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long PkgOid { get; set; }

        [JsonProperty("supplierOid")]
        public long SupplierOid { get; set; }

        [JsonProperty("supplierUserUuid")]
        public Guid SupplierUserUuid { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("tokenKey")]
        public string TokenKey { get; set; }
    }

    public partial class ScmPackageEventStatusModel
    {
        public static ScmPackageEventStatusModel FromJson(string json) => JsonConvert.DeserializeObject<ScmPackageEventStatusModel>(json, KKday.PMS.B2S.Models.Package.SCMPackageEventStatusModel.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ScmPackageEventStatusModel self) => JsonConvert.SerializeObject(self, KKday.PMS.B2S.Models.Package.SCMPackageEventStatusModel.Converter.Settings);
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

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
