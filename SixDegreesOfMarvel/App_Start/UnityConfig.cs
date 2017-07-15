using MarvelApi.Tasks;
using Microsoft.Practices.Unity;
using System.Web.Http;
using MarvelApi.Tasks.Interfaces;
using Unity.WebApi;

namespace SixDegreesOfMarvel
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<IMarvelApiTasks, MarvelApiTasks>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}