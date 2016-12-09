using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDemo
{
    public class NotifyPropertyChangedAttribute : HandlerAttribute
    {
        readonly ICallHandler handler;

        public NotifyPropertyChangedAttribute()
        {
            handler = new NotifyPropertyChangedCallHandler();
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return handler;
        }
    }
}
