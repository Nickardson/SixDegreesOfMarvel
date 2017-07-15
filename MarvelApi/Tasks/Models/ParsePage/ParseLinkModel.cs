using Newtonsoft.Json;

namespace MarvelApi.Tasks.Models.ParsePage
{
    public class ParseLinkModel
    {
        [JsonProperty("*")]
        public string Name { get; set; }

        //public int NS { get; set; }
    }
}