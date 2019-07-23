using System.Linq;

using Microsoft.AspNetCore.Mvc.Filters;

using NLog;

namespace Guestlogix.Api.Extensions
{
    public class LogActionFilterAttribute : ActionFilterAttribute
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _logger.Trace($"{actionContext.HttpContext.Request.Path.Value}, {actionContext.HttpContext.Request.Method}, ModelState: {actionContext.ModelState.IsValid}");

            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage + (e.Exception != null ? " " + e.Exception.Message : ""));
                _logger.Error($"{actionContext.HttpContext.Request.Path.Value}, {actionContext.HttpContext.Request.Method}, Invalid data: {string.Join("; ", errors)}");
            }

            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            _logger.Trace($"{actionExecutedContext.HttpContext.Request.Path.Value}, Status: {actionExecutedContext.HttpContext.Response.StatusCode}");
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
