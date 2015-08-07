using System;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Demo.MvcApp.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            //container.LoadConfiguration();

            // TODO: Register your types here
            //container.RegisterType<Data.Models.DemoContext>(new PerRequestLifetimeManager());
            //container.RegisterType<Data.Repositories.IUserInfoRepository, Data.Repositories.UserInfoRepository>();
            //container.RegisterType<ServiceInterface.IUserInfoService, Service.UserInfoService>();

            //load config from unity.config
            string filename = System.IO.Path.Combine(System.Threading.Thread.GetDomain().BaseDirectory, "unity.config");
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filename };
            System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection("unity");
            container.LoadConfiguration(section);

        }
    }
}