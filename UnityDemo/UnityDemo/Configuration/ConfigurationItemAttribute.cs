using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDemo.Configuration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConfigurationItemAttribute : Attribute
    {
        public string Id { get; set; }

        public ConfigurationItemAttribute(string id)
        {
            Id = id;
        }
    }
}
