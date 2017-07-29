using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VTracker.Extensions
{
    public class SessionExpiresAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            HttpContext ctx = HttpContext.Current;
            if (ctx.Session["userId"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/LogOff");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}