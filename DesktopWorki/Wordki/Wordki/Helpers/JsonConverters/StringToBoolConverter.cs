using System;
using Newtonsoft.Json;

namespace Wordki.Helpers.JsonConverters {
  public class StringToBoolConverter : JsonConverter {

    public override bool CanConvert(Type objectType) {
      return objectType == typeof(string);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      return "1".Equals(reader.Value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      writer.WriteValue((bool)value ? "1" : "0");
    }
  }
}
