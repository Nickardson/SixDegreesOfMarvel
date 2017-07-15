using System.Threading.Tasks;
using MarvelApi.Models;

namespace MarvelApi.Tasks.Interfaces
{
    public interface IMarvelApiTasks
    {
        Task<ParsedPageModel> GetPageContents(string pageName);
    }
}