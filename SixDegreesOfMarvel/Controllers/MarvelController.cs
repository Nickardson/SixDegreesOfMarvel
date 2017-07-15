using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MarvelApi.Tasks.Interfaces;
using MarvelApi.Tasks.Models;
using MarvelApi.Tasks.Models.ParsePage;

namespace SixDegreesOfMarvel.Controllers
{
    [RoutePrefix("api/marvel")]
    public class MarvelController : ApiController
    {
        private IMarvelApiTasks MarvelApi { get; }

        public MarvelController(IMarvelApiTasks marvelApi)
        {
            MarvelApi = marvelApi;
        }
        
        [Route("page")]
        public async Task<IHttpActionResult> GetPage([FromUri] string pageName)
        {
            var response = await MarvelApi.GetPageContents(pageName);
            return Json(response);
        }

        [Route("category")]
        public async Task<IHttpActionResult> GetCategory([FromUri] string categoryName)
        {
            var response = await MarvelApi.GetCategoryMembers($"Category:{categoryName}");
            return Json(response);
        }
    }
}
