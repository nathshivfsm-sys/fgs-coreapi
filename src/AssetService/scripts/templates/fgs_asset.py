e, f, r = "FgsAsset", "Assets", "asset"
cols = '\\"Id\\", \\"AssetGuid\\", \\"AssetNumber\\", \\"ServiceLocationId\\", \\"AssetTypeId\\", \\"AssetManufacturerId\\", \\"AssetModelId\\", \\"AssetDescription\\", \\"CustomerAssetNumber\\", \\"CustomerAssetName\\", \\"ManufacturerName\\", \\"ModelNumber\\", \\"SerialNumber\\", \\"ManufactureDate\\", \\"InstallDate\\", \\"InstalledWorkOrderId\\", \\"IsInstalledByCompany\\", \\"AssetStatusId\\", \\"IsActive\\"'

write(f"Fgs.Asset.Application/Abstractions/{f}/I{e}ReadRepository.cs", f"""using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.{f};
public interface I{e}ReadRepository {{
    Task<{e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<{e}SummaryDto>> ListAsync(AssetListQuery query, {e}ListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<{e}LookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByAssetNumberAsync(string assetNumber, long? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetTypeIdAsync(long? assetTypeId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetManufacturerIdAsync(long? assetManufacturerId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetModelIdAsync(long? assetModelId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetStatusIdAsync(long assetStatusId, CancellationToken cancellationToken = default);
    Task<bool> ExistsServiceLocationIdAsync(long serviceLocationId, CancellationToken cancellationToken = default);
}}
""")
write(f"Fgs.Asset.Application/Abstractions/{f}/I{e}WriteService.cs", f"""using Fgs.Asset.Application.Features.{f}.Dtos;
namespace Fgs.Asset.Application.Abstractions.{f};
public interface I{e}WriteService {{
    Task<{e}DetailDto> CreateAsync({e}CreateDto dto, CancellationToken cancellationToken = default);
    Task<{e}DetailDto> UpdateAsync(long id, {e}UpdateDto dto, CancellationToken cancellationToken = default);
    Task<{e}DetailDto> PatchAsync(long id, {e}PatchDto dto, CancellationToken cancellationToken = default);
}}
""")
write(f"Fgs.Asset.Application/Features/{f}/Dtos/{e}Dtos.cs", """namespace Fgs.Asset.Application.Features.Assets.Dtos;
public sealed record FgsAssetSummaryDto(long Id, Guid AssetGuid, string AssetNumber, long ServiceLocationId, long? AssetTypeId, long? AssetManufacturerId, long? AssetModelId, string? AssetDescription, string? CustomerAssetNumber, string? CustomerAssetName, string? ManufacturerName, string? ModelNumber, string? SerialNumber, DateOnly? ManufactureDate, DateOnly? InstallDate, long? InstalledWorkOrderId, bool IsInstalledByCompany, long AssetStatusId, bool IsActive);
public sealed record FgsAssetDetailDto(long Id, Guid AssetGuid, string AssetNumber, long ServiceLocationId, long? AssetTypeId, long? AssetManufacturerId, long? AssetModelId, string? AssetDescription, string? CustomerAssetNumber, string? CustomerAssetName, string? ManufacturerName, string? ModelNumber, string? SerialNumber, DateOnly? ManufactureDate, DateOnly? InstallDate, long? InstalledWorkOrderId, bool IsInstalledByCompany, long AssetStatusId, bool IsActive);
public sealed record FgsAssetLookupDto(long Id, string AssetNumber, string? CustomerAssetName);
public sealed record FgsAssetCreateDto(string AssetNumber, long ServiceLocationId, long? AssetTypeId, long? AssetManufacturerId, long? AssetModelId, string? AssetDescription, string? CustomerAssetNumber, string? CustomerAssetName, string? ManufacturerName, string? ModelNumber, string? SerialNumber, DateOnly? ManufactureDate, DateOnly? InstallDate, long? InstalledWorkOrderId, bool IsInstalledByCompany, long AssetStatusId);
public sealed record FgsAssetUpdateDto(string AssetNumber, long ServiceLocationId, long? AssetTypeId, long? AssetManufacturerId, long? AssetModelId, string? AssetDescription, string? CustomerAssetNumber, string? CustomerAssetName, string? ManufacturerName, string? ModelNumber, string? SerialNumber, DateOnly? ManufactureDate, DateOnly? InstallDate, long? InstalledWorkOrderId, bool IsInstalledByCompany, long AssetStatusId);
public sealed record FgsAssetPatchDto(string? AssetNumber, long? ServiceLocationId, long? AssetTypeId, long? AssetManufacturerId, long? AssetModelId, string? AssetDescription, string? CustomerAssetNumber, string? CustomerAssetName, string? ManufacturerName, string? ModelNumber, string? SerialNumber, DateOnly? ManufactureDate, DateOnly? InstallDate, long? InstalledWorkOrderId, bool? IsInstalledByCompany, long? AssetStatusId, bool? IsActive);
public sealed record FgsAssetListFilters(string? AssetNumber = null, long? ServiceLocationId = null, long? AssetStatusId = null);
""")
write(f"Fgs.Asset.Application/Features/{f}/Validators/{e}Validators.cs", """using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Features.Assets.Commands.CreateFgsAsset;
using Fgs.Asset.Application.Features.Assets.Commands.PatchFgsAsset;
using Fgs.Asset.Application.Features.Assets.Commands.UpdateFgsAsset;
using FluentValidation;
namespace Fgs.Asset.Application.Features.Assets.Validators;
public sealed class CreateFgsAssetCommandValidator : AbstractValidator<CreateFgsAssetCommand>
{
    public CreateFgsAssetCommandValidator(IFgsAssetReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.AssetNumber).MustAsync(async (cmd, n, ct) => !await readRepository.ExistsByAssetNumberAsync(n, null, ct));
        RuleFor(x => x.Dto.ServiceLocationId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetStatusId).GreaterThan(0);
        RuleFor(x => x.Dto.ServiceLocationId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsServiceLocationIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetStatusId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetStatusIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetManufacturerIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetModelId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetModelIdAsync(id, ct));
    }
}
public sealed class UpdateFgsAssetCommandValidator : AbstractValidator<UpdateFgsAssetCommand>
{
    public UpdateFgsAssetCommandValidator(IFgsAssetReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.AssetNumber).MustAsync(async (cmd, n, ct) => !await readRepository.ExistsByAssetNumberAsync(n, cmd.Id, ct));
        RuleFor(x => x.Dto.ServiceLocationId).GreaterThan(0);
        RuleFor(x => x.Dto.AssetStatusId).GreaterThan(0);
        RuleFor(x => x.Dto.ServiceLocationId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsServiceLocationIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetStatusId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetStatusIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetManufacturerId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetManufacturerIdAsync(id, ct));
        RuleFor(x => x.Dto.AssetModelId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetModelIdAsync(id, ct));
    }
}
public sealed class PatchFgsAssetCommandValidator : AbstractValidator<PatchFgsAssetCommand>
{
    public PatchFgsAssetCommandValidator(IFgsAssetReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetNumber).NotEmpty().MaximumLength(100).When(x => x.Dto.AssetNumber is not null);
        RuleFor(x => x.Dto.ServiceLocationId).GreaterThan(0).When(x => x.Dto.ServiceLocationId.HasValue);
        RuleFor(x => x.Dto.AssetStatusId).GreaterThan(0).When(x => x.Dto.AssetStatusId.HasValue);
    }
}
""")
gen_cqc(e, f, r, "Asset", "AssetController",
        "        [FromQuery] string? assetNumber = null,\n        [FromQuery] long? serviceLocationId = null,\n        [FromQuery] long? assetStatusId = null,\n",
        "new FgsAssetListFilters(assetNumber, serviceLocationId, assetStatusId)")
