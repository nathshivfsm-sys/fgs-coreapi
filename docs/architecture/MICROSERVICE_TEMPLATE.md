# FGS Microservice Template

## Layering

| Layer | Responsibility |
|-------|----------------|
| **API** | Thin controllers inheriting `FgsApiControllerBase`; `IMediator` only; `UseFgsFoundationMiddleware()` |
| **Application** | CQRS (`Features/{Area}/Commands|Queries/{UseCase}/`), `IRequest<ApiResponse<T>>`, validators, abstractions |
| **Infrastructure** | EF, Refit clients, hosted workers, repository implementations |
| **Domain** | Entities and enums |

## DI order (`Program.cs`)

```csharp
var hostOptions = builder.AddFgsApiHost(options =>
{
    options.ServiceName = "fgs-{service}-service";
    // ...
});
builder.Services.AddFgs{Service}Application();
builder.Services.AddFgs{Service}Infrastructure(builder.Configuration);
builder.AddFgsObservability(hostOptions.ServiceName);
// ...
app.UseFgsApiHost(hostOptions);
app.MapFgsHealthChecks();
```

`AddFgsFoundation()` is registered **only** inside `AddFgs{Service}Application()`.

Observability (Datadog APM, Serilog JSON, DogStatsD, health) is shared via `Fgs.Observability` — see [docs/observability/DATADOG.md](../observability/DATADOG.md). Use the same `Datadog` + `Serilog` appsettings sections on every API; do not add per-service Serilog/Datadog packages.

## Data access

- Register: `services.AddFgsPersistence<{Service}DbContext>();`
- Handlers use `IUnitOfWork` / `IRepository<TEntity>` — never `DbContext` in Application or API.
- Complex reads: optional `I{Entity}Repository` in Application, implemented in Infrastructure.

## HTTP between services

- Refit interfaces in `Fgs.Contracts.Clients`
- Register: `services.AddFgsRefitClient<TClient>(configuration, "OtherService:BaseUrl", defaultUrl);`
- Responses: `Task<ApiResponse<T>>`; unwrap with `EnsureSuccess()` / `ThrowIfFailed()`
- No raw `HttpClient` for service-to-service calls

## API responses

- All JSON endpoints return `ApiResponse<T>` (`Fgs.Contracts.Api`)
- Controllers: `FromApiResponse(await Mediator.Send(...))`
- Unhandled exceptions → `ExceptionHandlingMiddleware` → `ApiResponse<object>.Fail`

## Resilience

- HTTP: Polly via `Microsoft.Extensions.Http.Resilience` on Refit (`AddFgsRefitClient`)
- EF: `options.UseFgsNpgsql(...)` (retry on failure)
- Config section: `Resilience:Http`

## Workers

Hosted services resolve `IMediator` in a scope and `Send` commands — no business logic in the worker loop.
