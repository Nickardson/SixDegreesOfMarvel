using Newtonsoft.Json;

namespace MarvelApi.Tasks.Models.ParsePage
{
    public class ParseTextModel
    {
        [JsonProperty("*")]
        public string Content { get; set; }
    }
}