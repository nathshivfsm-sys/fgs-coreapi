"""Remaining Asset CRUD entity generators (Model, Attribute, Option, Asset, Warranty)."""

FK_TYPE = """
    public async Task<bool> ExistsAssetTypeIdAsync(long assetTypeId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAssetType\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id AND \\\"IsActive\\\" = TRUE)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetTypeId }, cancellationToken: cancellationToken));
    }
"""

FK_MFR = """
    public async Task<bool> ExistsAssetManufacturerIdAsync(long assetManufacturerId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAssetManufacturer\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id AND \\\"IsActive\\\" = TRUE)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetManufacturerId }, cancellationToken: cancellationToken));
    }
"""

FK_STATUS = """
    public async Task<bool> ExistsAssetStatusIdAsync(long assetStatusId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAssetStatus\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id AND \\\"IsActive\\\" = TRUE)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetStatusId }, cancellationToken: cancellationToken));
    }
"""

FK_MODEL = """
    public async Task<bool> ExistsAssetModelIdAsync(long? assetModelId, CancellationToken cancellationToken = default)
    {
        if (!assetModelId.HasValue) return true;
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAssetModel\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id AND \\\"IsActive\\\" = TRUE)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetModelId.Value }, cancellationToken: cancellationToken));
    }
"""

FK_SVC_LOC = """
    public async Task<bool> ExistsServiceLocationIdAsync(long serviceLocationId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsServiceLocationCache\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"ServiceLocationId\\\" = @ServiceLocationId)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, ServiceLocationId = serviceLocationId }, cancellationToken: cancellationToken));
    }
"""

FK_ASSET = """
    public async Task<bool> ExistsAssetIdAsync(long assetId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAsset\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetId }, cancellationToken: cancellationToken));
    }
"""

FK_ATTR = """
    public async Task<bool> ExistsAssetAttributeIdAsync(long assetAttributeId, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAssetAttribute\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id AND \\\"IsActive\\\" = TRUE)";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetAttributeId }, cancellationToken: cancellationToken));
    }
"""


