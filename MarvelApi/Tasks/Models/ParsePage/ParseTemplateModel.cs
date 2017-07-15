using Newtonsoft.Json;

namespace MarvelApi.Tasks.Models.ParsePage
{
    public class ParseTemplateModel
    {
        [JsonProperty("*")]
        public string Name { get; set; }

        /// <summary>
        /// Empty string if does exist?
        /// </summary>
        public string Exists { get; set; }
    }
}