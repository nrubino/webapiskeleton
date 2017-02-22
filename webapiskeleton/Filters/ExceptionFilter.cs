using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using log4net;

namespace webapiskeleton.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnException(HttpActionExecutedContext filterContext)
        {
            var guidId = Guid.NewGuid();
            var errorTime = DateTime.Now.ToString("O");
            LogManager.GetLogger("webapiskeleton.logger").Error(string.Format("Exception Occured in {0}, Error Time : {1}, Error Id : {2}", filterContext.ActionContext.ControllerContext.Controller.GetType().FullName, errorTime, guidId));
            LogManager.GetLogger("webapiskeleton.logger").Error(filterContext.Exception);

            filterContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            filterContext.Response = filterContext.Request.CreateResponse(
                HttpStatusCode.BadRequest,
                new
                {
                    error = true,
                    guid = guidId,
                    datetime = errorTime
                });
        }
    }
}
