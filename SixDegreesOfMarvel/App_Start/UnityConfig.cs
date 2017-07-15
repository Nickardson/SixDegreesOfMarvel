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
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IMarvelApiTasks, MarvelApiTasks>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}