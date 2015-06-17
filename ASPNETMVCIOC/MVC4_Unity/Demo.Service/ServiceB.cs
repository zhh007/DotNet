using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Interface;

namespace Demo.Service
{
    public class ServiceB : IServiceB
    {
        #region IServiceB Members

        public string Write()
        {
            return "I am Service B.";
        }

        #endregion
    }
}