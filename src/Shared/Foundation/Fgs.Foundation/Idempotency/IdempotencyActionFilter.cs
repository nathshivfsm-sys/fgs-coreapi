using System.Text.Json;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Fgs.Foundation.Idempotency;

public sealed class IdempotencyActionFilter(
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<IdempotencyActionFilter> logger) : IAsyncActionFilter
{
    public const string HeaderName = "Idempotency-Key";
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromHours(24);

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var hasAttribute = context.ActionDescriptor.EndpointMetadata
            .OfType<IdempotentAttribute>()
            .Any();
        if (!hasAttribute)
        {
            await next();
            return;
        }

        if (!HttpMethods.IsPost(context.HttpContext.Request.Method)
            && !HttpMethods.IsPut(context.HttpContext.Request.Method)
            && !HttpMethods.IsPatch(context.HttpContext.Request.Method))
        {
            await next();
            return;
        }

        var keyHeader = context.HttpContext.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(keyHeader))
        {
            await next();
            return;
        }

        var cacheKey = BuildCacheKey(context.HttpContext, keyHeader.Trim());
        var cached = await cache.GetAsync<IdempotentResponseCacheEntry>(cacheKey, context.HttpContext.RequestAborted);
        if (cached is not null)
        {
            logger.LogInformation("Idempotency cache hit for {CacheKey}", cacheKey);
            context.Result = new ContentResult
            {
                StatusCode = cached.StatusCode,
                ContentType = cached.ContentType,
                Content = cached.Body
            };
            context.HttpContext.Response.Headers["X-Idempotency-Replayed"] = "true";
            return;
        }

        var executed = await next();
        if (executed.Exception is not null || executed.Canceled)
        {
            return;
        }

        if (executed.Result is not ObjectResult { Value: not null } objectResult)
        {
            return;
        }

        var statusCode = objectResult.StatusCode
            ?? context.HttpContext.Response.StatusCode;
        if (statusCode is < 200 or >= 300)
        {
            return;
        }

        var body = JsonSerializer.Serialize(objectResult.Value);
        var entry = new IdempotentResponseCacheEntry(
            statusCode,
            "application/json",
            body);

        await cache.SetAsync(cacheKey, entry, DefaultTtl, context.HttpContext.RequestAborted);
    }

    private string BuildCacheKey(HttpContext httpContext, string idempotencyKey)
    {
        var tenant = tenantContextAccessor.Current;
        var tenantPart = tenant is null
            ? "none"
            : $"{tenant.TenantId}:{tenant.CompanyId}";
        var path = httpContext.Request.Path.Value ?? "/";
        return $"fgs:http:idempotency:{tenantPart}:{httpContext.Request.Method}:{path}:{idempotencyKey}";
    }
}

public sealed record IdempotentResponseCacheEntry(int StatusCode, string ContentType, string Body);
