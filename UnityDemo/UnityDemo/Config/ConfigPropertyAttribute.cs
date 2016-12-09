using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDemo.Config
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConfigPropertyAttribute : Attribute
    {
        public string Id { get; set; }

        public ConfigPropertyAttribute(string id)
        {
            Id = id;
        }
    }
}
