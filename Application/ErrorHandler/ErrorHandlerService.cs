using Domain.Abstractions;
using Domain.CustomException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace Application.ErrorHandler
{
    public class ErrorHandlerService : IErrorHandler
    {
        async Task IErrorHandler.Handle(Exception exception, HttpContext context)
        {
            var ErrorCode = Guid.NewGuid().ToString();
            Log.Error("Tracking Code {0} => " + exception.Message + ErrorCode);

            ProblemDetails details = new();

            if (exception is ArgumentException ||
                exception is ArgumentOutOfRangeException ||
                exception is ArgumentNullException)
            {
                details.Status = (int)HttpStatusCode.BadRequest;
                details.Title = "Invalid Request Parameters or body.";
                details.Detail = $"Request tracking Code is {ErrorCode}";

                await ManageResponse(details, context);
            }

            if (exception is MyCustomException)
            {
                details.Status = (int)HttpStatusCode.MethodNotAllowed;
                details.Title = "You Shouldn't do something in a specific step of process.";
                details.Detail = $"Request tracking Code is {ErrorCode}";

                await ManageResponse(details, context);
            }

            // Other exceptions ...... 

            // Unknown Error

            details.Status = (int)HttpStatusCode.InternalServerError;
            details.Title = $"Ooops, Something went wrong.";
            details.Detail = $"Request tracking Code is {ErrorCode}";

            await ManageResponse(details, context);
        }

        private async Task ManageResponse(ProblemDetails details, HttpContext context)
        {
            await context.Response.WriteAsJsonAsync(details);
            return;
        }
    }
}