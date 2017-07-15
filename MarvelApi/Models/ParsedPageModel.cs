using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MarvelApi.Models
{
    // https://marvel.com/universe3zx/api.php?action=parse&format=json&page=Warlock_(Technarchy)
    public class ParsedPageModel
    {
        public ParseModel Parse { get; set; }
    }

    public class ParseModel
    {
        public ParseTextModel Text { get; set; }
    }

    public class ParseTextModel
    {
        [JsonProperty("*")]
        public string Content;
    }
}
