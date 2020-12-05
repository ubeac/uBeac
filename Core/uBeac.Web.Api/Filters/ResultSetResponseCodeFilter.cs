using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using uBeac.Web.Api.Controllers;

namespace uBeac.Web.Api.Filters
{
    public class ResultSetResponseCodeFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is null)
                return;

            if (typeof(ObjectResult) != context.Result.GetType())
                return;

            var result = ((ObjectResult)context.Result).Value;

            if (result is IResponse resultSet) 
            {
                context.HttpContext.Response.StatusCode = resultSet.StatusCode;
            }
        }
    }
}
