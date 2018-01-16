using Newtonsoft.Json;
using System;

namespace WordkiModelCore.DTO
{
    public class WordDTO
    {
        [JsonProperty("Id")]
        public long Id { get; set; }
        [JsonProperty("Gid")]
        public long GroupId { get; set; }
        [JsonProperty("L1")]
        public string Language1 { get; set; }
        [JsonProperty("L2")]
        public string Language2 { get; set; }
        [JsonProperty("L1C")]
        public string Language1Comment { get; set; }
        [JsonProperty("L2C")]
        public string Language2Comment { get; set; }
        [JsonProperty("D")]
        public byte Drawer { get; set; }
        [JsonProperty("IV")]
        public bool IsVisible { get; set; }
        [JsonProperty("S")]
        public int State { get; set; }
        [JsonProperty("IS")]
        public bool IsSelected { get; set; }
        [JsonProperty("RC")]
        public ushort RepeatingCounter { get; set; }
        [JsonProperty("LR")]
        public DateTime LastRepeating { get; set; }
        [JsonProperty("C")]
        public string Comment { get; set; }
    }
}
