using System;
using Newtonsoft.Json;
using Oazachaosu.Core.Common;

namespace WordkiModel
{
    public class LanguageJsonCoverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ILanguage);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return LanguageFactory.GetLanguage((LanguageType)((Int64)reader.Value));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((int)((ILanguage)value).Type);
        }
    }
}