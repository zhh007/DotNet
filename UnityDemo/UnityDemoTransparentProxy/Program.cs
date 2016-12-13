using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityDemoTransparentProxy.Config;

/// <summary>
/// 使用Unity TransparentProxyInterceptor实现配置管理
/// </summary>
namespace UnityDemoTransparentProxy
{
    class Program
    {
        public static IUnityContainer UnityContainer = new UnityContainer();
        static void Main(string[] args)
        {
            UnityContainer.AddNewExtension<Interception>();
            var interceptionConfig = UnityContainer.Configure<Interception>();

            Assembly thisAss = Assembly.GetExecutingAssembly();
            foreach (Module mod in thisAss.GetLoadedModules(false))
            {
                foreach (Type t in mod.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(ConfigBase)))
                    {
                        interceptionConfig.SetInterceptorFor<SomeEntityClass>(new TransparentProxyInterceptor());

                        //config error
                        //UnityContainer.RegisterType(t, t,
                        //    new ContainerControlledLifetimeManager(),
                        //    new Interceptor<TransparentProxyInterceptor>());
                    }
                }
            }

            SomeEntityClass obj = UnityContainer.Resolve<SomeEntityClass>();
            Console.WriteLine(obj.StringProperty);
            Console.WriteLine(obj.IntProperty);

            Console.ReadKey();
        }
    }
}

/**
 * UnityContainer.AddNewExtension<Interception>()
            .Configure<Interception>()
            .SetInterceptorFor<SomeEntityClass>(new TransparentProxyInterceptor());
 */
