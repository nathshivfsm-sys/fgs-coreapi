using System.Net;
using Fgs.Foundation.Correlation;
using Fgs.Foundation.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fgs.Foundation.Tests.Middleware;

public sealed class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WritesProblemJsonWithCorrelationAndApiFields()
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
        context.Response.ContentType.Should().Be("application/problem+json");
        context.Response.Headers["X-Correlation-ID"].ToString()
            .Should().Be(correlationId.ToString("N"));

        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        body.Should().Contain("\"success\":false");
        body.Should().Contain("\"statusCode\":409");
        body.Should().Contain(correlationId.ToString("N"));
        body.Should().Contain("boom");
        body.Should().Contain("httpstatuses.com");
        body.Should().Contain("\"title\"");
        body.Should().Contain("Conflict");
    }

    [Fact]
    public async Task InvokeAsync_UnhandledException_Returns500ProblemJson()
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
        context.Response.ContentType.Should().Be("application/problem+json");
        context.Response.Headers["X-Correlation-ID"].ToString().Should().Be("trace-abc");

        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        body.Should().Contain("\"statusCode\":500");
        body.Should().Contain("\"success\":false");
        body.Should().Contain("Internal Server Error");
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
        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        body.Should().Contain("mapped");
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