def gen_asset_model(write, gen_cqc, gen_tests):
    e, f, r = "FgsAssetModel", "AssetModels", "assetmodel"
    write(f"Fgs.Asset.Application/Abstractions/{f}/I{e}ReadRepository.cs", f"""using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.{f};
public interface I{e}ReadRepository
{{
    Task<{e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<{e}SummaryDto>> ListAsync(AssetListQuery query, {e}ListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<{e}LookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetTypeIdAsync(long assetTypeId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetManufacturerIdAsync(long assetManufacturerId, CancellationToken cancellationToken = default);
}}
""")
    write(f"Fgs.Asset.Application/Abstractions/{f}/I{e}WriteService.cs", f"""using Fgs.Asset.Application.Features.{f}.Dtos;
namespace Fgs.Asset.Application.Abstractions.{f};
public interface I{e}WriteService
{{
    Task<{e}DetailDto> CreateAsync({e}CreateDto dto, CancellationToken cancellationToken = default);
    Task<{e}DetailDto> UpdateAsync(long id, {e}UpdateDto dto, CancellationToken cancellationToken = default);
    Task<{e}DetailDto> PatchAsync(long id, {e}PatchDto dto, CancellationToken cancellationToken = default);
}}
""")
    write(f"Fgs.Asset.Application/Features/{f}/Dtos/{e}Dtos.cs", """namespace Fgs.Asset.Application.Features.AssetModels.Dtos;
public sealed record FgsAssetModelSummaryDto(long Id, long AssetTypeId, long AssetManufacturerId, string ModelNumber, string ModelDescription, bool IsActive);
public sealed record FgsAssetModelDetailDto(long Id, long AssetTypeId, long AssetManufacturerId, string ModelNumber, string ModelDescription, bool IsActive);
public sealed record FgsAssetModelLookupDto(long Id, string ModelNumber, string ModelDescription);
public sealed record FgsAssetModelCreateDto(long AssetTypeId, long AssetManufacturerId, string ModelNumber, string ModelDescription);
public sealed record FgsAssetModelUpdateDto(long AssetTypeId, long AssetManufacturerId, string ModelNumber, string ModelDescription);
public sealed record FgsAssetModelPatchDto(long? AssetTypeId, long? AssetManufacturerId, string? ModelNumber, string? ModelDescription, bool? IsActive);
public sealed record FgsAssetModelListFilters(string? ModelNumber = null, long? AssetTypeId = null, long? AssetManufacturerId = null);
""")
    write(f"Fgs.Asset.Application/Features/{f}/Validators/{e}Validators.cs", """using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Features.AssetModels.Commands.CreateFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Commands.PatchFgsAssetModel;
using Fgs.Asset.Application.Features.AssetModels.Commands.UpdateFgsAssetModel;
using FluentValidation;
namespace Fgs.Asset.Application.Features.AssetModels.Validators;
public sealed class CreateFgsAssetModelCommandValidator : AbstractValidator<CreateFgsAssetModelCommand>
{
    public CreateFgsAssetModelCommandValidator(IFgsAssetModelReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetManufacturerId).GreaterThan(0);
        RuleFor(x => x.Dto.ModelNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.ModelDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (c, v, ct) => await readRepository.ExistsAssetTypeIdAsync(v, ct)).WithMessage("The specified asset type was not found.");
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (c, v, ct) => await readRepository.ExistsAssetManufacturerIdAsync(v, ct)).WithMessage("The specified asset manufacturer was not found.");
    }
}
public sealed class UpdateFgsAssetModelCommandValidator : AbstractValidator<UpdateFgsAssetModelCommand>
{
    public UpdateFgsAssetModelCommandValidator(IFgsAssetModelReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetManufacturerId).GreaterThan(0);
        RuleFor(x => x.Dto.ModelNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.ModelDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (c, v, ct) => await readRepository.ExistsAssetTypeIdAsync(v, ct)).WithMessage("The specified asset type was not found.");
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (c, v, ct) => await readRepository.ExistsAssetManufacturerIdAsync(v, ct)).WithMessage("The specified asset manufacturer was not found.");
    }
}
public sealed class PatchFgsAssetModelCommandValidator : AbstractValidator<PatchFgsAssetModelCommand>
{
    public PatchFgsAssetModelCommandValidator(IFgsAssetModelReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0).When(x => x.Dto.AssetTypeId.HasValue);
        RuleFor(x => x.Dto.AssetManufacturerId).GreaterThan(0).When(x => x.Dto.AssetManufacturerId.HasValue);
        RuleFor(x => x.Dto.ModelNumber).NotEmpty().MaximumLength(100).When(x => x.Dto.ModelNumber is not null);
        RuleFor(x => x.Dto.ModelDescription).NotEmpty().MaximumLength(500).When(x => x.Dto.ModelDescription is not null);
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (c, v, ct) => !v.HasValue || await readRepository.ExistsAssetTypeIdAsync(v.Value, ct)).When(x => x.Dto.AssetTypeId.HasValue);
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (c, v, ct) => !v.HasValue || await readRepository.ExistsAssetManufacturerIdAsync(v.Value, ct)).When(x => x.Dto.AssetManufacturerId.HasValue);
    }
}
""")
    gen_cqc(e, f, r, "Asset Model", "AssetModelController",
            "        [FromQuery] string? modelNumber = null,\n        [FromQuery] long? assetTypeId = null,\n        [FromQuery] long? assetManufacturerId = null,\n",
            "new FgsAssetModelListFilters(modelNumber, assetTypeId, assetManufacturerId)")
    cols = '\\"Id\\", \\"AssetTypeId\\", \\"AssetManufacturerId\\", \\"ModelNumber\\", \\"ModelDescription\\", \\"IsActive\\"'
    _write_infra_model(write, e, f, cols)


