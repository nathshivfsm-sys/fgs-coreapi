e, f, r = "FgsAssetWarranty", "AssetWarranties", "assetwarranty"
cols = '\\"Id\\", \\"AssetId\\", \\"WarrantyType\\", \\"WarrantyProvider\\", \\"WarrantyNumber\\", \\"RegistrationNumber\\", \\"StartDate\\", \\"EndDate\\", \\"CoverageDescription\\"'

write(f"Fgs.Asset.Application/Abstractions/{f}/I{e}ReadRepository.cs", f"""using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.{f};
public interface I{e}ReadRepository {{
    Task<{e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<{e}SummaryDto>> ListAsync(AssetListQuery query, {e}ListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<{e}LookupDto>> LookupAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetIdAsync(long assetId, CancellationToken cancellationToken = default);
}}
""")
write(f"Fgs.Asset.Application/Abstractions/{f}/I{e}WriteService.cs", f"""using Fgs.Asset.Application.Features.{f}.Dtos;
namespace Fgs.Asset.Application.Abstractions.{f};
public interface I{e}WriteService {{
    Task<{e}DetailDto> UpdateAsync(long id, {e}UpdateDto dto, CancellationToken cancellationToken = default);
    Task<{e}DetailDto> PatchAsync(long id, {e}PatchDto dto, CancellationToken cancellationToken = default);
}}
""")
write(f"Fgs.Asset.Application/Features/{f}/Dtos/{e}Dtos.cs", """namespace Fgs.Asset.Application.Features.AssetWarranties.Dtos;
public sealed record FgsAssetWarrantySummaryDto(long Id, long AssetId, string WarrantyType, string? WarrantyProvider, string? WarrantyNumber, string? RegistrationNumber, DateOnly StartDate, DateOnly EndDate, string? CoverageDescription);
public sealed record FgsAssetWarrantyDetailDto(long Id, long AssetId, string WarrantyType, string? WarrantyProvider, string? WarrantyNumber, string? RegistrationNumber, DateOnly StartDate, DateOnly EndDate, string? CoverageDescription);
public sealed record FgsAssetWarrantyLookupDto(long Id, string WarrantyType, DateOnly StartDate, DateOnly EndDate);
public sealed record FgsAssetWarrantyUpdateDto(long AssetId, string WarrantyType, string? WarrantyProvider, string? WarrantyNumber, string? RegistrationNumber, DateOnly StartDate, DateOnly EndDate, string? CoverageDescription);
public sealed record FgsAssetWarrantyPatchDto(long? AssetId, string? WarrantyType, string? WarrantyProvider, string? WarrantyNumber, string? RegistrationNumber, DateOnly? StartDate, DateOnly? EndDate, string? CoverageDescription);
public sealed record FgsAssetWarrantyListFilters(long? AssetId = null, string? WarrantyType = null);
""")
write(f"Fgs.Asset.Application/Features/{f}/Validators/{e}Validators.cs", """using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Commands.PatchFgsAssetWarranty;
using Fgs.Asset.Application.Features.AssetWarranties.Commands.UpdateFgsAssetWarranty;
using FluentValidation;
namespace Fgs.Asset.Application.Features.AssetWarranties.Validators;
public sealed class UpdateFgsAssetWarrantyCommandValidator : AbstractValidator<UpdateFgsAssetWarrantyCommand>
{
    public UpdateFgsAssetWarrantyCommandValidator(IFgsAssetWarrantyReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetId).GreaterThan(0);
        RuleFor(x => x.Dto.WarrantyType).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.WarrantyType).Must(t => string.Equals(t, t.Trim().ToUpperInvariant(), StringComparison.Ordinal));
        RuleFor(x => x.Dto.EndDate).GreaterThanOrEqualTo(x => x.Dto.StartDate);
        RuleFor(x => x.Dto.AssetId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetIdAsync(id, ct));
        RuleFor(x => x.Dto.WarrantyProvider).MaximumLength(200);
        RuleFor(x => x.Dto.WarrantyNumber).MaximumLength(100);
        RuleFor(x => x.Dto.RegistrationNumber).MaximumLength(100);
        RuleFor(x => x.Dto.CoverageDescription).MaximumLength(1000);
    }
}
public sealed class PatchFgsAssetWarrantyCommandValidator : AbstractValidator<PatchFgsAssetWarrantyCommand>
{
    public PatchFgsAssetWarrantyCommandValidator(IFgsAssetWarrantyReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetId).GreaterThan(0).When(x => x.Dto.AssetId.HasValue);
        RuleFor(x => x.Dto.WarrantyType).NotEmpty().MaximumLength(75).When(x => x.Dto.WarrantyType is not null);
        RuleFor(x => x.Dto.AssetId).MustAsync(async (cmd, id, ct) => !id.HasValue || await readRepository.ExistsAssetIdAsync(id.Value, ct)).When(x => x.Dto.AssetId.HasValue);
    }
}
""")
gen_cqc(e, f, r, "Asset Warranty", "AssetWarrantyController",
        "        [FromQuery] long? assetId = null,\n        [FromQuery] string? warrantyType = null,\n",
        "new FgsAssetWarrantyListFilters(assetId, warrantyType)",
        has_post=False, has_is_active=False, lookup_active=False)
