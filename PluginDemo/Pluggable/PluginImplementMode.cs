using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pluggable
{
    /// <summary>
    /// 插件实现方式
    /// </summary>
    public enum PluginImplementMode : int
    {
        /// <summary>
        /// 只能存在一个实现
        /// </summary>
        Single,
        /// <summary>
        /// 可以存在多个实现
        /// </summary>
        Multiple
    }
}