def _write_infra_model(write, e, f, cols):
    write(f"Fgs.Asset.Infrastructure/{f}/{e}Sql.cs", f"""using Fgs.Foundation.Paging;
namespace Fgs.Asset.Infrastructure.{f};
internal static class {e}Sql
{{
    public const string Table = "asset.\\"FgsAssetModel\\"";
    public const string SelectDetailColumns = "{cols}";
    public const string SelectSummaryColumns = SelectDetailColumns;
    public const string SelectLookupColumns = "\\"Id\\", \\"ModelNumber\\", \\"ModelDescription\\"";
    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase) {{ "Id", "AssetTypeId", "AssetManufacturerId", "ModelNumber", "ModelDescription", "IsActive" }};
    public static string ResolveOrderBy(string? sortBy, SortDirection direction) {{ var dir = direction == SortDirection.Desc ? "DESC" : "ASC"; if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy)) return $"ORDER BY \\"Id\\" {{dir}}"; var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase)); return $"ORDER BY \\"{{column}}\\" {{dir}}"; }}
}}
""")
    write(f"Fgs.Asset.Infrastructure/{f}/{e}DapperRows.cs", """using Fgs.Asset.Application.Features.AssetModels.Dtos;
namespace Fgs.Asset.Infrastructure.AssetModels;
internal class FgsAssetModelSummaryRow { public long Id { get; set; } public long AssetTypeId { get; set; } public long AssetManufacturerId { get; set; } public string ModelNumber { get; set; } = null!; public string ModelDescription { get; set; } = null!; public bool IsActive { get; set; } public FgsAssetModelSummaryDto ToDto() => new(Id, AssetTypeId, AssetManufacturerId, ModelNumber, ModelDescription, IsActive); }
internal sealed class FgsAssetModelDetailRow : FgsAssetModelSummaryRow { public new FgsAssetModelDetailDto ToDto() => new(Id, AssetTypeId, AssetManufacturerId, ModelNumber, ModelDescription, IsActive); }
internal sealed class FgsAssetModelLookupRow { public long Id { get; set; } public string ModelNumber { get; set; } = null!; public string ModelDescription { get; set; } = null!; public FgsAssetModelLookupDto ToDto() => new(Id, ModelNumber, ModelDescription); }
""")
    write(f"Fgs.Asset.Infrastructure/{f}/{e}ReadRepository.cs", _read_repo_template(e, f, cols, FK_TYPE + FK_MFR, """
        if (!string.IsNullOrWhiteSpace(filters.ModelNumber)) where.Add("\\"ModelNumber\\" ILIKE @ModelNumber");
        if (filters.AssetTypeId.HasValue) where.Add("\\"AssetTypeId\\" = @AssetTypeId");
        if (filters.AssetManufacturerId.HasValue) where.Add("\\"AssetManufacturerId\\" = @AssetManufacturerId");
""", "ModelNumber = string.IsNullOrWhiteSpace(filters.ModelNumber) ? null : $\"%{filters.ModelNumber.Trim()}%\", AssetTypeId = filters.AssetTypeId, AssetManufacturerId = filters.AssetManufacturerId,", '\\"ModelNumber\\" ILIKE @Search OR \\"ModelDescription\\" ILIKE @Search', '"Name"', 'ModelNumber'))
    write(f"Fgs.Asset.Infrastructure/{f}/{e}WriteService.cs", """using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Features.AssetModels.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Fgs.Asset.Infrastructure.AssetModels;
public sealed class FgsAssetModelWriteService : IFgsAssetModelWriteService
{
    private readonly FgsAssetDbContext _context; private readonly IUnitOfWork _unitOfWork; private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetModelWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper) { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }
    public async Task<FgsAssetModelDetailDto> CreateAsync(FgsAssetModelCreateDto dto, CancellationToken cancellationToken = default) { var entity = new FgsAssetModel { AssetTypeId = dto.AssetTypeId, AssetManufacturerId = dto.AssetManufacturerId, ModelNumber = dto.ModelNumber.Trim(), ModelDescription = dto.ModelDescription.Trim() }; _auditHelper.StampForCreate(entity); await _context.FgsAssetModels.AddAsync(entity, cancellationToken); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetModelDetailDto> UpdateAsync(long id, FgsAssetModelUpdateDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Model '{id}' was not found."); entity.AssetTypeId = dto.AssetTypeId; entity.AssetManufacturerId = dto.AssetManufacturerId; entity.ModelNumber = dto.ModelNumber.Trim(); entity.ModelDescription = dto.ModelDescription.Trim(); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetModelDetailDto> PatchAsync(long id, FgsAssetModelPatchDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Model '{id}' was not found."); if (dto.AssetTypeId.HasValue) entity.AssetTypeId = dto.AssetTypeId.Value; if (dto.AssetManufacturerId.HasValue) entity.AssetManufacturerId = dto.AssetManufacturerId.Value; if (dto.ModelNumber is not null) entity.ModelNumber = dto.ModelNumber.Trim(); if (dto.ModelDescription is not null) entity.ModelDescription = dto.ModelDescription.Trim(); if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value; _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    private async Task<FgsAssetModel?> Find(long id, CancellationToken ct) => await _context.FgsAssetModels.FirstOrDefaultAsync(x => x.Id == id, ct);
    private static FgsAssetModelDetailDto Map(FgsAssetModel e) => new(e.Id, e.AssetTypeId, e.AssetManufacturerId, e.ModelNumber, e.ModelDescription, e.IsActive);
}
""")


