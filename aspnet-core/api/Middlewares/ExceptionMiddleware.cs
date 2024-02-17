using repository.Helpers;
using System.Net;
using System.Text.Json;

namespace api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment env;
        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env) 
        {
            this.next = next;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) 
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                object response;

                if (env.IsDevelopment())
                {
                    response = new { message = ex.Message, stackTrace = ex.StackTrace };
                }
                else
                {
                    response = new { message = "An error occured" };
                    HelperSerilog.LogException(ex);
                }
                
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
