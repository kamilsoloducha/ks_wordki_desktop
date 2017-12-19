using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WordkiModel
{
    public interface IGroup
    {
        [JsonProperty(PropertyName = "Id")]
        [PropertyIndex(0)]
        long Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        [PropertyIndex(2)]
        string Name { get; set; }

        [JsonProperty(PropertyName = "Language1", DefaultValueHandling = DefaultValueHandling.Populate)]
        [JsonConverter(typeof(LanguageJsonCoverter))]
        [PropertyIndex(3)]
        LanguageType Language1 { get; set; }

        [JsonProperty(PropertyName = "Language2", DefaultValueHandling = DefaultValueHandling.Populate)]
        [JsonConverter(typeof(LanguageJsonCoverter))]
        [PropertyIndex(4)]
        LanguageType Language2 { get; set; }

        [JsonProperty(PropertyName = "State")]
        [PropertyIndex(5)]
        int State { get; set; }

        [JsonProperty(PropertyName = "CreationDate")]
        [PropertyIndex(6)]
        DateTime CreationDate { get; set; }

        [JsonIgnore]
        IList<IWord> Words { get; set; }

        [JsonIgnore]
        IList<IResult> Results { get; set; }

        void AddWord(IWord word);
        void AddResult(IResult result);
    }
}