using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pluggable
{
    /// <summary>
    /// 插件实现特性
    /// </summary>
    public class PluginAttribute : Attribute
    {
        public PluginAttribute()
            : base()
        {
            this.Version = "0.0.0.0";
            this.Author = "";
            this.Description = "";
        }

        public string Version { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
    }
}
