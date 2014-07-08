using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary.Interface;
using Pluggable;

namespace ClassLibrary.Implements
{
    [Plugin]
    public class TestClass : IOperator
    {
        public void Operate()
        {
            Console.WriteLine("2.0.0.0");
        }
    }
}
