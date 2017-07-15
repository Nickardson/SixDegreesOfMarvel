using MarvelApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MarvelApi.Tasks.Interfaces;
using Newtonsoft.Json;

namespace MarvelApi.Tasks
{
    public class MarvelApiTasks : IMarvelApiTasks
    {
        private HttpClient _client;

        public MarvelApiTasks()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://marvel.com/universe3zx/")
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            
        }

        public async Task<ParsedPageModel> GetPageContents(string pageName)
        {
            var response = await _client.GetAsync($"api.php?action=parse&format=json&page={pageName}");

            var parsedPageModel = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ParsedPageModel>(parsedPageModel);
        }
    }
}
