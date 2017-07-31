using MarvelApi.Tasks;
using Microsoft.Practices.Unity;
using System.Web.Http;
using SixDegreesOfMarvel.Tasks.Tasks;
using Unity.WebApi;

namespace SixDegreesOfMarvel
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<MarvelApiTasks, MarvelApiTasks>();
            container.RegisterType<MarvelDependencyTasks, MarvelDependencyTasks>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}