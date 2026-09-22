using System.Net;
using System.Text.Json;
using Fgs.Contracts.Api;
using Fgs.Foundation.Correlation;
using Fgs.Foundation.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fgs.Foundation.Tests.Middleware;

public sealed class ExceptionHandlingMiddlewareTests
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task InvokeAsync_WritesApiResponseWithCorrelationHeader()
    {
        var correlationId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        var services = new ServiceCollection();
        services.AddSingleton<ICorrelationContext>(new FixedCorrelationContext(correlationId));
        var provider = services.BuildServiceProvider();

        var context = new DefaultHttpContext { RequestServices = provider };
        context.Request.Path = "/api/v1/invoice";
        context.Response.Body = new MemoryStream();

        var middleware = new ExceptionHandlingMiddleware(
            _ => throw new InvalidOperationException("boom"),
            NullLogger<ExceptionHandlingMiddleware>.Instance,
            []);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        context.Response.ContentType.Should().Be("application/json");
        context.Response.Headers["X-Correlation-ID"].ToString()
            .Should().Be(correlationId.ToString("N"));

        var payload = await ReadApiResponseAsync(context);
        payload.Success.Should().BeFalse();
        payload.StatusCode.Should().Be(409);
        payload.Data.Should().BeNull();
        payload.Errors.Should().ContainSingle("boom");
    }

    [Fact]
    public async Task InvokeAsync_UnhandledException_Returns500ApiResponse()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        var context = new DefaultHttpContext { RequestServices = provider };
        context.Request.Path = "/api/v1/unknown";
        context.TraceIdentifier = "trace-abc";
        context.Response.Body = new MemoryStream();

        var middleware = new ExceptionHandlingMiddleware(
            _ => throw new Exception("unexpected"),
            NullLogger<ExceptionHandlingMiddleware>.Instance,
            []);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.Should().Be("application/json");
        context.Response.Headers["X-Correlation-ID"].ToString().Should().Be("trace-abc");

        var payload = await ReadApiResponseAsync(context);
        payload.Success.Should().BeFalse();
        payload.StatusCode.Should().Be(500);
        payload.Errors.Should().ContainSingle("unexpected");
    }

    [Fact]
    public async Task InvokeAsync_UsesCustomMapperWhenProvided()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var context = new DefaultHttpContext { RequestServices = provider };
        context.Response.Body = new MemoryStream();

        var mapper = new FixedMapper(HttpStatusCode.UnprocessableEntity, ["mapped"]);
        var middleware = new ExceptionHandlingMiddleware(
            _ => throw new InvalidOperationException("ignored"),
            NullLogger<ExceptionHandlingMiddleware>.Instance,
            [mapper]);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(422);
        var payload = await ReadApiResponseAsync(context);
        payload.Errors.Should().ContainSingle("mapped");
        payload.StatusCode.Should().Be(422);
    }

    [Fact]
    public async Task InvokeAsync_ValidationException_Returns400ApiResponseWithAllErrors()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var context = new DefaultHttpContext { RequestServices = provider };
        context.Response.Body = new MemoryStream();

        var exception = new FluentValidation.ValidationException(
        [
            new FluentValidation.Results.ValidationFailure("EmployeeNumber", "An employee with this employee number already exists."),
            new FluentValidation.Results.ValidationFailure("UserId", "An employee linked to this user already exists."),
            new FluentValidation.Results.ValidationFailure("TechCode", "A technician with this tech code already exists.")
        ]);

        var middleware = new ExceptionHandlingMiddleware(
            _ => throw exception,
            NullLogger<ExceptionHandlingMiddleware>.Instance,
            []);

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(400);
        context.Response.ContentType.Should().Be("application/json");

        var payload = await ReadApiResponseAsync(context);
        payload.Success.Should().BeFalse();
        payload.StatusCode.Should().Be(400);
        payload.Errors.Should().HaveCount(3);
        payload.Errors.Should().Contain("An employee with this employee number already exists.");
        payload.Errors.Should().Contain("An employee linked to this user already exists.");
        payload.Errors.Should().Contain("A technician with this tech code already exists.");
    }

    private static async Task<ApiResponse<object>> ReadApiResponseAsync(HttpContext context)
    {
        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        body.Should().NotContain("httpstatuses.com");
        body.Should().NotContain("\"title\"");
        body.Should().NotContain("\"detail\"");
        body.Should().NotContain("\"instance\"");
        body.Should().NotContain("\"traceId\"");
        body.Should().NotContain("\"correlationId\"");

        return JsonSerializer.Deserialize<ApiResponse<object>>(body, SerializerOptions)
               ?? throw new InvalidOperationException("Failed to deserialize ApiResponse.");
    }

    private sealed class FixedCorrelationContext(Guid id) : ICorrelationContext
    {
        public Guid GetCorrelationId() => id;
    }

    private sealed class FixedMapper(HttpStatusCode status, IReadOnlyList<string> errors) : IExceptionStatusMapper
    {
        public bool TryMap(Exception exception, out ExceptionMapping mapping)
        {
            mapping = new ExceptionMapping(status, errors);
            return true;
        }
    }
}
