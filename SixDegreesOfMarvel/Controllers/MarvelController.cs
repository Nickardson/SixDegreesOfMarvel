using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MarvelApi.Models;
using MarvelApi.Tasks.Interfaces;

namespace SixDegreesOfMarvel.Controllers
{
    [RoutePrefix("api/marvel")]
    public class MarvelController : ApiController
    {
        private IMarvelApiTasks _marvelApi { get; }

        public MarvelController(IMarvelApiTasks marvelApi)
        {
            _marvelApi = marvelApi;
        }
        
        [Route("page/{pageName}")]
        public async Task<JsonResult<ParsedPageModel>> GetPage(string pageName)
        {
            var response = await _marvelApi.GetPageContents(pageName);
            return Json(response);
        }
    }
}
