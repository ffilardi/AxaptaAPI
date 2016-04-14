using AxaptaApiApp.Utils;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AxaptaApiApp.Filters
{
    public class CacheFilter : ActionFilterAttribute
    {
        private const int cacheHours = 1;
        public string cacheKey { get; private set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            cacheKey = String.Format("{0}.{1}",
                actionContext.ControllerContext.ControllerDescriptor.ControllerType.Name,
                actionContext.ActionDescriptor.ActionName);

            foreach (var value in actionContext.ActionArguments.Values)
            {
                cacheKey = String.Format("{0}.{1}", cacheKey, value);
            }

            HttpResponseMessage response = MemoryCacheHelper.GetValue<HttpResponseMessage>(cacheKey);
            object content;

            if (response != null)
            {
                response.TryGetContentValue(out content);
                actionContext.Response = actionContext.Request.CreateResponse(response.StatusCode, content);
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.ActionContext.Request.Method == HttpMethod.Get)
            {
                MemoryCacheHelper.Add(cacheKey, actionExecutedContext.Response, DateTimeOffset.UtcNow.AddHours(cacheHours));
            }
        }
    }
}