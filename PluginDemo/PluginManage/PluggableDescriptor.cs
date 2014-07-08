using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pluggable;

namespace PluginManage
{
    public class PluggableDescriptor
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public PluginImplementMode Mode { get; set; }
        public string Description { get; set; }
    }
}
