using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserService.Application.Common.Models;

namespace UserService.API.Filters;

public sealed class ValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ValidationException vex)
            return;

        var errors = vex.Errors.Select(e => e.ErrorMessage).ToList();
        context.Result = new ObjectResult(ApiResponse<object?>.Fail(400, errors))
        {
            StatusCode = 400
        };
        context.ExceptionHandled = true;
    }
}