def _read_repo_template(e, f, cols, fk_methods, filter_where, filter_params, search_clause, order_col, lookup_order="Id"):
    return f"""using Dapper;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Application.Abstractions.{f};
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
namespace Fgs.Asset.Infrastructure.{f};
internal sealed class {e}ReadRepository : I{e}ReadRepository
{{
    private readonly IAssetReadConnectionFactory _connectionFactory;
    private readonly ITenantContextAccessor _tenantContextAccessor;
    public {e}ReadRepository(IAssetReadConnectionFactory connectionFactory, ITenantContextAccessor tenantContextAccessor) {{ _connectionFactory = connectionFactory; _tenantContextAccessor = tenantContextAccessor; }}
    public async Task<{e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default) {{ var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var sql = $"SELECT {{{e}Sql.SelectDetailColumns}} FROM {{{e}Sql.Table}} WHERE \\"Id\\" = @Id AND \\"TenantId\\" = @TenantId AND \\"CompanyId\\" = @CompanyId"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryFirstOrDefaultAsync<{e}DetailRow>(new CommandDefinition(sql, new {{ Id = id, TenantId = tenantId, CompanyId = companyId }}, cancellationToken: cancellationToken)))?.ToDto(); }}
    public async Task<PagedResult<{e}SummaryDto>> ListAsync(AssetListQuery query, {e}ListFilters filters, CancellationToken cancellationToken = default) {{ var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var paging = query.ToPagedQuery(); var page = Math.Max(1, paging.Page); var pageSize = Math.Clamp(paging.PageSize, 1, 200); var offset = (page - 1) * pageSize; var where = new List<string> {{ "\\"TenantId\\" = @TenantId", "\\"CompanyId\\" = @CompanyId" }}; if (paging.IsActive.HasValue) where.Add("\\"IsActive\\" = @IsActive"); {filter_where} if (!string.IsNullOrWhiteSpace(paging.Search)) where.Add("({search_clause})"); var whereClause = string.Join(" AND ", where); var sql = $"SELECT {{{e}Sql.SelectSummaryColumns}} FROM {{{e}Sql.Table}} WHERE {{whereClause}} {{{e}Sql.ResolveOrderBy(paging.SortBy, paging.SortDirection)}} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {{{e}Sql.Table}} WHERE {{whereClause}};"; var parameters = new {{ TenantId = tenantId, CompanyId = companyId, IsActive = paging.IsActive, {filter_params} Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{{paging.Search.Trim()}}%", PageSize = pageSize, Offset = offset }}; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken)); var rows = (await multi.ReadAsync<{e}SummaryRow>()).ToList(); return new PagedResult<{e}SummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, await multi.ReadSingleAsync<int>()); }}
    public async Task<IReadOnlyList<{e}LookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default) {{ var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var activeFilter = activeOnly ? "AND \\"IsActive\\" = TRUE" : string.Empty; var sql = $"SELECT {{{e}Sql.SelectLookupColumns}} FROM {{{e}Sql.Table}} WHERE \\"TenantId\\" = @TenantId AND \\"CompanyId\\" = @CompanyId {{activeFilter}} ORDER BY \\"{lookup_order}\\" ASC"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryAsync<{e}LookupRow>(new CommandDefinition(sql, new {{ TenantId = tenantId, CompanyId = companyId }}, cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList(); }}
{fk_methods}
}}
"""


