using System.Text.Json;
using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.GridPreferences;
using Fgs.User.Application.Features.GridPreferences.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Fgs.User.Infrastructure.Entities.GridPreferences;

public sealed class FgsUserGridPreferenceStore(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContext tenantContext,
    IFgsUserContext userContext) : IFgsUserGridPreferenceStore
{
    private const int MaxCodeLength = 100;

    private const int MaxColumnCount = 100;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<FgsUserGridPreferenceDto> GetAsync(
        string gridCode,
        CancellationToken cancellationToken = default)
    {
        var scope = RequireScope();
        var code = NormalizeGridCode(gridCode);
        var entity = await context.FgsUserGridPreferences
            .AsNoTracking()
            .FirstOrDefaultAsync(
                preference => preference.TenantId == scope.TenantId
                    && preference.CompanyId == scope.CompanyId
                    && preference.UserId == scope.UserId
                    && preference.GridCode == code,
                cancellationToken);

        var columns = entity is null ? [] : ReadColumns(entity.GridConfiguration);
        return new FgsUserGridPreferenceDto(code, columns);
    }

    public async Task<FgsUserGridPreferenceDto> UpsertAsync(
        string gridCode,
        IReadOnlyList<string> columns,
        CancellationToken cancellationToken = default)
    {
        var scope = RequireScope();
        var code = NormalizeGridCode(gridCode);
        var normalizedColumns = NormalizeColumns(columns);
        var json = Serialize(normalizedColumns);
        var now = DateTimeOffset.UtcNow;
        var actor = scope.UserId.ToString("D");

        var existing = await TrackedAsync(scope, code, cancellationToken);
        if (existing is not null)
        {
            existing.GridConfiguration = json;
            existing.UpdatedOn = now;
            existing.UpdatedBy = actor;
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new FgsUserGridPreferenceDto(code, normalizedColumns);
        }

        var entity = new FgsUserGridPreference
        {
            TenantId = scope.TenantId,
            CompanyId = scope.CompanyId,
            UserId = scope.UserId,
            GridCode = code,
            GridConfiguration = json,
            CreatedOn = now,
            CreatedBy = actor
        };

        await context.FgsUserGridPreferences.AddAsync(entity, cancellationToken);
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsUniqueViolation(exception))
        {
            foreach (var entry in exception.Entries)
            {
                entry.State = EntityState.Detached;
            }

            var concurrent = await TrackedAsync(scope, code, cancellationToken)
                ?? throw new InvalidOperationException(
                    "The grid preference was created concurrently and could not be updated.",
                    exception);

            concurrent.GridConfiguration = json;
            concurrent.UpdatedOn = now;
            concurrent.UpdatedBy = actor;
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new FgsUserGridPreferenceDto(code, normalizedColumns);
    }

    public async Task DeleteAsync(string gridCode, CancellationToken cancellationToken = default)
    {
        var scope = RequireScope();
        var code = NormalizeGridCode(gridCode);
        var entity = await TrackedAsync(scope, code, cancellationToken);
        if (entity is null)
        {
            return;
        }

        context.FgsUserGridPreferences.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private CallerScope RequireScope()
    {
        if (!userContext.IsAuthenticated || userContext.UserId is not Guid userId)
        {
            throw new UnauthorizedAccessException("A signed-in user is required.");
        }

        return new CallerScope(tenantContext.TenantId, tenantContext.CompanyId, userId);
    }

    private Task<FgsUserGridPreference?> TrackedAsync(
        CallerScope scope,
        string gridCode,
        CancellationToken cancellationToken) =>
        context.FgsUserGridPreferences.FirstOrDefaultAsync(
            preference => preference.TenantId == scope.TenantId
                && preference.CompanyId == scope.CompanyId
                && preference.UserId == scope.UserId
                && preference.GridCode == gridCode,
            cancellationToken);

    private static string NormalizeGridCode(string gridCode)
    {
        if (string.IsNullOrWhiteSpace(gridCode))
        {
            throw new ArgumentException("Grid code is required.");
        }

        var trimmed = gridCode.Trim();
        if (trimmed.Length > MaxCodeLength)
        {
            throw new ArgumentException("Grid code must be 100 characters or fewer.");
        }

        return trimmed;
    }

    private static IReadOnlyList<string> NormalizeColumns(IReadOnlyList<string> columns)
    {
        if (columns.Count > MaxColumnCount)
        {
            throw new ArgumentException("At most 100 column codes are allowed.");
        }

        var normalized = new string[columns.Count];
        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (var index = 0; index < columns.Count; index++)
        {
            var column = columns[index]?.Trim();
            if (string.IsNullOrEmpty(column) || column.Length > MaxCodeLength || !seen.Add(column))
            {
                throw new ArgumentException("Column codes must be non-blank, at most 100 characters, and distinct.");
            }

            normalized[index] = column;
        }

        return normalized;
    }

    private static string Serialize(IReadOnlyList<string> columns) =>
        JsonSerializer.Serialize(new GridConfigurationPayload { Columns = columns.ToList() }, JsonOptions);

    private static IReadOnlyList<string> ReadColumns(string json)
    {
        if (string.IsNullOrWhiteSpace(json) || json == "{}")
        {
            return [];
        }

        try
        {
            var payload = JsonSerializer.Deserialize<GridConfigurationPayload>(json, JsonOptions);
            return payload?.Columns ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (current is PostgresException postgres && postgres.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                return true;
            }

            if (current.Message.Contains("23505", StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private sealed class GridConfigurationPayload
    {
        public List<string> Columns { get; set; } = [];
    }

    private readonly record struct CallerScope(long TenantId, long CompanyId, Guid UserId);
}
