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
        public async Task<bool> ClearCache()
        {
            await DependencyTasks.ClearCache();
            
            return true;
        }

        [HttpGet]
        [Route("connections")]
        public async Task<IEnumerable<ConnectionModel>> Connections([FromUri] string fromCharacter, [FromUri] string toCharacter)
        {
            var from = await DependencyTasks.GetCharacterByName(fromCharacter);
            var to = await DependencyTasks.GetCharacterByName(toCharacter);

            if (from == null)
            {
                throw new NullReferenceException($"Could not find the character '{fromCharacter}'.");
            }
            if (to == null)
            {
                throw new NullReferenceException($"Could not find the character '{toCharacter}'.");
            }

            var path = DependencyTasks.BreadthFirstSearch(from, to);

            return path.Select(x => new ConnectionModel(x.Item1, x.Item2));
        }

        /// <summary>
        /// Gets a list of all characters that have been discovered.
        /// </summary>
        [HttpGet]
        [Route("character")]
        public async Task<IEnumerable<CharacterModel>> GetAllCharacters()
        {
            var list = await DependencyTasks.GetAllCharacters();

            return list.Select(x => new CharacterModel(x));
        }

        /// <summary>
        /// Gets a list of all groups that have been discovered.
        /// </summary>
        [HttpGet]
        [Route("group")]
        public async Task<IEnumerable<GroupModel>> GetAllGroups()
        {
            var list = await DependencyTasks.GetAllGroups();

            return list.Select(x => new GroupModel(x));
        }

        /// <summary>
        /// Gets a character by name.
        /// </summary>
        /// <param name="characterName">The page name of the character.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("character")]
        public async Task<Character> GetCharacterByName([FromUri] string characterName)
        {
            var character = await DependencyTasks.GetCharacterByName(characterName);
            return character;
        }

        /// <summary>
        /// Gets all groups which the given character belongs to.
        /// </summary>
        /// <param name="characterName">The page name of the character.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("character/groups")]
        public async Task<CharacterGroupsResponse> GetCharacterGroups([FromUri] string characterName)
        {
            var groups = await DependencyTasks.GetGroupAffiliations(characterName);
            return new CharacterGroupsResponse(groups);
        }

        [HttpGet]
        [Route("group/characters")]
        public async Task<GroupCharactersResponse> GetGroupCharacters([FromUri] string groupName)
        {
            var characters = await DependencyTasks.GetCharacterAffiliations(groupName);
            return new GroupCharactersResponse(characters);
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