def gen_all_remaining(write, gen_cqc, gen_tests, created):
    gen_asset_model(write, gen_cqc, gen_tests)
    _gen_from_template_file(write, gen_cqc, gen_tests, "asset_attribute")
    _gen_from_template_file(write, gen_cqc, gen_tests, "asset_attribute_option")
    _gen_from_template_file(write, gen_cqc, gen_tests, "fgs_asset")
    _gen_from_template_file(write, gen_cqc, gen_tests, "asset_warranty")


def _gen_from_template_file(write, gen_cqc, gen_tests, name):
    import os
    path = os.path.join(os.path.dirname(__file__), "templates", f"{name}.py")
    if not os.path.exists(path):
        return
    ns = {"write": write, "gen_cqc": gen_cqc, "gen_tests": gen_tests, "FK_TYPE": FK_TYPE, "FK_MFR": FK_MFR, "FK_STATUS": FK_STATUS, "FK_MODEL": FK_MODEL, "FK_SVC_LOC": FK_SVC_LOC, "FK_ASSET": FK_ASSET, "FK_ATTR": FK_ATTR, "_read_repo_template": _read_repo_template}
    with open(path, encoding="utf-8") as f:
        exec(f.read(), ns)


def update_dependency_injection(write):
    write("Fgs.Asset.Infrastructure/DependencyInjection.cs", """using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Abstractions.AssetManufacturers;
using Fgs.Asset.Application.Abstractions.AssetModels;
using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Abstractions.AssetStatuses;
using Fgs.Asset.Application.Abstractions.AssetTypes;
using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Application.Abstractions.Time;
using Fgs.Asset.Infrastructure.AssetAttributeOptions;
using Fgs.Asset.Infrastructure.AssetAttributes;
using Fgs.Asset.Infrastructure.AssetManufacturers;
using Fgs.Asset.Infrastructure.AssetModels;
using Fgs.Asset.Infrastructure.Assets;
using Fgs.Asset.Infrastructure.AssetStatuses;
using Fgs.Asset.Infrastructure.AssetTypes;
using Fgs.Asset.Infrastructure.AssetWarranties;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Common.Time;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Asset.Infrastructure.Database.Read;
using Fgs.Credentials;
using Fgs.Credentials.Abstractions;
using Fgs.Credentials.Extensions;
using Fgs.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fgs.Asset.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFgsAssetInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddFgsStandardInfrastructure(configuration, "fgs-asset-service", "DATABASE");
        services.AddDbContext<FgsAssetDbContext>((sp, options) =>
        {
            var connectionString = ConnectionStringResolver.ResolveRequired(sp.GetRequiredService<IConfiguration>(), ConnectionStringNames.FgsAsset, "FGS_ASSET_DB", sp.GetService<ICredentialConfigurationProvider>());
            options.UseFgsNpgsql(connectionString, "__EFMigrationsHistory", FgsAssetDbContext.MigrationHistorySchema);
        });
        services.AddFgsPersistence<FgsAssetDbContext>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IAssetReadConnectionFactory, FgsAssetReadConnectionFactory>();
        services.AddScoped<AssetEntityAuditHelper>();
        services.AddScoped<IFgsAssetTypeReadRepository, FgsAssetTypeReadRepository>();
        services.AddScoped<IFgsAssetTypeWriteService, FgsAssetTypeWriteService>();
        services.AddScoped<IFgsAssetManufacturerReadRepository, FgsAssetManufacturerReadRepository>();
        services.AddScoped<IFgsAssetManufacturerWriteService, FgsAssetManufacturerWriteService>();
        services.AddScoped<IFgsAssetStatusReadRepository, FgsAssetStatusReadRepository>();
        services.AddScoped<IFgsAssetStatusWriteService, FgsAssetStatusWriteService>();
        services.AddScoped<IFgsAssetModelReadRepository, FgsAssetModelReadRepository>();
        services.AddScoped<IFgsAssetModelWriteService, FgsAssetModelWriteService>();
        services.AddScoped<IFgsAssetAttributeReadRepository, FgsAssetAttributeReadRepository>();
        services.AddScoped<IFgsAssetAttributeWriteService, FgsAssetAttributeWriteService>();
        services.AddScoped<IFgsAssetAttributeOptionReadRepository, FgsAssetAttributeOptionReadRepository>();
        services.AddScoped<IFgsAssetAttributeOptionWriteService, FgsAssetAttributeOptionWriteService>();
        services.AddScoped<IFgsAssetReadRepository, FgsAssetReadRepository>();
        services.AddScoped<IFgsAssetWriteService, FgsAssetWriteService>();
        services.AddScoped<IFgsAssetWarrantyReadRepository, FgsAssetWarrantyReadRepository>();
        services.AddScoped<IFgsAssetWarrantyWriteService, FgsAssetWarrantyWriteService>();
        return services;
    }
}
""")


