using E_Commerce.Services.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.CustomMiddleWares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlerMiddleWare> logger;

        public ExceptionHandlerMiddleWare(RequestDelegate Next, ILogger<ExceptionHandlerMiddleWare> logger)
        {
            next = Next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {

                await next.Invoke(httpContext);
                await HandleNotFoundEndPointAsync(httpContext);

            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Something Went Wrong");
                

                var Problem = new ProblemDetails()
                {
                    Title = "An UnExpectedErrorOccured",
                    
                    Detail = ex.Message,
                    Instance = httpContext.Request.Path,
                    Status = ex switch {
                    
                        NotFoundException => StatusCodes.Status404NotFound,
                        _=> StatusCodes.Status500InternalServerError
                    }
                };
                httpContext.Response.StatusCode = Problem.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(Problem);

            }
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Problem = new ProblemDetails()
                {
                    Title = "Error will Processing The Http Request - EndPoint Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"End Point {httpContext.Request.Path} is not found",
                    Instance = httpContext.Request.Path
                };
                await httpContext.Response.WriteAsJsonAsync(Problem);

            }
        }
    }
}
