using System;
using System.Net;
using FlaschenPost.Drinks;
using Microsoft.AspNetCore.Mvc;

namespace FlaschenPost.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                InvalidContractException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = exception.Message,
                Instance = context.Request.Path
            });
        }
    }
}