def update_gateway(gateway_root, created):
    import os
    routes = "assettype|assetmanufacturer|assetstatus|assetmodel|assetattribute|assetattributeoption|asset|assetwarranty"
    for fname, is_prod in [("conf.d/includes/upstreams.conf", False), ("conf.d/includes/upstreams.prod.conf", True)]:
        path = os.path.join(gateway_root, fname.replace("/", os.sep))
        with open(path, encoding="utf-8") as f:
            text = f.read()
        if "asset_service" not in text:
            block = "upstream asset_service {\n    least_conn;\n"
            if is_prod:
                block += "    zone asset_service 128k;\n"
            block += "    server asset-service:5015 max_fails=3 fail_timeout=10s;\n    keepalive " + ("128" if is_prod else "32") + ";\n}\n"
            with open(path, "w", encoding="utf-8", newline="\n") as f:
                f.write(text.rstrip() + "\n" + block)
    for fname in ["conf.d/includes/api-v1-routes.conf", "conf.d/includes/api-v1-routes.prod.conf"]:
        path = os.path.join(gateway_root, fname.replace("/", os.sep))
        with open(path, encoding="utf-8") as f:
            text = f.read()
        if "asset_service" not in text:
            block = f"\n# Asset Service — Fgs.Asset.API\nlocation ~ ^/api/v1/({routes})(/|$) {{\n    include /etc/nginx/conf.d/includes/rate-limit.inc;\n    proxy_pass http://asset_service;\n    proxy_set_header X-Forwarded-Prefix /api/v1/$1;\n    include /etc/nginx/proxy_params.conf;\n    include /etc/nginx/cache_params.conf;\n}}\n"
            with open(path, "w", encoding="utf-8", newline="\n") as f:
                f.write(text.rstrip() + block)
