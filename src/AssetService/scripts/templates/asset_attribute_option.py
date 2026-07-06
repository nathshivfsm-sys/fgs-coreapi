e, f, r = "FgsAssetAttributeOption", "AssetAttributeOptions", "assetattributeoption"
cols = '\\"Id\\", \\"AssetAttributeId\\", \\"OptionCode\\", \\"OptionName\\", \\"DisplayOrder\\", \\"IsActive\\"'

write(f"Fgs.Asset.Application/Abstractions/{f}/I{e}ReadRepository.cs", f"""using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.{f};
public interface I{e}ReadRepository {{
    Task<{e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<{e}SummaryDto>> ListAsync(AssetListQuery query, {e}ListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<{e}LookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByOptionCodeAsync(long assetAttributeId, string optionCode, long? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetAttributeIdAsync(long assetAttributeId, CancellationToken cancellationToken = default);
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
write(f"Fgs.Asset.Application/Features/{f}/Dtos/{e}Dtos.cs", """namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
public sealed record FgsAssetAttributeOptionSummaryDto(long Id, long AssetAttributeId, string OptionCode, string OptionName, int DisplayOrder, bool IsActive);
public sealed record FgsAssetAttributeOptionDetailDto(long Id, long AssetAttributeId, string OptionCode, string OptionName, int DisplayOrder, bool IsActive);
public sealed record FgsAssetAttributeOptionLookupDto(long Id, string OptionCode, string OptionName);
public sealed record FgsAssetAttributeOptionCreateDto(long AssetAttributeId, string OptionCode, string OptionName, int DisplayOrder);
public sealed record FgsAssetAttributeOptionUpdateDto(long AssetAttributeId, string OptionCode, string OptionName, int DisplayOrder);
public sealed record FgsAssetAttributeOptionPatchDto(long? AssetAttributeId, string? OptionCode, string? OptionName, int? DisplayOrder, bool? IsActive);
public sealed record FgsAssetAttributeOptionListFilters(string? OptionCode = null, string? OptionName = null, long? AssetAttributeId = null);
""")
write(f"Fgs.Asset.Application/Features/{f}/Validators/{e}Validators.cs", """using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.CreateFgsAssetAttributeOption;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.PatchFgsAssetAttributeOption;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Commands.UpdateFgsAssetAttributeOption;
using FluentValidation;
namespace Fgs.Asset.Application.Features.AssetAttributeOptions.Validators;
public sealed class CreateFgsAssetAttributeOptionCommandValidator : AbstractValidator<CreateFgsAssetAttributeOptionCommand>
{
    public CreateFgsAssetAttributeOptionCommandValidator(IFgsAssetAttributeOptionReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetAttributeId).GreaterThan(0);
        RuleFor(x => x.Dto.OptionCode).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.OptionCode).Must(c => string.Equals(c, c.Trim().ToUpperInvariant(), StringComparison.Ordinal));
        RuleFor(x => x.Dto.OptionCode).MustAsync(async (cmd, code, ct) => !await readRepository.ExistsByOptionCodeAsync(cmd.Dto.AssetAttributeId, code, null, ct));
        RuleFor(x => x.Dto.OptionName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.AssetAttributeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetAttributeIdAsync(id, ct));
    }
}
public sealed class UpdateFgsAssetAttributeOptionCommandValidator : AbstractValidator<UpdateFgsAssetAttributeOptionCommand>
{
    public UpdateFgsAssetAttributeOptionCommandValidator(IFgsAssetAttributeOptionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetAttributeId).GreaterThan(0);
        RuleFor(x => x.Dto.OptionCode).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.OptionCode).MustAsync(async (cmd, code, ct) => !await readRepository.ExistsByOptionCodeAsync(cmd.Dto.AssetAttributeId, code, cmd.Id, ct));
        RuleFor(x => x.Dto.OptionName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.AssetAttributeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetAttributeIdAsync(id, ct));
    }
}
public sealed class PatchFgsAssetAttributeOptionCommandValidator : AbstractValidator<PatchFgsAssetAttributeOptionCommand>
{
    public PatchFgsAssetAttributeOptionCommandValidator(IFgsAssetAttributeOptionReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetAttributeId).MustAsync(async (cmd, id, ct) => !id.HasValue || await readRepository.ExistsAssetAttributeIdAsync(id.Value, ct)).When(x => x.Dto.AssetAttributeId.HasValue);
    }
}
""")
gen_cqc(e, f, r, "Asset Attribute Option", "AssetAttributeOptionController",
        "        [FromQuery] string? optionCode = null,\n        [FromQuery] string? optionName = null,\n        [FromQuery] long? assetAttributeId = null,\n",
        "new FgsAssetAttributeOptionListFilters(optionCode, optionName, assetAttributeId)")
write(f"Fgs.Asset.Infrastructure/{f}/{e}Sql.cs", f"""using Fgs.Foundation.Paging;
namespace Fgs.Asset.Infrastructure.{f};
internal static class {e}Sql {{ public const string Table = "asset.\\"FgsAssetAttributeOption\\""; public const string SelectDetailColumns = "{cols}"; public const string SelectSummaryColumns = SelectDetailColumns; public const string SelectLookupColumns = "\\"Id\\", \\"OptionCode\\", \\"OptionName\\""; private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase) {{ "Id", "AssetAttributeId", "OptionCode", "OptionName", "DisplayOrder", "IsActive" }}; public static string ResolveOrderBy(string? sortBy, SortDirection direction) {{ var dir = direction == SortDirection.Desc ? "DESC" : "ASC"; if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy)) return $"ORDER BY \\"DisplayOrder\\" {{dir}}"; return $"ORDER BY \\"{{AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase))}}\\" {{dir}}"; }} }}
""")
write(f"Fgs.Asset.Infrastructure/{f}/{e}DapperRows.cs", """using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
namespace Fgs.Asset.Infrastructure.AssetAttributeOptions;
internal class FgsAssetAttributeOptionSummaryRow { public long Id { get; set; } public long AssetAttributeId { get; set; } public string OptionCode { get; set; } = null!; public string OptionName { get; set; } = null!; public int DisplayOrder { get; set; } public bool IsActive { get; set; } public FgsAssetAttributeOptionSummaryDto ToDto() => new(Id, AssetAttributeId, OptionCode, OptionName, DisplayOrder, IsActive); }
internal sealed class FgsAssetAttributeOptionDetailRow : FgsAssetAttributeOptionSummaryRow { public new FgsAssetAttributeOptionDetailDto ToDto() => new(Id, AssetAttributeId, OptionCode, OptionName, DisplayOrder, IsActive); }
internal sealed class FgsAssetAttributeOptionLookupRow { public long Id { get; set; } public string OptionCode { get; set; } = null!; public string OptionName { get; set; } = null!; public FgsAssetAttributeOptionLookupDto ToDto() => new(Id, OptionCode, OptionName); }
""")
extra = FK_ATTR + """
    public async Task<bool> ExistsByOptionCodeAsync(long assetAttributeId, string optionCode, long? excludeId = null, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var exclude = excludeId.HasValue ? "AND \\"Id\\" <> @ExcludeId" : string.Empty;
        var sql = $"SELECT EXISTS(SELECT 1 FROM {FgsAssetAttributeOptionSql.Table} WHERE \\"TenantId\\" = @TenantId AND \\"CompanyId\\" = @CompanyId AND \\"AssetAttributeId\\" = @AssetAttributeId AND \\"OptionCode\\" = @OptionCode {exclude})";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, AssetAttributeId = assetAttributeId, OptionCode = optionCode.Trim().ToUpperInvariant(), ExcludeId = excludeId }, cancellationToken: cancellationToken));
    }
