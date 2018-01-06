using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CRM.utility
{
    public class utilis
    {
    }
    public class AuthorizeVerifiedadminAttribute : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IAuthorizationFilter
    {
        void System.Web.Mvc.IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            var authorized = false;
            //_xD is basically the userID, just to hide name 
            if (filterContext.HttpContext.Session["AdminId"] != null)
            {
                authorized = true;
            }
            if (!authorized)
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }
        protected void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary
                                   {
                                       { "action", "Index" },
                                       { "controller", "login" },
                                      { "ReturnUrl", filterContext.HttpContext.Request.Url.ToString() }
                                   });
        }
    }
}