write(f"Fgs.Asset.Infrastructure/{f}/{e}Sql.cs", f"""using Fgs.Foundation.Paging;
namespace Fgs.Asset.Infrastructure.{f};
internal static class {e}Sql {{ public const string Table = "asset.\\"FgsAssetWarranty\\""; public const string SelectDetailColumns = "{cols}"; public const string SelectSummaryColumns = SelectDetailColumns; public const string SelectLookupColumns = "\\"Id\\", \\"WarrantyType\\", \\"StartDate\\", \\"EndDate\\""; private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase) {{ "Id", "AssetId", "WarrantyType", "StartDate", "EndDate" }}; public static string ResolveOrderBy(string? sortBy, SortDirection direction) {{ var dir = direction == SortDirection.Desc ? "DESC" : "ASC"; if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy)) return $"ORDER BY \\"Id\\" {{dir}}"; return $"ORDER BY \\"{{AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase))}}\\" {{dir}}"; }} }}
""")
write(f"Fgs.Asset.Infrastructure/{f}/{e}DapperRows.cs", """using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
namespace Fgs.Asset.Infrastructure.AssetWarranties;
internal class FgsAssetWarrantySummaryRow { public long Id { get; set; } public long AssetId { get; set; } public string WarrantyType { get; set; } = null!; public string? WarrantyProvider { get; set; } public string? WarrantyNumber { get; set; } public string? RegistrationNumber { get; set; } public DateOnly StartDate { get; set; } public DateOnly EndDate { get; set; } public string? CoverageDescription { get; set; } public FgsAssetWarrantySummaryDto ToDto() => new(Id, AssetId, WarrantyType, WarrantyProvider, WarrantyNumber, RegistrationNumber, StartDate, EndDate, CoverageDescription); }
internal sealed class FgsAssetWarrantyDetailRow : FgsAssetWarrantySummaryRow { public new FgsAssetWarrantyDetailDto ToDto() => new(Id, AssetId, WarrantyType, WarrantyProvider, WarrantyNumber, RegistrationNumber, StartDate, EndDate, CoverageDescription); }
internal sealed class FgsAssetWarrantyLookupRow { public long Id { get; set; } public string WarrantyType { get; set; } = null!; public DateOnly StartDate { get; set; } public DateOnly EndDate { get; set; } public FgsAssetWarrantyLookupDto ToDto() => new(Id, WarrantyType, StartDate, EndDate); }
""")
# Custom read repo without isActive
write(f"Fgs.Asset.Infrastructure/{f}/{e}ReadRepository.cs", r'''using Dapper;
using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Abstractions.Persistence;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Foundation.Paging;
using Fgs.MultiTenancy;
namespace Fgs.Asset.Infrastructure.AssetWarranties;
internal sealed class FgsAssetWarrantyReadRepository : IFgsAssetWarrantyReadRepository
{
    private readonly IAssetReadConnectionFactory _connectionFactory; private readonly ITenantContextAccessor _tenantContextAccessor;
    public FgsAssetWarrantyReadRepository(IAssetReadConnectionFactory connectionFactory, ITenantContextAccessor tenantContextAccessor) { _connectionFactory = connectionFactory; _tenantContextAccessor = tenantContextAccessor; }
    public async Task<FgsAssetWarrantyDetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var sql = $"SELECT {FgsAssetWarrantySql.SelectDetailColumns} FROM {FgsAssetWarrantySql.Table} WHERE \"Id\" = @Id AND \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryFirstOrDefaultAsync<FgsAssetWarrantyDetailRow>(new CommandDefinition(sql, new { Id = id, TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken)))?.ToDto(); }
    public async Task<PagedResult<FgsAssetWarrantySummaryDto>> ListAsync(AssetListQuery query, FgsAssetWarrantyListFilters filters, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var paging = query.ToPagedQuery(); var page = Math.Max(1, paging.Page); var pageSize = Math.Clamp(paging.PageSize, 1, 200); var offset = (page - 1) * pageSize; var where = new List<string> { "\"TenantId\" = @TenantId", "\"CompanyId\" = @CompanyId" }; if (filters.AssetId.HasValue) where.Add("\"AssetId\" = @AssetId"); if (!string.IsNullOrWhiteSpace(filters.WarrantyType)) where.Add("\"WarrantyType\" = @WarrantyType"); if (!string.IsNullOrWhiteSpace(paging.Search)) where.Add("(\"WarrantyType\" ILIKE @Search OR \"WarrantyProvider\" ILIKE @Search)"); var whereClause = string.Join(" AND ", where); var sql = $"SELECT {FgsAssetWarrantySql.SelectSummaryColumns} FROM {FgsAssetWarrantySql.Table} WHERE {whereClause} {FgsAssetWarrantySql.ResolveOrderBy(paging.SortBy, paging.SortDirection)} LIMIT @PageSize OFFSET @Offset; SELECT COUNT(*) FROM {FgsAssetWarrantySql.Table} WHERE {whereClause};"; var parameters = new { TenantId = tenantId, CompanyId = companyId, AssetId = filters.AssetId, WarrantyType = filters.WarrantyType?.Trim().ToUpperInvariant(), Search = string.IsNullOrWhiteSpace(paging.Search) ? null : $"%{paging.Search.Trim()}%", PageSize = pageSize, Offset = offset }; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); await using var multi = await connection.QueryMultipleAsync(new CommandDefinition(sql, parameters, cancellationToken: cancellationToken)); var rows = (await multi.ReadAsync<FgsAssetWarrantySummaryRow>()).ToList(); return new PagedResult<FgsAssetWarrantySummaryDto>(rows.Select(r => r.ToDto()).ToList(), page, pageSize, await multi.ReadSingleAsync<int>()); }
    public async Task<IReadOnlyList<FgsAssetWarrantyLookupDto>> LookupAsync(CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var sql = $"SELECT {FgsAssetWarrantySql.SelectLookupColumns} FROM {FgsAssetWarrantySql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId ORDER BY \"Id\" ASC"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return (await connection.QueryAsync<FgsAssetWarrantyLookupRow>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId }, cancellationToken: cancellationToken))).Select(r => r.ToDto()).ToList(); }
    public async Task<bool> ExistsAssetIdAsync(long assetId, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\"FgsAsset\" WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"Id\" = @Id)"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetId }, cancellationToken: cancellationToken)); }
}
''')
write(f"Fgs.Asset.Infrastructure/{f}/{e}WriteService.cs", """using Fgs.Asset.Application.Abstractions.AssetWarranties;
using Fgs.Asset.Application.Features.AssetWarranties.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Fgs.Asset.Infrastructure.AssetWarranties;
public sealed class FgsAssetWarrantyWriteService : IFgsAssetWarrantyWriteService
{
    private readonly FgsAssetDbContext _context; private readonly IUnitOfWork _unitOfWork; private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetWarrantyWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper) { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }
    public async Task<FgsAssetWarrantyDetailDto> UpdateAsync(long id, FgsAssetWarrantyUpdateDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Warranty '{id}' was not found."); Apply(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetWarrantyDetailDto> PatchAsync(long id, FgsAssetWarrantyPatchDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Warranty '{id}' was not found."); Patch(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    private async Task<FgsAssetWarranty?> Find(long id, CancellationToken ct) => await _context.FgsAssetWarranties.FirstOrDefaultAsync(x => x.Id == id, ct);
    private static void Apply(FgsAssetWarranty e, FgsAssetWarrantyUpdateDto dto) { e.AssetId = dto.AssetId; e.WarrantyType = dto.WarrantyType.Trim().ToUpperInvariant(); e.WarrantyProvider = Trim(dto.WarrantyProvider); e.WarrantyNumber = Trim(dto.WarrantyNumber); e.RegistrationNumber = Trim(dto.RegistrationNumber); e.StartDate = dto.StartDate; e.EndDate = dto.EndDate; e.CoverageDescription = Trim(dto.CoverageDescription); }
    private static void Patch(FgsAssetWarranty e, FgsAssetWarrantyPatchDto dto) { if (dto.AssetId.HasValue) e.AssetId = dto.AssetId.Value; if (dto.WarrantyType is not null) e.WarrantyType = dto.WarrantyType.Trim().ToUpperInvariant(); if (dto.WarrantyProvider is not null) e.WarrantyProvider = Trim(dto.WarrantyProvider); if (dto.WarrantyNumber is not null) e.WarrantyNumber = Trim(dto.WarrantyNumber); if (dto.RegistrationNumber is not null) e.RegistrationNumber = Trim(dto.RegistrationNumber); if (dto.StartDate.HasValue) e.StartDate = dto.StartDate.Value; if (dto.EndDate.HasValue) e.EndDate = dto.EndDate.Value; if (dto.CoverageDescription is not null) e.CoverageDescription = Trim(dto.CoverageDescription); }
    private static string? Trim(string? v) => string.IsNullOrWhiteSpace(v) ? null : v.Trim();
    private static FgsAssetWarrantyDetailDto Map(FgsAssetWarranty e) => new(e.Id, e.AssetId, e.WarrantyType, e.WarrantyProvider, e.WarrantyNumber, e.RegistrationNumber, e.StartDate, e.EndDate, e.CoverageDescription);
}
""")
for t in ["Validator", "CommandHandler", "QueryHandler"]:
    write(f"Fgs.Asset.Tests/{f}/FgsAssetWarranty{t}Tests.cs", f"namespace Fgs.Asset.Tests.AssetWarranties; public sealed class FgsAssetWarranty{t}Tests {{ [Fact] public void Placeholder() => true.Should().BeTrue(); }}")
