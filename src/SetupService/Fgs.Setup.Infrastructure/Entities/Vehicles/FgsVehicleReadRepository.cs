using Dapper;
using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;
using Fgs.MultiTenancy;
using Fgs.Setup.Application.Abstractions.Persistence;
using Fgs.Setup.Application.Abstractions.Vehicles;
using Fgs.Setup.Application.Common.SetupCrud;
using Fgs.Setup.Application.Features.Vehicles.Dtos;

namespace Fgs.Setup.Infrastructure.Entities.Vehicles;

internal sealed class FgsVehicleReadRepository : IFgsVehicleReadRepository
{
    private readonly ISetupReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsVehicleReadRepository(
        ISetupReadConnectionFactory connectionFactory,
        ITenantContextAccessor tenantContextAccessor)
    {
        _connectionFactory = connectionFactory;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsVehicleDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = $"""
            SELECT {FgsVehicleSql.SelectDetailColumns}
            FROM {FgsVehicleSql.Table}
            WHERE "Id" = @Id
              AND "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var row = await connection.QueryFirstOrDefaultAsync<FgsVehicleDetailRow>(
            new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return row?.ToDto();
    }

    public async Task<PagedResult<FgsVehicleSummaryDto>> ListAsync(
        SetupListQuery query,
        FgsVehicleListFilters filters,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var paging = query.ToPagedQuery();
        var page = Math.Max(1, paging.Page);
        var pageSize = Math.Clamp(paging.PageSize, 1, 200);
        var offset = (page - 1) * pageSize;

        var where = new List<string>
        {
            "\"TenantId\" = @TenantId",
            "\"CompanyId\" = @CompanyId"
        };

        if (paging.IsActive.HasValue)
        {
            where.Add("\"IsActive\" = @IsActive");
        }

        if (!string.IsNullOrWhiteSpace(filters.VIN))
        {
            where.Add("\"VIN\" ILIKE @VIN");
        }

        if (filters.InventoryLocationId.HasValue)
        {
            where.Add("\"InventoryLocationId\" = @InventoryLocationId");
        }

        if (!string.IsNullOrWhiteSpace(paging.Search))
        {
            where.Add(
                "(\"VIN\" ILIKE @Search OR \"Make\" ILIKE @Search OR \"Model\" ILIKE @Search OR \"LicensePlate\" ILIKE @Search)");
        }

        var whereClause = string.Join(" AND ", where);
        var orderBy = FgsVehicleSql.ResolveOrderBy(paging.SortBy, paging.SortDirection);

        var sql = $"""
            SELECT {FgsVehicleSql.SelectSummaryColumns}
            FROM {FgsVehicleSql.Table}
            WHERE {whereClause}
            {orderBy}
            LIMIT @PageSize OFFSET @Offset;

            SELECT COUNT(*)
            FROM {FgsVehicleSql.Table}
            WHERE {whereClause};
            """;

        var parameters = new
        {
            TenantId = tenantId,
            CompanyId = companyId,
            IsActive = paging.IsActive,
            VIN = string.IsNullOrWhiteSpace(filters.VIN) ? null : $"%{filters.VIN.Trim()}%",
            InventoryLocationId = filters.InventoryLocationId,
            Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%",
            PageSize = pageSize,
            Offset = offset
        };

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

        var rows = (await multi.ReadAsync<FgsVehicleSummaryRow>()).ToList();
        var totalCount = await multi.ReadSingleAsync<int>();

        return new PagedResult<FgsVehicleSummaryDto>(
            rows.Select(r => r.ToDto()).ToList(),
            page,
            pageSize,
            totalCount);
    }

    public async Task<IReadOnlyList<FgsVehicleLookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var activeFilter = activeOnly ? "AND \"IsActive\" = TRUE" : string.Empty;
        var sql = $"""
            SELECT {FgsVehicleSql.SelectLookupColumns}
            FROM {FgsVehicleSql.Table}
            WHERE "TenantId" = @TenantId
              AND "CompanyId" = @CompanyId
              {activeFilter}
            ORDER BY "VIN" ASC
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<FgsVehicleLookupRow>(
            new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToDto()).ToList();
    }

    public async Task<bool> ExistsInventoryLocationIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = SetupTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var sql = """
            SELECT EXISTS(
                SELECT 1
                FROM inventory."FgsInventoryLocation"
                WHERE "TenantId" = @TenantId AND "CompanyId" = @CompanyId AND "Id" = @Id AND "IsActive" = TRUE
            )
            """;

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, CompanyId = companyId, Id = id },
                cancellationToken: cancellationToken));
    }
}
