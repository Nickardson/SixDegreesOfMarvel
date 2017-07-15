using Newtonsoft.Json;

namespace MarvelApi.Tasks.Models.ParsePage
{
    public class ParseCategoryModel
    {
        [JsonProperty("*")]
        public string Name { get; set; }
        
        public string SortKey { get; set; }
    }
}