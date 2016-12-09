using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnityDemo.Config
{
    public class ConfigManagerInterceptionBehavior : IInterceptionBehavior
    {
        public bool WillExecute
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return new Type[0];
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (IsGetPropertyMethod(input.MethodBase))
            {
                var attr = GetConfigProperty(input.MethodBase);
                if (attr != null)
                {
                    var ret = GetConfigValueById(input, attr.Id);
                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }

            return getNext()(input, getNext);
        }

        private static bool IsSetPropertyMethod(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("set_");
        }

        private static bool IsGetPropertyMethod(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("get_");
        }

        private ConfigPropertyAttribute GetConfigProperty(MethodBase method)
        {
            ConfigPropertyAttribute attr = null;

            var propertyName = method.Name.Substring(4);
            var target = method.ReflectedType;
            var property = target.GetProperty(propertyName);
            attr = property.GetCustomAttribute<ConfigPropertyAttribute>(false);

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
