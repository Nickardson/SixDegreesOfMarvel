using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MarvelApi.Tasks;
using MarvelApi.Tasks.Models;
using MarvelApi.Tasks.Models.ParsePage;
using SixDegreesOfMarvel.Tasks.Tasks;

namespace SixDegreesOfMarvel.Controllers
{
    [RoutePrefix("api/marvel")]
    public class MarvelController : ApiController
    {
        private MarvelApiTasks MarvelApi { get; }

        public MarvelController(MarvelApiTasks marvelApi)
        {
            MarvelApi = marvelApi;
        }

        [HttpGet]
        [Route("character/groups")]
        public async Task<IHttpActionResult> GetCharacterGroups([FromUri] string pageName)
        {
            return Json(await MarvelApi.GetGroupAffiliations(pageName));
        }

        [HttpGet]
        [Route("group/characters")]
        public async Task<IHttpActionResult> GetGroupCharacters([FromUri] string pageName)
        {
            return Json(await MarvelApi.GetCharacterAffiliations(pageName));
        }

        [HttpGet]
        [Route("stress")]
        public async Task<IHttpActionResult> Stress()
        {
            var list = new MarvelDependencyTasks().DoThing();
            list.Reverse();
            return Json(list);
        }

        //[Route("page")]
        //public async Task<IHttpActionResult> GetPage([FromUri] string pageName)
        //{
        //    var response = await MarvelApi.GetPageContents(pageName);
        //    return Json(response);
        //}

        //[Route("category")]
        //public async Task<IHttpActionResult> GetCategory([FromUri] string categoryName)
        //{
        //    var response = await MarvelApi.GetCategoryMembers($"Category:{categoryName}");
        //    return Json(response);
        //}
    }
}
