using System.Linq;
using System.Web.Mvc;
using Demo.Infrastructure;
using Microsoft.Practices.Unity.Mvc;
using Microsoft.Practices.Unity;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Demo.MvcApp.App_Start.UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Demo.MvcApp.App_Start.UnityWebActivator), "Shutdown")]

namespace Demo.MvcApp.App_Start
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            //var container = UnityConfig.GetConfiguredContainer();
            var container = ServiceLocator.Instance.GetConfiguredContainer();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));

            container.RegisterTypes(AllClasses.FromLoadedAssemblies(), WithMappings.FromMatchingInterface, WithName.Default, overwriteExistingMappings: true);

            container.RegisterType(typeof(Data.Models.DemoContext), typeof(Data.Models.DemoContext), "", new PerRequestLifetimeManager());

        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = ServiceLocator.Instance.GetConfiguredContainer();
            container.Dispose();
        }
    }
}