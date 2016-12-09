using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnityDemo
{
    public class NotifyPropertyChangedCallHandler : ICallHandler
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
            if (IsGetProperty(input.MethodBase))
            {
                var methodInfo = input.MethodBase as MethodInfo;
                if (methodInfo != null)
                {
                    if (methodInfo.ReturnType == typeof(string))
                    {
                        return input.CreateMethodReturn("abc", input.Arguments);
                    }
                    else if(methodInfo.ReturnType == typeof(int))
                    {
                        return input.CreateMethodReturn(123, input.Arguments);
                    }
                }
            }

            IMethodReturn result = getNext.Invoke().Invoke(input, getNext);
            return result;
        }

        private static bool IsSetProperty(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("set_");
        }

        private static bool IsGetProperty(MethodBase method)
        {
            return method.IsSpecialName && method.Name.StartsWith("get_");
        }
    }
}
