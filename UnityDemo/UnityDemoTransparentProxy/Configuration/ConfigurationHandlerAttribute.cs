using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDemoTransparentProxy.Configuration
{
    /// <summary>
    /// 配置拦截属性
    /// </summary>
    public class ConfigurationHandlerAttribute : HandlerAttribute
    {
        readonly ICallHandler handler;

        public ConfigurationHandlerAttribute()
        {
            handler = new ConfigurationCallHandler();
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return handler;
        }
    }
}
