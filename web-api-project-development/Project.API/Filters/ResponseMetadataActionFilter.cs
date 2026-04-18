using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Project.Core.Entities.Business;

namespace Project.API.Filters
{
    public class ResponseMetadataActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // No-op
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is IResponseViewModel response)
            {
                response.RequestId = context.HttpContext.TraceIdentifier;
                response.Timestamp = DateTime.UtcNow;
            }
        }
    }
}
