using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MarvelApi.Tasks.Models.CategoryMembers
{
    public class QueryCategoryMembersModel
    {
        public QueryCategoryMembersQueryModel Query { get; set; }

        [JsonProperty("Query-Continue")]
        public QueryCategoryMembersQueryContinueModel QueryContinue { get; set; }
    }
}