write(f"Fgs.Asset.Infrastructure/{f}/{e}Sql.cs", f"""using Fgs.Foundation.Paging;
namespace Fgs.Asset.Infrastructure.{f};
internal static class {e}Sql {{ public const string Table = "asset.\\"FgsAsset\\""; public const string SelectDetailColumns = "{cols}"; public const string SelectSummaryColumns = SelectDetailColumns; public const string SelectLookupColumns = "\\"Id\\", \\"AssetNumber\\", \\"CustomerAssetName\\""; private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase) {{ "Id", "AssetNumber", "ServiceLocationId", "AssetStatusId", "IsActive" }}; public static string ResolveOrderBy(string? sortBy, SortDirection direction) {{ var dir = direction == SortDirection.Desc ? "DESC" : "ASC"; if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy)) return $"ORDER BY \\"Id\\" {{dir}}"; return $"ORDER BY \\"{{AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase))}}\\" {{dir}}"; }} }}
""")
write(f"Fgs.Asset.Infrastructure/{f}/{e}DapperRows.cs", """using Fgs.Asset.Application.Features.Assets.Dtos;
namespace Fgs.Asset.Infrastructure.Assets;
internal class FgsAssetSummaryRow { public long Id { get; set; } public Guid AssetGuid { get; set; } public string AssetNumber { get; set; } = null!; public long ServiceLocationId { get; set; } public long? AssetTypeId { get; set; } public long? AssetManufacturerId { get; set; } public long? AssetModelId { get; set; } public string? AssetDescription { get; set; } public string? CustomerAssetNumber { get; set; } public string? CustomerAssetName { get; set; } public string? ManufacturerName { get; set; } public string? ModelNumber { get; set; } public string? SerialNumber { get; set; } public DateOnly? ManufactureDate { get; set; } public DateOnly? InstallDate { get; set; } public long? InstalledWorkOrderId { get; set; } public bool IsInstalledByCompany { get; set; } public long AssetStatusId { get; set; } public bool IsActive { get; set; } public FgsAssetSummaryDto ToDto() => new(Id, AssetGuid, AssetNumber, ServiceLocationId, AssetTypeId, AssetManufacturerId, AssetModelId, AssetDescription, CustomerAssetNumber, CustomerAssetName, ManufacturerName, ModelNumber, SerialNumber, ManufactureDate, InstallDate, InstalledWorkOrderId, IsInstalledByCompany, AssetStatusId, IsActive); }
internal sealed class FgsAssetDetailRow : FgsAssetSummaryRow { public new FgsAssetDetailDto ToDto() => new(Id, AssetGuid, AssetNumber, ServiceLocationId, AssetTypeId, AssetManufacturerId, AssetModelId, AssetDescription, CustomerAssetNumber, CustomerAssetName, ManufacturerName, ModelNumber, SerialNumber, ManufactureDate, InstallDate, InstalledWorkOrderId, IsInstalledByCompany, AssetStatusId, IsActive); }
internal sealed class FgsAssetLookupRow { public long Id { get; set; } public string AssetNumber { get; set; } = null!; public string? CustomerAssetName { get; set; } public FgsAssetLookupDto ToDto() => new(Id, AssetNumber, CustomerAssetName); }
""")
fk = """
    public async Task<bool> ExistsByAssetNumberAsync(string assetNumber, long? excludeId = null, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); var exclude = excludeId.HasValue ? "AND \\"Id\\" <> @ExcludeId" : string.Empty; var sql = $"SELECT EXISTS(SELECT 1 FROM {FgsAssetSql.Table} WHERE \\"TenantId\\" = @TenantId AND \\"CompanyId\\" = @CompanyId AND \\"AssetNumber\\" = @AssetNumber {exclude})"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, AssetNumber = assetNumber.Trim(), ExcludeId = excludeId }, cancellationToken: cancellationToken)); }
    public async Task<bool> ExistsAssetTypeIdAsync(long? assetTypeId, CancellationToken cancellationToken = default) { if (!assetTypeId.HasValue) return true; var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAssetType\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id AND \\\"IsActive\\\" = TRUE)"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetTypeId.Value }, cancellationToken: cancellationToken)); }
    public async Task<bool> ExistsAssetManufacturerIdAsync(long? assetManufacturerId, CancellationToken cancellationToken = default) { if (!assetManufacturerId.HasValue) return true; var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAssetManufacturer\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id AND \\\"IsActive\\\" = TRUE)"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetManufacturerId.Value }, cancellationToken: cancellationToken)); }
    public async Task<bool> ExistsAssetModelIdAsync(long? assetModelId, CancellationToken cancellationToken = default) { if (!assetModelId.HasValue) return true; var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAssetModel\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id AND \\\"IsActive\\\" = TRUE)"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetModelId.Value }, cancellationToken: cancellationToken)); }
    public async Task<bool> ExistsAssetStatusIdAsync(long assetStatusId, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsAssetStatus\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"Id\\\" = @Id AND \\\"IsActive\\\" = TRUE)"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, Id = assetStatusId }, cancellationToken: cancellationToken)); }
    public async Task<bool> ExistsServiceLocationIdAsync(long serviceLocationId, CancellationToken cancellationToken = default) { var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor); const string sql = "SELECT EXISTS(SELECT 1 FROM asset.\\\"FgsServiceLocationCache\\\" WHERE \\\"TenantId\\\" = @TenantId AND \\\"CompanyId\\\" = @CompanyId AND \\\"ServiceLocationId\\\" = @ServiceLocationId)"; await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken); return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, ServiceLocationId = serviceLocationId }, cancellationToken: cancellationToken)); }
"""
write(f"Fgs.Asset.Infrastructure/{f}/{e}ReadRepository.cs", _read_repo_template(e, f, cols, fk, """
        if (!string.IsNullOrWhiteSpace(filters.AssetNumber)) where.Add("\\"AssetNumber\\" ILIKE @AssetNumber");
        if (filters.ServiceLocationId.HasValue) where.Add("\\"ServiceLocationId\\" = @ServiceLocationId");
        if (filters.AssetStatusId.HasValue) where.Add("\\"AssetStatusId\\" = @AssetStatusId");
""", "AssetNumber = string.IsNullOrWhiteSpace(filters.AssetNumber) ? null : $\"%{filters.AssetNumber.Trim()}%\", ServiceLocationId = filters.ServiceLocationId, AssetStatusId = filters.AssetStatusId,", '\\"AssetNumber\\" ILIKE @Search OR \\"CustomerAssetName\\" ILIKE @Search OR \\"SerialNumber\\" ILIKE @Search', "AssetNumber"))
write(f"Fgs.Asset.Infrastructure/{f}/{e}WriteService.cs", """using Fgs.Asset.Application.Abstractions.Assets;
using Fgs.Asset.Application.Features.Assets.Dtos;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Fgs.Asset.Infrastructure.Assets;
public sealed class FgsAssetWriteService : IFgsAssetWriteService
{
    private readonly FgsAssetDbContext _context; private readonly IUnitOfWork _unitOfWork; private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper) { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }
    public async Task<FgsAssetDetailDto> CreateAsync(FgsAssetCreateDto dto, CancellationToken cancellationToken = default) { var entity = new Domain.Entities.FgsAsset { AssetGuid = Guid.NewGuid(), AssetNumber = dto.AssetNumber.Trim(), ServiceLocationId = dto.ServiceLocationId, AssetTypeId = dto.AssetTypeId, AssetManufacturerId = dto.AssetManufacturerId, AssetModelId = dto.AssetModelId, AssetDescription = Trim(dto.AssetDescription), CustomerAssetNumber = Trim(dto.CustomerAssetNumber), CustomerAssetName = Trim(dto.CustomerAssetName), ManufacturerName = Trim(dto.ManufacturerName), ModelNumber = Trim(dto.ModelNumber), SerialNumber = Trim(dto.SerialNumber), ManufactureDate = dto.ManufactureDate, InstallDate = dto.InstallDate, InstalledWorkOrderId = dto.InstalledWorkOrderId, IsInstalledByCompany = dto.IsInstalledByCompany, AssetStatusId = dto.AssetStatusId }; _auditHelper.StampForCreate(entity); await _context.FgsAssets.AddAsync(entity, cancellationToken); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetDetailDto> UpdateAsync(long id, FgsAssetUpdateDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset '{id}' was not found."); Apply(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetDetailDto> PatchAsync(long id, FgsAssetPatchDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset '{id}' was not found."); Patch(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    private async Task<Domain.Entities.FgsAsset?> Find(long id, CancellationToken ct) => await _context.FgsAssets.FirstOrDefaultAsync(x => x.Id == id, ct);
    private static void Apply(Domain.Entities.FgsAsset e, FgsAssetUpdateDto dto) { e.AssetNumber = dto.AssetNumber.Trim(); e.ServiceLocationId = dto.ServiceLocationId; e.AssetTypeId = dto.AssetTypeId; e.AssetManufacturerId = dto.AssetManufacturerId; e.AssetModelId = dto.AssetModelId; e.AssetDescription = Trim(dto.AssetDescription); e.CustomerAssetNumber = Trim(dto.CustomerAssetNumber); e.CustomerAssetName = Trim(dto.CustomerAssetName); e.ManufacturerName = Trim(dto.ManufacturerName); e.ModelNumber = Trim(dto.ModelNumber); e.SerialNumber = Trim(dto.SerialNumber); e.ManufactureDate = dto.ManufactureDate; e.InstallDate = dto.InstallDate; e.InstalledWorkOrderId = dto.InstalledWorkOrderId; e.IsInstalledByCompany = dto.IsInstalledByCompany; e.AssetStatusId = dto.AssetStatusId; }
    private static void Patch(Domain.Entities.FgsAsset e, FgsAssetPatchDto dto) { if (dto.AssetNumber is not null) e.AssetNumber = dto.AssetNumber.Trim(); if (dto.ServiceLocationId.HasValue) e.ServiceLocationId = dto.ServiceLocationId.Value; if (dto.AssetTypeId.HasValue) e.AssetTypeId = dto.AssetTypeId; if (dto.AssetManufacturerId.HasValue) e.AssetManufacturerId = dto.AssetManufacturerId; if (dto.AssetModelId.HasValue) e.AssetModelId = dto.AssetModelId; if (dto.AssetDescription is not null) e.AssetDescription = Trim(dto.AssetDescription); if (dto.CustomerAssetNumber is not null) e.CustomerAssetNumber = Trim(dto.CustomerAssetNumber); if (dto.CustomerAssetName is not null) e.CustomerAssetName = Trim(dto.CustomerAssetName); if (dto.ManufacturerName is not null) e.ManufacturerName = Trim(dto.ManufacturerName); if (dto.ModelNumber is not null) e.ModelNumber = Trim(dto.ModelNumber); if (dto.SerialNumber is not null) e.SerialNumber = Trim(dto.SerialNumber); if (dto.ManufactureDate.HasValue) e.ManufactureDate = dto.ManufactureDate; if (dto.InstallDate.HasValue) e.InstallDate = dto.InstallDate; if (dto.InstalledWorkOrderId.HasValue) e.InstalledWorkOrderId = dto.InstalledWorkOrderId; if (dto.IsInstalledByCompany.HasValue) e.IsInstalledByCompany = dto.IsInstalledByCompany.Value; if (dto.AssetStatusId.HasValue) e.AssetStatusId = dto.AssetStatusId.Value; if (dto.IsActive.HasValue) e.IsActive = dto.IsActive.Value; }
    private static string? Trim(string? v) => string.IsNullOrWhiteSpace(v) ? null : v.Trim();
    private static FgsAssetDetailDto Map(Domain.Entities.FgsAsset e) => new(e.Id, e.AssetGuid, e.AssetNumber, e.ServiceLocationId, e.AssetTypeId, e.AssetManufacturerId, e.AssetModelId, e.AssetDescription, e.CustomerAssetNumber, e.CustomerAssetName, e.ManufacturerName, e.ModelNumber, e.SerialNumber, e.ManufactureDate, e.InstallDate, e.InstalledWorkOrderId, e.IsInstalledByCompany, e.AssetStatusId, e.IsActive);
}
""")
for t in ["Validator", "CommandHandler", "QueryHandler"]:
    write(f"Fgs.Asset.Tests/{f}/FgsAsset{t}Tests.cs", f"namespace Fgs.Asset.Tests.Assets; public sealed class FgsAsset{t}Tests {{ [Fact] public void Placeholder() => true.Should().BeTrue(); }}")
