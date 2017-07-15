using System.Threading.Tasks;
using MarvelApi.Tasks.Models;
using MarvelApi.Tasks.Models.CategoryMembers;
using MarvelApi.Tasks.Models.ParsePage;

namespace MarvelApi.Tasks.Interfaces
{
    public interface IMarvelApiTasks
    {
        Task<ParsePageModel> GetPageContents(string pageName);

        Task<QueryCategoryMembersModel> GetCategoryMembers(string categoryName, string continuePage = null, int limit = 100);
    }
}