using Microsoft.AspNetCore.Http;

namespace Domain.Abstractions;

public interface IErrorHandler
{
    Task Handle(Exception exception, HttpContext context);
}
