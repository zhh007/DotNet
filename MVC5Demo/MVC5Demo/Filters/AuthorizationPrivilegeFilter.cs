using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC5Demo.Filters
{
    public class AuthorizationPrivilegeFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary{
                        { "controller", "Account" },{ "action", "Login" }});
            }

            base.OnActionExecuting(filterContext);
        }
    }
}