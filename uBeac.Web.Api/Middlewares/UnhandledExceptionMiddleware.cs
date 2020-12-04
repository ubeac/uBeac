using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using uBeac.Web.Api.Controllers;

namespace uBeac.Web.Middlewares
{
    public class UnhandledExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public UnhandledExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static Task HandleException(HttpContext context, Exception ex)
        {

            var resultSet = new Response
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            resultSet.Errors.Add("SERVER_ERROR", ex.Message);

            string result = JsonConvert.SerializeObject(resultSet);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsync(result);
        }
    }
}
