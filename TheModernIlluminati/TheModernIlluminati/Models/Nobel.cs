using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TheModernIlluminati.Models
{
    public partial class Nobel
    {
        [JsonProperty("laureates")]
        public List<Laureate> Laureates { get; set; }
        public Laureate[] LaureateArray{ get; set; }
    }

    public partial class Laureate
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Id { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("surname", NullValueHandling = NullValueHandling.Ignore)]
        public string Surname { get; set; }

        [JsonProperty("born", NullValueHandling = NullValueHandling.Ignore)]
        public string Born { get; set; }

        [JsonProperty("died")]
        public DiedUnion Died { get; set; }

        [JsonProperty("bornCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string BornCountry { get; set; }

        [JsonProperty("bornCountryCode", NullValueHandling = NullValueHandling.Ignore)]
        public string BornCountryCode { get; set; }

        [JsonProperty("bornCity", NullValueHandling = NullValueHandling.Ignore)]
        public string BornCity { get; set; }

        [JsonProperty("diedCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string DiedCountry { get; set; }

        [JsonProperty("diedCountryCode", NullValueHandling = NullValueHandling.Ignore)]
        public string DiedCountryCode { get; set; }

        [JsonProperty("diedCity", NullValueHandling = NullValueHandling.Ignore)]
        public string DiedCity { get; set; }

        [JsonProperty("gender")]
        public Gender Gender { get; set; }

        [JsonProperty("prizes")]
        public Prize[] Prizes { get; set; }
    }

    public partial class Prize
    {
        [JsonProperty("year")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Year { get; set; }

        [JsonProperty("category")]
        public Category Category { get; set; }

        [JsonProperty("share")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Share { get; set; }

        [JsonProperty("motivation")]
        public string Motivation { get; set; }

        [JsonProperty("affiliations")]
        public AffiliationElement[] Affiliations { get; set; }

        [JsonProperty("overallMotivation", NullValueHandling = NullValueHandling.Ignore)]
        public string OverallMotivation { get; set; }
    }

    public partial class AffiliationClass
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
    }

    public enum DiedEnum { The00000000 };

    public enum Gender { Female, Male, Org };

    public enum Category { Chemistry, Economics, Literature, Medicine, Peace, Physics };

    public partial struct DiedUnion
    {
        public DateTimeOffset? DateTime;
        public DiedEnum? Enum;

        public static implicit operator DiedUnion(DateTimeOffset DateTime) => new DiedUnion { DateTime = DateTime };
        public static implicit operator DiedUnion(DiedEnum Enum) => new DiedUnion { Enum = Enum };
    }

    public partial struct AffiliationElement
    {
        public AffiliationClass AffiliationClass;
        public object[] AnythingArray;

        public static implicit operator AffiliationElement(AffiliationClass AffiliationClass) => new AffiliationElement { AffiliationClass = AffiliationClass };
        public static implicit operator AffiliationElement(object[] AnythingArray) => new AffiliationElement { AnythingArray = AnythingArray };
    }

    public partial class Nobel
    {
        public static Nobel FromJson(string json) => JsonConvert.DeserializeObject<Nobel>(json, TheModernIlluminati.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Nobel self) => JsonConvert.SerializeObject(self, TheModernIlluminati.Models.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                DiedUnionConverter.Singleton,
                DiedEnumConverter.Singleton,
                GenderConverter.Singleton,
                AffiliationElementConverter.Singleton,
                CategoryConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class DiedUnionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DiedUnion) || t == typeof(DiedUnion?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    DateTimeOffset dt;
                    if (DateTimeOffset.TryParse(stringValue, out dt))
                    {
                        return new DiedUnion { DateTime = dt };
                    }
                    if (stringValue == "0000-00-00")
                    {
                        return new DiedUnion { Enum = DiedEnum.The00000000 };
                    }
                    break;
            }
            throw new Exception("Cannot unmarshal type DiedUnion");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (DiedUnion)untypedValue;
            if (value.DateTime != null)
            {
                serializer.Serialize(writer, value.DateTime.Value.ToString("o", System.Globalization.CultureInfo.InvariantCulture));
                return;
            }
            if (value.Enum != null)
            {
                if (value.Enum == DiedEnum.The00000000)
                {
                    serializer.Serialize(writer, "0000-00-00");
                    return;
                }
            }
            throw new Exception("Cannot marshal type DiedUnion");
        }

        public static readonly DiedUnionConverter Singleton = new DiedUnionConverter();
    }

    internal class DiedEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DiedEnum) || t == typeof(DiedEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "0000-00-00")
            {
                return DiedEnum.The00000000;
            }
            throw new Exception("Cannot unmarshal type DiedEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (DiedEnum)untypedValue;
            if (value == DiedEnum.The00000000)
            {
                serializer.Serialize(writer, "0000-00-00");
                return;
            }
            throw new Exception("Cannot marshal type DiedEnum");
        }

        public static readonly DiedEnumConverter Singleton = new DiedEnumConverter();
    }

    internal class GenderConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Gender) || t == typeof(Gender?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "female":
                    return Gender.Female;
                case "male":
                    return Gender.Male;
                case "org":
                    return Gender.Org;
            }
            throw new Exception("Cannot unmarshal type Gender");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Gender)untypedValue;
            switch (value)
            {
                case Gender.Female:
                    serializer.Serialize(writer, "female");
                    return;
                case Gender.Male:
                    serializer.Serialize(writer, "male");
                    return;
                case Gender.Org:
                    serializer.Serialize(writer, "org");
                    return;
            }
            throw new Exception("Cannot marshal type Gender");
        }

        public static readonly GenderConverter Singleton = new GenderConverter();
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

    internal class AffiliationElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AffiliationElement) || t == typeof(AffiliationElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<AffiliationClass>(reader);
                    return new AffiliationElement { AffiliationClass = objectValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<object[]>(reader);
                    return new AffiliationElement { AnythingArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type AffiliationElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (AffiliationElement)untypedValue;
            if (value.AnythingArray != null)
            {
                serializer.Serialize(writer, value.AnythingArray);
                return;
            }
            if (value.AffiliationClass != null)
            {
                serializer.Serialize(writer, value.AffiliationClass);
                return;
            }
            throw new Exception("Cannot marshal type AffiliationElement");
        }

        public static readonly AffiliationElementConverter Singleton = new AffiliationElementConverter();
    }

    internal class CategoryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Category) || t == typeof(Category?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "chemistry":
                    return Category.Chemistry;
                case "economics":
                    return Category.Economics;
                case "literature":
                    return Category.Literature;
                case "medicine":
                    return Category.Medicine;
                case "peace":
                    return Category.Peace;
                case "physics":
                    return Category.Physics;
            }
            throw new Exception("Cannot unmarshal type Category");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Category)untypedValue;
            switch (value)
            {
                case Category.Chemistry:
                    serializer.Serialize(writer, "chemistry");
                    return;
                case Category.Economics:
                    serializer.Serialize(writer, "economics");
                    return;
                case Category.Literature:
                    serializer.Serialize(writer, "literature");
                    return;
                case Category.Medicine:
                    serializer.Serialize(writer, "medicine");
                    return;
                case Category.Peace:
                    serializer.Serialize(writer, "peace");
                    return;
                case Category.Physics:
                    serializer.Serialize(writer, "physics");
                    return;
            }
            throw new Exception("Cannot marshal type Category");
        }

        public static readonly CategoryConverter Singleton = new CategoryConverter();
    }
}
