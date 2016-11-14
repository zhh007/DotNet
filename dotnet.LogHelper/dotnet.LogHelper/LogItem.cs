using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet.LogHelper
{
    public class LogItem
    {
        public string Message { get; set; }
        public string Detail { get; set; }
        public LogItem(string msg, string detail)
        {
            this.Message = msg;
            this.Detail = detail;
        }
    }
}
