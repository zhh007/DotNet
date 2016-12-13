using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnityDemoTransparentProxy.Configuration
{
    /// <summary>
    /// 配置拦截器
    /// </summary>
    public class ConfigurationCallHandler : ICallHandler
    {
        public int Order
        {
            get
            {
                return 0;
            }
            set { }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (IsGetPropertyMethod(input.MethodBase))
            {
                var attr = GetConfigurationItemAttribute(input.MethodBase);
                if (attr != null)
                {
                    var ret = GetConfigValueById(input, attr.Id);
                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }

            IMethodReturn result = getNext.Invoke().Invoke(input, getNext);
            return result;
        }

        private static bool IsSetPropertyMethod(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("set_");
        }

        private static bool IsGetPropertyMethod(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("get_");
        }

        private ConfigurationItemAttribute GetConfigurationItemAttribute(MethodBase method)
        {
            ConfigurationItemAttribute attr = null;

            var propertyName = method.Name.Substring(4);
            var target = method.ReflectedType;
            var property = target.GetProperty(propertyName);
            attr = property.GetCustomAttribute<ConfigurationItemAttribute>(false);

            return attr;
        }

        private IMethodReturn GetConfigValueById(IMethodInvocation input, string configId)
        {
            Console.WriteLine(configId);
            if (input == null || input.MethodBase == null)
            {
                return null;
            }

            var methodInfo = input.MethodBase as MethodInfo;
            if (methodInfo == null)
            {
                return null;
            }

            if (methodInfo.ReturnType == typeof(string))
            {
                return input.CreateMethodReturn("abc", input.Arguments);
            }
            else if (methodInfo.ReturnType == typeof(int))
            {
                return input.CreateMethodReturn(123, input.Arguments);
            }

            return null;
        }
    }
}
