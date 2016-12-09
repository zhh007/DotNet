using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDemo.Config
{
    /// <summary>
    /// 配置拦截属性
    /// </summary>
    public class ConfigManagerHandlerAttribute : HandlerAttribute
    {
        readonly ICallHandler handler;

        public ConfigManagerHandlerAttribute()
        {
            handler = new ConfigManagerCallHandler();
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return handler;
        }
    }
}
