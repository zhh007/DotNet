using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Reflection;
using System.IO;

namespace Demo.Infrastructure
{
    public sealed class ServiceLocator : IServiceProvider
    {
        #region Private Fields
        private readonly IUnityContainer container;
        #endregion

        #region Private Static Fields
        private static ServiceLocator instance = new ServiceLocator();
        #endregion

        #region Ctors
        /// <summary>
        /// Initializes a new instance of <c>ServiceLocator</c> class.
        /// </summary>
        private ServiceLocator()
        {
            string filename = string.Empty;
            try
            {
                container = new UnityContainer();

                //unity.config
                filename = System.IO.Path.Combine(System.Threading.Thread.GetDomain().BaseDirectory, "unity.config");
                if (File.Exists(filename))
                {
                    var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filename };
                    System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection("unity");
                    container.LoadConfiguration(section);
                }

                //web.config or app.config
                UnityConfigurationSection section2 = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                if (section2 != null)
                {
                    section2.Configure(container);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                //LogHelper.Error(string.Format("Unity文件({0})处理出错", filename), ex);
            }
        }
        #endregion

        #region Public Static Properties
        /// <summary>
        /// Gets the singleton instance of the <c>ServiceLocator</c> class.
        /// </summary>
        public static ServiceLocator Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        #region Private Methods
        private IEnumerable<ParameterOverride> GetParameterOverrides(object overridedArguments)
        {
            List<ParameterOverride> overrides = new List<ParameterOverride>();
            Type argumentsType = overridedArguments.GetType();
            argumentsType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList()
                .ForEach(property =>
                {
                    var propertyValue = property.GetValue(overridedArguments, null);
                    var propertyName = property.Name;
                    overrides.Add(new ParameterOverride(propertyName, propertyValue));
                });
            return overrides;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the service instance with the given type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service instance.</returns>
        public T GetService<T>()
        {
            return container.Resolve<T>();
        }

        public T GetService<T>(string name)
        {
            return container.Resolve<T>(name);
        }
        /// <summary>
        /// Gets the service instance with the given type by using the overrided arguments.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="overridedArguments">The overrided arguments.</param>
        /// <returns>The service instance.</returns>
        public T GetService<T>(object overridedArguments)
        {
            var overrides = GetParameterOverrides(overridedArguments);
            return container.Resolve<T>(overrides.ToArray());
        }
        /// <summary>
        /// Gets the service instance with the given type by using the overrided arguments.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="overridedArguments">The overrided arguments.</param>
        /// <returns>The service instance.</returns>
        public object GetService(Type serviceType, object overridedArguments)
        {
            var overrides = GetParameterOverrides(overridedArguments);
            return container.Resolve(serviceType, overrides.ToArray());
        }

        public IUnityContainer GetConfiguredContainer()
        {
            return container;
        }
        #endregion

        #region IServiceProvider Members
        /// <summary>
        /// Gets the service instance with the given type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>The service instance.</returns>
        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        #endregion
    }
}
