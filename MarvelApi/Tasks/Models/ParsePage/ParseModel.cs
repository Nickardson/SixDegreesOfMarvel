using System.Collections.Generic;
using Newtonsoft.Json;

namespace MarvelApi.Tasks.Models.ParsePage
{
    public class ParseModel
    {
        public ParseTextModel Text { get; set; }

        public List<ParseCategoryModel> Categories { get; set; }

        public List<ParseLinkModel> Links { get; set; }

        public List<ParseTemplateModel> Templates { get; set; }

        public List<string> Images { get; set; }

        public List<string> ExternalLinks { get; set; }

        //public List<object> Sections { get; set; }

        public string DisplayTitle { get; set; }

        [JsonProperty("RevId")]
        public int RevisionId { get; set; }
    }
}