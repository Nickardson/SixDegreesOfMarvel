using Newtonsoft.Json;

namespace MarvelApi.Tasks.Models.CategoryMembers
{
    public class QueryCategoryMembersContinueItemModel
    {
        [JsonProperty("CMContinue")]
        public string ContinueTitle { get; set; }
    }
}