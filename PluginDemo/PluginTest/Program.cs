using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ClassLibrary.Interface;
using System.IO;
using System.Diagnostics;
using PluginManage;

namespace AssemblyLoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = PluginManager.Instance.GetAllPluginDescriptors();
            foreach (var item in list)
            {
                Console.WriteLine(item.Type.FullName);
            }

            var op = PluginManager.Instance.GetPlugin<IOperator>();
            op.Operate();

            Console.ReadKey();

            op = PluginManager.Instance.GetPlugin<IOperator>();
            op.Operate();

            Console.ReadKey();

        }
    }
}
