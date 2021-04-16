﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var welcome = Welcome.FromJson(jsonString);

namespace QuickType
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Welcome
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("breweryType")]
        public BreweryType BreweryType { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("address2")]
        public object Address2 { get; set; }

        [JsonProperty("address3")]
        public object Address3 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("countyProvince")]
        public object CountyProvince { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("websiteUrl")]
        public Uri WebsiteUrl { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }
    }

    public enum BreweryType { Brewpub, Closed, Large, Micro };

    public enum Country { UnitedStates };

    public partial class Welcome
    {
        public static Welcome[] FromJson(string json) => JsonConvert.DeserializeObject<Welcome[]>(json, QuickType.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Welcome[] self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                BreweryTypeConverter.Singleton,
                CountryConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class BreweryTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(BreweryType) || t == typeof(BreweryType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "brewpub":
                    return BreweryType.Brewpub;
                case "closed":
                    return BreweryType.Closed;
                case "large":
                    return BreweryType.Large;
                case "micro":
                    return BreweryType.Micro;
            }
            throw new Exception("Cannot unmarshal type BreweryType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (BreweryType)untypedValue;
            switch (value)
            {
                case BreweryType.Brewpub:
                    serializer.Serialize(writer, "brewpub");
                    return;
                case BreweryType.Closed:
                    serializer.Serialize(writer, "closed");
                    return;
                case BreweryType.Large:
                    serializer.Serialize(writer, "large");
                    return;
                case BreweryType.Micro:
                    serializer.Serialize(writer, "micro");
                    return;
            }
            throw new Exception("Cannot marshal type BreweryType");
        }

        public static readonly BreweryTypeConverter Singleton = new BreweryTypeConverter();
    }

    internal class CountryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Country) || t == typeof(Country?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "United States")
            {
                return Country.UnitedStates;
            }
            throw new Exception("Cannot unmarshal type Country");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Country)untypedValue;
            if (value == Country.UnitedStates)
            {
                serializer.Serialize(writer, "United States");
                return;
            }
            throw new Exception("Cannot marshal type Country");
        }

        public static readonly CountryConverter Singleton = new CountryConverter();
    }
}
