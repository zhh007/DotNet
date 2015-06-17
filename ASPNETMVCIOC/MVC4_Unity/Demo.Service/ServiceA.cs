using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.Interface;

namespace Demo.Service
{
    public class ServiceA : IServiceA
    {
        #region IServiceA Members

        public string Say()
        {
            return "I am Service A.";
        }

        #endregion
    }
}