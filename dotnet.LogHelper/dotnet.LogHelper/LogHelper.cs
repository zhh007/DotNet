using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.LogHelper
{
    public class LogHelper
    {
        public static void Log(string message, string detail = "")
        {
            _LogHelper.Instance.Log(message, detail);
        }
    }
}
