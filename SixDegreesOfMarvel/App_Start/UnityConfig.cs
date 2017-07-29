using MarvelApi.Tasks;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace SixDegreesOfMarvel
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<MarvelApiTasks, MarvelApiTasks>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}