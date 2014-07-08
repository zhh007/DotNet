using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pluggable
{
    /// <summary>
    /// 插件接口特性
    /// </summary>
    public class PluggableAttribute : Attribute
    {
        public PluggableAttribute()
            : base()
        {
            this.Mode = PluginImplementMode.Single;
            this.Name = "";
            this.Description = "";
        }

        /// <summary>
        /// 插件实现方式
        /// </summary>
        public PluginImplementMode Mode { get; set; }

        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; set; }
    }
}
