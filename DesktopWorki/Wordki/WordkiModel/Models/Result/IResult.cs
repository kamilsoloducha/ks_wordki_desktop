using System;
using Newtonsoft.Json;
using WordkiModel.Enums;

namespace WordkiModel
{
    public interface IResult
    {
        [JsonProperty(PropertyName = "Id")]
        [PropertyIndex(0)]
        long Id { get; set; }

        [JsonProperty(PropertyName = "GroupId")]
        [PropertyIndex(2)]
        IGroup Group { get; set; }

        [JsonProperty(PropertyName = "Correct")]
        [PropertyIndex(3)]
        short Correct { get; set; }

        [JsonProperty(PropertyName = "Accepted")]
        [PropertyIndex(4)]
        short Accepted { get; set; }

        [JsonProperty(PropertyName = "Wrong")]
        [PropertyIndex(5)]
        short Wrong { get; set; }

        [JsonProperty(PropertyName = "Invisibilities")]
        [PropertyIndex(6)]
        short Invisibilities { get; set; }

        [JsonProperty(PropertyName = "TimeCount")]
        [PropertyIndex(7)]
        short TimeCount { get; set; }

        [JsonProperty(PropertyName = "TranslationDirection")]
        [PropertyIndex(8)]
        TranslationDirection TranslationDirection { get; set; }

        [JsonProperty(PropertyName = "LessonType")]
        [PropertyIndex(9)]
        LessonType LessonType { get; set; }

        [JsonProperty(PropertyName = "DateTime")]
        [PropertyIndex(10)]
        DateTime DateTime { get; set; }

        [JsonProperty(PropertyName = "State")]
        [PropertyIndex(11)]
        int State { get; set; }
    }
}