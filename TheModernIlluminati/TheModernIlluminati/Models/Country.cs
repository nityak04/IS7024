
namespace TheModernIlluminati.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Country
    {
        [JsonProperty("countries")]
        public List<Count> Countries { get; set; }
    }

    public partial class Count
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
    }

    public partial class Country
    {
        public static Country FromJson(string json) => JsonConvert.DeserializeObject<Country>(json, TheModernIlluminati.Models.Converters.Settings);
    }

    public static class Serializes
    {
        public static string ToJson(this Country self) => JsonConvert.SerializeObject(self, TheModernIlluminati.Models.Converters.Settings);
    }

    internal static class Converters
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