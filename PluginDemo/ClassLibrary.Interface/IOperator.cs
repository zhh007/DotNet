using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pluggable;

namespace ClassLibrary.Interface
{
    [Pluggable(Name = "操作接口", Description = "实现操作接口，可以...")]
    public interface IOperator
    {
        void Operate();
    }
}
