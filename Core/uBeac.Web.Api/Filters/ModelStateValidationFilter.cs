using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using uBeac.Web.Api.Controllers;

namespace uBeac.Web.Api.Filters
{
    public class ModelStateValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
            }
            else
            {
                var result = new Response<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };

                foreach (string key in context.ModelState.Keys)
                {
                    foreach (var error in context.ModelState[key].Errors)
                    {
                        result.Message = "Bad Data! Try with correct values!";
                        result.Errors.Add("ModelStateValidation", error.ErrorMessage);
                    }
                }
                context.Result = new ObjectResult(result);
            }
        }
    }
}
