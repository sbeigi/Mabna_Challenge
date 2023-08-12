using Domain.Abstractions;

namespace Mabna.Middleware;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class GlobalErrorHandler : IMiddleware
{
    public GlobalErrorHandler(IErrorHandler errorHandler)
    {
        _ErrorHandler = errorHandler;
    }

    public IErrorHandler _ErrorHandler { get; }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await _ErrorHandler.Handle(ex, context);
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class GlobalErrorHandlerExtensions
{
    public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalErrorHandler>();
    }
}
