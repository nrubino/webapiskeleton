using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using log4net;

namespace webapiskeleton.Filters
{
    public class LoggingFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var man = LogManager.GetLogger("webapiskeleton.logger");
            man.Info(string.Format("Entered {0}", filterContext.ControllerContext.Controller.GetType().FullName));
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            LogManager.GetLogger("webapiskeleton.logger")
                .Info(string.Format("Exited {0}",
                    filterContext.ActionContext.ControllerContext.Controller.GetType().FullName));
        }
    }
}