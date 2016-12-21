using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Demo.Filters
{
    public class TimeLogFilter : ActionFilterAttribute
    {
        private const string Key = "__action_duration__";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var stopWatch = new Stopwatch();
            filterContext.HttpContext.Items[Key] = stopWatch;
            stopWatch.Start();
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var stopWatch = filterContext.HttpContext.Items[Key] as Stopwatch;
            if (stopWatch != null)
            {
                stopWatch.Stop();
                var actionName = filterContext.ActionDescriptor.ActionName;
                var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                Debug.WriteLine(string.Format("@@@ {0}- {1} : {2}", controllerName, actionName, stopWatch.Elapsed));
            }
        }
    }
}