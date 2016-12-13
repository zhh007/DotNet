﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Reflection;
using UnityDemo.Configuration;

/// <summary>
/// 使用Unity VirtualMethodInterceptor实现配置管理
/// </summary>
namespace UnityDemo
{
    class Program
    {
        public static IUnityContainer UnityContainer = new UnityContainer();
        static void Main(string[] args)
        {
            UnityContainer.AddNewExtension<Interception>();

            Assembly thisAss = Assembly.GetExecutingAssembly();
            foreach (Module mod in thisAss.GetLoadedModules(false))
            {
                foreach (Type t in mod.GetTypes())
                {
                    if(typeof(IConfiguration) == t)
                    {
                        continue;
                    }

                    if(typeof(IConfiguration).IsAssignableFrom(t))
                    {
                        UnityContainer.RegisterType(t, t
                            , new ContainerControlledLifetimeManager()
                            , new Interceptor<VirtualMethodInterceptor>()
                            , new InterceptionBehavior<ConfigurationInterceptionBehavior>());
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
/*
 * Policy Injection Matching Rules
https://msdn.microsoft.com/en-us/library/dn507448(v=pandp.30).aspx
*/
