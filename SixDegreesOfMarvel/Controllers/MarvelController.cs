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
using SixDegreesOfMarvel.Model.Models;
using SixDegreesOfMarvel.Models;
using SixDegreesOfMarvel.Tasks.DTO;
using SixDegreesOfMarvel.Tasks.Tasks;

namespace SixDegreesOfMarvel.Controllers
{
    [RoutePrefix("api/marvel")]
    public class MarvelController : ApiController
    {
        private MarvelApiTasks MarvelApi { get; }
        private MarvelDependencyTasks DependencyTasks { get; }

        public MarvelController(MarvelApiTasks marvelApi, MarvelDependencyTasks dependencyTasks)
        {
            MarvelApi = marvelApi;
            DependencyTasks = dependencyTasks;
        }

        /// <summary>
        /// Gets a list of all characters that have been discovered.
        /// </summary>
        [HttpGet]
        [Route("clearcache")]
        public async Task<JsonResult<bool>> ClearCache()
        {
            await DependencyTasks.ClearCache();

            return Json(true);
        }

        /// <summary>
        /// Gets a list of all characters that have been discovered.
        /// </summary>
        [HttpGet]
        [Route("character")]
        public async Task<JsonResult<IEnumerable<CharacterModel>>> GetAllCharacters()
        {
            var list = await DependencyTasks.GetAllCharacters();

            return Json(list.Select(x => new CharacterModel(x)));
        }

        /// <summary>
        /// Gets a list of all groups that have been discovered.
        /// </summary>
        [HttpGet]
        [Route("group")]
        public async Task<JsonResult<IEnumerable<GroupModel>>> GetAllGroups()
        {
            var list = await DependencyTasks.GetAllGroups();

            return Json(list.Select(x => new GroupModel(x)));
        }

        /// <summary>
        /// Gets a character by name.
        /// </summary>
        /// <param name="characterName">The page name of the character.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("character")]
        public async Task<JsonResult<Character>> GetCharacterByName([FromUri] string characterName)
        {
            var character = await DependencyTasks.GetCharacterByName(characterName);
            return Json(character);
        }

        /// <summary>
        /// Gets all groups which the given character belongs to.
        /// </summary>
        /// <param name="characterName">The page name of the character.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("character/groups")]
        public async Task<JsonResult<CharacterGroupsResponse>> GetCharacterGroups([FromUri] string characterName)
        {
            var groups = await DependencyTasks.GetGroupAffiliations(characterName);
            return Json(new CharacterGroupsResponse(groups));
        }

        [HttpGet]
        [Route("group/characters")]
        public async Task<JsonResult<GroupCharactersResponse>> GetGroupCharacters([FromUri] string groupName)
        {
            var characters = await DependencyTasks.GetCharacterAffiliations(groupName);
            return Json(new GroupCharactersResponse(characters));
        }
        
        //[HttpPost]
        //[Route("stress")]
        //public async Task<IHttpActionResult> StressPost([FromUri] string name, [FromBody] CharacterDTO dto)
        //{
        //    var character = await DependencyTasks.AddOrUpdateCharacter(name, dto);

        //    return Json(character);
        //}

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
