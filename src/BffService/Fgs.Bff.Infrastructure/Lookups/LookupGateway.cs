using System.Net.Http.Json;
using System.Text.Json;
using Fgs.Bff.Application.Features.Lookups;
using Fgs.Bff.Application.Features.Lookups.Abstractions;
using Fgs.Bff.Application.Features.Lookups.Dtos;
using Microsoft.Extensions.Logging;

namespace Fgs.Bff.Infrastructure.Lookups;

public sealed class LookupGateway(
    IHttpClientFactory httpClientFactory,
    ILogger<LookupGateway> logger) : ILookupGateway
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<LookupResultDto> GetLookupAsync(
        LookupRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (!LookupCatalog.TryGet(request.Key, out var definition))
        {
            return new LookupResultDto(request.Key, [], $"Unknown lookup key '{request.Key}'.");
        }

        try
        {
            var client = httpClientFactory.CreateClient(definition.Service);
            var query = LookupQueryStringBuilder.Build(request, definition);
            var path = definition.RelativePath.TrimStart('/') + query;

            using var response = await client.GetAsync(path, cancellationToken);
            var payload = await response.Content.ReadFromJsonAsync<ApiEnvelope>(JsonOptions, cancellationToken);

            if (payload is null)
            {
                return new LookupResultDto(
                    request.Key,
                    [],
                    $"Empty response from {definition.Service} ({(int)response.StatusCode}).");
            }

            if (!payload.Success || payload.Data.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
            {
                var error = payload.Errors is { Count: > 0 }
                    ? string.Join("; ", payload.Errors)
                    : $"Lookup failed with status {payload.StatusCode}.";
                return new LookupResultDto(request.Key, [], error);
            }

            var items = LookupItemMapper.MapArray(payload.Data);
            return new LookupResultDto(request.Key, items);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            logger.LogWarning(
                ex,
                "Lookup gateway call failed for key {LookupKey} via {Service}",
                request.Key,
                definition.Service);

            return new LookupResultDto(request.Key, [], ex.Message);
        }
    }

    private sealed class ApiEnvelope
    {
        public bool Success { get; init; }
        public int StatusCode { get; init; }
        public JsonElement Data { get; init; }
        public IReadOnlyList<string>? Errors { get; init; }
    }
}