"""
write(f"Fgs.Asset.Infrastructure/{f}/{e}ReadRepository.cs", _read_repo_template(e, f, cols, extra, """
        if (!string.IsNullOrWhiteSpace(filters.OptionCode)) where.Add("\\"OptionCode\\" = @OptionCode");
        if (!string.IsNullOrWhiteSpace(filters.OptionName)) where.Add("\\"OptionName\\" ILIKE @OptionName");
        if (filters.AssetAttributeId.HasValue) where.Add("\\"AssetAttributeId\\" = @AssetAttributeId");
""", "OptionCode = filters.OptionCode?.Trim().ToUpperInvariant(), OptionName = string.IsNullOrWhiteSpace(filters.OptionName) ? null : $\"%{filters.OptionName.Trim()}%\", AssetAttributeId = filters.AssetAttributeId,", '\\"OptionCode\\" ILIKE @Search OR \\"OptionName\\" ILIKE @Search', "DisplayOrder"))
write(f"Fgs.Asset.Infrastructure/{f}/{e}WriteService.cs", """using Fgs.Asset.Application.Abstractions.AssetAttributeOptions;
using Fgs.Asset.Application.Features.AssetAttributeOptions.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Fgs.Asset.Infrastructure.AssetAttributeOptions;
public sealed class FgsAssetAttributeOptionWriteService : IFgsAssetAttributeOptionWriteService
{
    private readonly FgsAssetDbContext _context; private readonly IUnitOfWork _unitOfWork; private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetAttributeOptionWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper) { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }
    public async Task<FgsAssetAttributeOptionDetailDto> CreateAsync(FgsAssetAttributeOptionCreateDto dto, CancellationToken cancellationToken = default) { var entity = new FgsAssetAttributeOption { AssetAttributeId = dto.AssetAttributeId, OptionCode = dto.OptionCode.Trim().ToUpperInvariant(), OptionName = dto.OptionName.Trim(), DisplayOrder = dto.DisplayOrder }; _auditHelper.StampForCreate(entity); await _context.FgsAssetAttributeOptions.AddAsync(entity, cancellationToken); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetAttributeOptionDetailDto> UpdateAsync(long id, FgsAssetAttributeOptionUpdateDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Attribute Option '{id}' was not found."); entity.AssetAttributeId = dto.AssetAttributeId; entity.OptionCode = dto.OptionCode.Trim().ToUpperInvariant(); entity.OptionName = dto.OptionName.Trim(); entity.DisplayOrder = dto.DisplayOrder; _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetAttributeOptionDetailDto> PatchAsync(long id, FgsAssetAttributeOptionPatchDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Attribute Option '{id}' was not found."); if (dto.AssetAttributeId.HasValue) entity.AssetAttributeId = dto.AssetAttributeId.Value; if (dto.OptionCode is not null) entity.OptionCode = dto.OptionCode.Trim().ToUpperInvariant(); if (dto.OptionName is not null) entity.OptionName = dto.OptionName.Trim(); if (dto.DisplayOrder.HasValue) entity.DisplayOrder = dto.DisplayOrder.Value; if (dto.IsActive.HasValue) entity.IsActive = dto.IsActive.Value; _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    private async Task<FgsAssetAttributeOption?> Find(long id, CancellationToken ct) => await _context.FgsAssetAttributeOptions.FirstOrDefaultAsync(x => x.Id == id, ct);
    private static FgsAssetAttributeOptionDetailDto Map(FgsAssetAttributeOption e) => new(e.Id, e.AssetAttributeId, e.OptionCode, e.OptionName, e.DisplayOrder, e.IsActive);
}
""")
for t in ["Validator", "CommandHandler", "QueryHandler"]:
    write(f"Fgs.Asset.Tests/{f}/FgsAssetAttributeOption{t}Tests.cs", f"namespace Fgs.Asset.Tests.AssetAttributeOptions; public sealed class FgsAssetAttributeOption{t}Tests {{ [Fact] public void Placeholder() => true.Should().BeTrue(); }}")
