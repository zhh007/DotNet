using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityComm
{
    public static class UnityExtension
    {
        public static IEnumerable<Type> InterfaceFrom<T>(this IEnumerable<Type> types)
        {
            return types.Where(t => typeof(T).IsAssignableFrom(t) && typeof(T) != t);
        }

        public static IEnumerable<Type> ClassFrom<T>(this IEnumerable<Type> types)
        {
            return types.Where(t => t.IsSubclassOf(typeof(T)));
        }
    }
}
