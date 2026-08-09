using Fgs.Foundation.Caching.Abstractions;
using Fgs.Foundation.Idempotency;
using Fgs.MultiTenancy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fgs.Foundation.Tests.Idempotency;

public sealed class IdempotencyActionFilterTests
{
    [Fact]
    public async Task WithoutAttribute_DoesNotTouchCache()
    {
        var cache = new Mock<ICacheService>(MockBehavior.Strict);
        var filter = new IdempotencyActionFilter(
            cache.Object,
            new TenantContextAccessor(),
            NullLogger<IdempotencyActionFilter>.Instance);

        var context = CreateContext(hasAttribute: false, idempotencyKey: "k1");
        await filter.OnActionExecutionAsync(context, () =>
        {
            context.Result = new ObjectResult(new { ok = true }) { StatusCode = 201 };
            return Task.FromResult(new ActionExecutedContext(context, [], context.Controller)
            {
                Result = context.Result
            });
        });

        cache.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task WithCachedEntry_ReplaysWithoutInvokingAction()
    {
        var cache = new InMemoryCacheService();
        var accessor = new TenantContextAccessor
        {
            Current = new TenantContext { TenantId = 1, CompanyId = 2 }
        };
        var filter = new IdempotencyActionFilter(
            cache,
            accessor,
            NullLogger<IdempotencyActionFilter>.Instance);

        var context = CreateContext(hasAttribute: true, idempotencyKey: "abc");
        var key = "fgs:http:idempotency:1:2:POST:/api/v1/invoice:abc";
        await cache.SetAsync(
            key,
            new IdempotentResponseCacheEntry(201, "application/json", "{\"replayed\":true}"));

        var nextCalled = false;
        await filter.OnActionExecutionAsync(context, () =>
        {
            nextCalled = true;
            return Task.FromResult(new ActionExecutedContext(context, [], context.Controller));
        });

        nextCalled.Should().BeFalse();
        var content = context.Result.Should().BeOfType<ContentResult>().Subject;
        content.StatusCode.Should().Be(201);
        content.Content.Should().Contain("replayed");
        context.HttpContext.Response.Headers["X-Idempotency-Replayed"].ToString().Should().Be("true");
    }

    [Fact]
    public async Task SuccessfulCreate_CachesResponse()
    {
        var cache = new InMemoryCacheService();
        var filter = new IdempotencyActionFilter(
            cache,
            new TenantContextAccessor { Current = new TenantContext { TenantId = 9, CompanyId = 8 } },
            NullLogger<IdempotencyActionFilter>.Instance);

        var context = CreateContext(hasAttribute: true, idempotencyKey: "new-key");
        await filter.OnActionExecutionAsync(context, () =>
        {
            context.Result = new ObjectResult(new { id = 42 }) { StatusCode = 201 };
            return Task.FromResult(new ActionExecutedContext(context, [], context.Controller)
            {
                Result = context.Result
            });
        });

        var cached = await cache.GetAsync<IdempotentResponseCacheEntry>(
            "fgs:http:idempotency:9:8:POST:/api/v1/invoice:new-key");
        cached.Should().NotBeNull();
        cached!.StatusCode.Should().Be(201);
        cached.Body.Should().Contain("42");
    }

    [Fact]
    public async Task MissingIdempotencyHeader_DoesNotCache()
    {
        var cache = new Mock<ICacheService>(MockBehavior.Strict);
        var filter = new IdempotencyActionFilter(
            cache.Object,
            new TenantContextAccessor(),
            NullLogger<IdempotencyActionFilter>.Instance);

        var context = CreateContext(hasAttribute: true, idempotencyKey: null);
        await filter.OnActionExecutionAsync(context, () =>
        {
            context.Result = new ObjectResult(new { id = 1 }) { StatusCode = 201 };
            return Task.FromResult(new ActionExecutedContext(context, [], context.Controller)
            {
                Result = context.Result
            });
        });

        cache.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetMethod_SkipsIdempotencyEvenWithAttribute()
    {
        var cache = new Mock<ICacheService>(MockBehavior.Strict);
        var filter = new IdempotencyActionFilter(
            cache.Object,
            new TenantContextAccessor(),
            NullLogger<IdempotencyActionFilter>.Instance);

        var context = CreateContext(hasAttribute: true, idempotencyKey: "k1", method: HttpMethods.Get);
        await filter.OnActionExecutionAsync(context, () =>
            Task.FromResult(new ActionExecutedContext(context, [], context.Controller)));

        cache.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task NonSuccessStatus_DoesNotCache()
    {
        var cache = new InMemoryCacheService();
        var filter = new IdempotencyActionFilter(
            cache,
            new TenantContextAccessor { Current = new TenantContext { TenantId = 1, CompanyId = 1 } },
            NullLogger<IdempotencyActionFilter>.Instance);

        var context = CreateContext(hasAttribute: true, idempotencyKey: "fail-key");
        await filter.OnActionExecutionAsync(context, () =>
        {
            context.Result = new ObjectResult(new { error = true }) { StatusCode = 400 };
            return Task.FromResult(new ActionExecutedContext(context, [], context.Controller)
            {
                Result = context.Result
            });
        });

        var cached = await cache.GetAsync<IdempotentResponseCacheEntry>(
            "fgs:http:idempotency:1:1:POST:/api/v1/invoice:fail-key");
        cached.Should().BeNull();
    }

    [Fact]
    public async Task NullTenant_UsesNoneSegmentInCacheKey()
    {
        var cache = new InMemoryCacheService();
        var filter = new IdempotencyActionFilter(
            cache,
            new TenantContextAccessor(),
            NullLogger<IdempotencyActionFilter>.Instance);

        var context = CreateContext(hasAttribute: true, idempotencyKey: "anon");
        await filter.OnActionExecutionAsync(context, () =>
        {
            context.Result = new ObjectResult(new { id = 7 }) { StatusCode = 201 };
            return Task.FromResult(new ActionExecutedContext(context, [], context.Controller)
            {
                Result = context.Result
            });
        });

        var cached = await cache.GetAsync<IdempotentResponseCacheEntry>(
            "fgs:http:idempotency:none:POST:/api/v1/invoice:anon");
        cached.Should().NotBeNull();
    }

    private static ActionExecutingContext CreateContext(
        bool hasAttribute,
        string? idempotencyKey,
        string? method = null)
    {
        var http = new DefaultHttpContext();
        http.Request.Method = method ?? HttpMethods.Post;
        http.Request.Path = "/api/v1/invoice";
        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            http.Request.Headers[IdempotencyActionFilter.HeaderName] = idempotencyKey;
        }

        var actionDescriptor = new ActionDescriptor();
        if (hasAttribute)
        {
            actionDescriptor.EndpointMetadata = [new IdempotentAttribute()];
        }

        var actionContext = new ActionContext(
            http,
            new Microsoft.AspNetCore.Routing.RouteData(),
            actionDescriptor);
        return new ActionExecutingContext(
            actionContext,
            [],
            new Dictionary<string, object?>(),
            controller: new object());
    }

    private sealed class InMemoryCacheService : ICacheService
    {
        private readonly Dictionary<string, object> _store = new(StringComparer.Ordinal);

        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class =>
            Task.FromResult(_store.TryGetValue(key, out var value) ? value as T : null);

        public Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? absoluteExpiration = null,
            CancellationToken cancellationToken = default) where T : class
        {
            _store[key] = value;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            _store.Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        {
            foreach (var key in _store.Keys.Where(k => k.StartsWith(prefix, StringComparison.Ordinal)).ToList())
            {
                _store.Remove(key);
            }

            return Task.CompletedTask;
        }

        public async Task<T?> GetOrSetAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan? absoluteExpiration = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var existing = await GetAsync<T>(key, cancellationToken);
            if (existing is not null)
            {
                return existing;
            }

            var created = await factory();
            await SetAsync(key, created, absoluteExpiration, cancellationToken);
            return created;
        }
    }
}
