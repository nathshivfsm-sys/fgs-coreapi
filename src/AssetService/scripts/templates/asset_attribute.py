# AssetAttribute generator template
e, f, r = "FgsAssetAttribute", "AssetAttributes", "assetattribute"
cols = '\\"Id\\", \\"AssetTypeId\\", \\"AttributeCode\\", \\"AttributeName\\", \\"InputType\\", \\"DefaultOptionId\\", \\"DefaultValueText\\", \\"DefaultValueInteger\\", \\"DefaultValueDecimal\\", \\"DefaultValueDate\\", \\"DefaultValueBoolean\\", \\"IsRequired\\", \\"IsSearchable\\", \\"DisplayOrder\\", \\"IsActive\\"'

write(f"Fgs.Asset.Application/Abstractions/{f}/I{e}ReadRepository.cs", f"""using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.{f}.Dtos;
using Fgs.Foundation.Paging;
namespace Fgs.Asset.Application.Abstractions.{f};
public interface I{e}ReadRepository {{
    Task<{e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedResult<{e}SummaryDto>> ListAsync(AssetListQuery query, {e}ListFilters filters, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<{e}LookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> ExistsByAttributeCodeAsync(long assetTypeId, string attributeCode, long? excludeId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsAssetTypeIdAsync(long assetTypeId, CancellationToken cancellationToken = default);
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

write(f"Fgs.Asset.Application/Features/{f}/Dtos/{e}Dtos.cs", """namespace Fgs.Asset.Application.Features.AssetAttributes.Dtos;
public sealed record FgsAssetAttributeSummaryDto(long Id, long AssetTypeId, string AttributeCode, string AttributeName, string InputType, long? DefaultOptionId, string? DefaultValueText, int? DefaultValueInteger, decimal? DefaultValueDecimal, DateOnly? DefaultValueDate, bool? DefaultValueBoolean, bool IsRequired, bool IsSearchable, int DisplayOrder, bool IsActive);
public sealed record FgsAssetAttributeDetailDto(long Id, long AssetTypeId, string AttributeCode, string AttributeName, string InputType, long? DefaultOptionId, string? DefaultValueText, int? DefaultValueInteger, decimal? DefaultValueDecimal, DateOnly? DefaultValueDate, bool? DefaultValueBoolean, bool IsRequired, bool IsSearchable, int DisplayOrder, bool IsActive);
public sealed record FgsAssetAttributeLookupDto(long Id, string AttributeCode, string AttributeName);
public sealed record FgsAssetAttributeCreateDto(long AssetTypeId, string AttributeCode, string AttributeName, string InputType, long? DefaultOptionId, string? DefaultValueText, int? DefaultValueInteger, decimal? DefaultValueDecimal, DateOnly? DefaultValueDate, bool? DefaultValueBoolean, bool IsRequired, bool IsSearchable, int DisplayOrder);
public sealed record FgsAssetAttributeUpdateDto(long AssetTypeId, string AttributeCode, string AttributeName, string InputType, long? DefaultOptionId, string? DefaultValueText, int? DefaultValueInteger, decimal? DefaultValueDecimal, DateOnly? DefaultValueDate, bool? DefaultValueBoolean, bool IsRequired, bool IsSearchable, int DisplayOrder);
public sealed record FgsAssetAttributePatchDto(long? AssetTypeId, string? AttributeCode, string? AttributeName, string? InputType, long? DefaultOptionId, string? DefaultValueText, int? DefaultValueInteger, decimal? DefaultValueDecimal, DateOnly? DefaultValueDate, bool? DefaultValueBoolean, bool? IsRequired, bool? IsSearchable, int? DisplayOrder, bool? IsActive);
public sealed record FgsAssetAttributeListFilters(string? AttributeCode = null, string? AttributeName = null, long? AssetTypeId = null);
""")

write(f"Fgs.Asset.Application/Features/{f}/Validators/{e}Validators.cs", """using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Features.AssetAttributes.Commands.CreateFgsAssetAttribute;
using Fgs.Asset.Application.Features.AssetAttributes.Commands.PatchFgsAssetAttribute;
using Fgs.Asset.Application.Features.AssetAttributes.Commands.UpdateFgsAssetAttribute;
using FluentValidation;
namespace Fgs.Asset.Application.Features.AssetAttributes.Validators;
public sealed class CreateFgsAssetAttributeCommandValidator : AbstractValidator<CreateFgsAssetAttributeCommand>
{
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "TEXT", "TEXTAREA", "INTEGER", "DECIMAL", "DATE", "BOOLEAN", "DROPDOWN" };
    public CreateFgsAssetAttributeCommandValidator(IFgsAssetAttributeReadRepository readRepository)
    {
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.AttributeCode).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.AttributeCode).Must(c => string.Equals(c, c.Trim().ToUpperInvariant(), StringComparison.Ordinal));
        RuleFor(x => x.Dto.AttributeCode).MustAsync(async (cmd, code, ct) => !await readRepository.ExistsByAttributeCodeAsync(cmd.Dto.AssetTypeId, code, null, ct));
        RuleFor(x => x.Dto.AttributeName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.InputType).NotEmpty().Must(t => Allowed.Contains(t));
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct));
        RuleFor(x => x.Dto.DefaultValueText).MaximumLength(500);
    }
}
public sealed class UpdateFgsAssetAttributeCommandValidator : AbstractValidator<UpdateFgsAssetAttributeCommand>
{
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "TEXT", "TEXTAREA", "INTEGER", "DECIMAL", "DATE", "BOOLEAN", "DROPDOWN" };
    public UpdateFgsAssetAttributeCommandValidator(IFgsAssetAttributeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.AssetTypeId).GreaterThan(0);
        RuleFor(x => x.Dto.AttributeCode).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.AttributeCode).MustAsync(async (cmd, code, ct) => !await readRepository.ExistsByAttributeCodeAsync(cmd.Dto.AssetTypeId, code, cmd.Id, ct));
        RuleFor(x => x.Dto.AttributeName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.InputType).NotEmpty().Must(t => Allowed.Contains(t));
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => await readRepository.ExistsAssetTypeIdAsync(id, ct));
    }
}
public sealed class PatchFgsAssetAttributeCommandValidator : AbstractValidator<PatchFgsAssetAttributeCommand>
{
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "TEXT", "TEXTAREA", "INTEGER", "DECIMAL", "DATE", "BOOLEAN", "DROPDOWN" };
    public PatchFgsAssetAttributeCommandValidator(IFgsAssetAttributeReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.InputType).Must(t => Allowed.Contains(t!)).When(x => x.Dto.InputType is not null);
        RuleFor(x => x.Dto.AssetTypeId).MustAsync(async (cmd, id, ct) => !id.HasValue || await readRepository.ExistsAssetTypeIdAsync(id.Value, ct)).When(x => x.Dto.AssetTypeId.HasValue);
    }
}
""")

gen_cqc(e, f, r, "Asset Attribute", "AssetAttributeController",
        "        [FromQuery] string? attributeCode = null,\n        [FromQuery] string? attributeName = null,\n        [FromQuery] long? assetTypeId = null,\n",
        "new FgsAssetAttributeListFilters(attributeCode, attributeName, assetTypeId)")

write(f"Fgs.Asset.Infrastructure/{f}/{e}Sql.cs", f"""using Fgs.Foundation.Paging;
namespace Fgs.Asset.Infrastructure.{f};
internal static class {e}Sql {{
    public const string Table = "asset.\\"FgsAssetAttribute\\"";
    public const string SelectDetailColumns = "{cols}";
    public const string SelectSummaryColumns = SelectDetailColumns;
    public const string SelectLookupColumns = "\\"Id\\", \\"AttributeCode\\", \\"AttributeName\\"";
    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase) {{ "Id", "AssetTypeId", "AttributeCode", "AttributeName", "InputType", "DisplayOrder", "IsActive" }};
    public static string ResolveOrderBy(string? sortBy, SortDirection direction) {{ var dir = direction == SortDirection.Desc ? "DESC" : "ASC"; if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy)) return $"ORDER BY \\"DisplayOrder\\" {{dir}}, \\"Id\\" {{dir}}"; return $"ORDER BY \\"{{AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase))}}\\" {{dir}}"; }}
}}
""")

write(f"Fgs.Asset.Infrastructure/{f}/{e}DapperRows.cs", """using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
namespace Fgs.Asset.Infrastructure.AssetAttributes;
internal class FgsAssetAttributeSummaryRow { public long Id { get; set; } public long AssetTypeId { get; set; } public string AttributeCode { get; set; } = null!; public string AttributeName { get; set; } = null!; public string InputType { get; set; } = null!; public long? DefaultOptionId { get; set; } public string? DefaultValueText { get; set; } public int? DefaultValueInteger { get; set; } public decimal? DefaultValueDecimal { get; set; } public DateOnly? DefaultValueDate { get; set; } public bool? DefaultValueBoolean { get; set; } public bool IsRequired { get; set; } public bool IsSearchable { get; set; } public int DisplayOrder { get; set; } public bool IsActive { get; set; } public FgsAssetAttributeSummaryDto ToDto() => new(Id, AssetTypeId, AttributeCode, AttributeName, InputType, DefaultOptionId, DefaultValueText, DefaultValueInteger, DefaultValueDecimal, DefaultValueDate, DefaultValueBoolean, IsRequired, IsSearchable, DisplayOrder, IsActive); }
internal sealed class FgsAssetAttributeDetailRow : FgsAssetAttributeSummaryRow { public new FgsAssetAttributeDetailDto ToDto() => new(Id, AssetTypeId, AttributeCode, AttributeName, InputType, DefaultOptionId, DefaultValueText, DefaultValueInteger, DefaultValueDecimal, DefaultValueDate, DefaultValueBoolean, IsRequired, IsSearchable, DisplayOrder, IsActive); }
internal sealed class FgsAssetAttributeLookupRow { public long Id { get; set; } public string AttributeCode { get; set; } = null!; public string AttributeName { get; set; } = null!; public FgsAssetAttributeLookupDto ToDto() => new(Id, AttributeCode, AttributeName); }
""")

extra = FK_TYPE + """
    public async Task<bool> ExistsByAttributeCodeAsync(long assetTypeId, string attributeCode, long? excludeId = null, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = AssetTenantScopeResolver.ResolveRequired(_tenantContextAccessor);
        var exclude = excludeId.HasValue ? "AND \\"Id\\" <> @ExcludeId" : string.Empty;
        var sql = $"SELECT EXISTS(SELECT 1 FROM {FgsAssetAttributeSql.Table} WHERE \"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId AND \"AssetTypeId\" = @AssetTypeId AND \"AttributeCode\" = @AttributeCode {exclude})";
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, CompanyId = companyId, AssetTypeId = assetTypeId, AttributeCode = attributeCode.Trim().ToUpperInvariant(), ExcludeId = excludeId }, cancellationToken: cancellationToken));
    }
"""
write(f"Fgs.Asset.Infrastructure/{f}/{e}ReadRepository.cs", _read_repo_template(e, f, cols, extra, """
        if (!string.IsNullOrWhiteSpace(filters.AttributeCode)) where.Add("\\"AttributeCode\\" = @AttributeCode");
        if (!string.IsNullOrWhiteSpace(filters.AttributeName)) where.Add("\\"AttributeName\\" ILIKE @AttributeName");
        if (filters.AssetTypeId.HasValue) where.Add("\\"AssetTypeId\\" = @AssetTypeId");
""", "AttributeCode = filters.AttributeCode?.Trim().ToUpperInvariant(), AttributeName = string.IsNullOrWhiteSpace(filters.AttributeName) ? null : $\"%{filters.AttributeName.Trim()}%\", AssetTypeId = filters.AssetTypeId,", '\\"AttributeCode\\" ILIKE @Search OR \\"AttributeName\\" ILIKE @Search', "DisplayOrder"))

write(f"Fgs.Asset.Infrastructure/{f}/{e}WriteService.cs", """using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Asset.Domain.Entities;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;
namespace Fgs.Asset.Infrastructure.AssetAttributes;
public sealed class FgsAssetAttributeWriteService : IFgsAssetAttributeWriteService
{
    private readonly FgsAssetDbContext _context; private readonly IUnitOfWork _unitOfWork; private readonly AssetEntityAuditHelper _auditHelper;
    public FgsAssetAttributeWriteService(FgsAssetDbContext context, IUnitOfWork unitOfWork, AssetEntityAuditHelper auditHelper) { _context = context; _unitOfWork = unitOfWork; _auditHelper = auditHelper; }
    public async Task<FgsAssetAttributeDetailDto> CreateAsync(FgsAssetAttributeCreateDto dto, CancellationToken cancellationToken = default) { var entity = MapCreate(dto); _auditHelper.StampForCreate(entity); await _context.FgsAssetAttributes.AddAsync(entity, cancellationToken); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetAttributeDetailDto> UpdateAsync(long id, FgsAssetAttributeUpdateDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Attribute '{id}' was not found."); ApplyUpdate(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    public async Task<FgsAssetAttributeDetailDto> PatchAsync(long id, FgsAssetAttributePatchDto dto, CancellationToken cancellationToken = default) { var entity = await Find(id, cancellationToken) ?? throw new KeyNotFoundException($"Asset Attribute '{id}' was not found."); ApplyPatch(entity, dto); _auditHelper.StampForUpdate(entity); await _unitOfWork.SaveChangesAsync(cancellationToken); return Map(entity); }
    private async Task<FgsAssetAttribute?> Find(long id, CancellationToken ct) => await _context.FgsAssetAttributes.FirstOrDefaultAsync(x => x.Id == id, ct);
    private static FgsAssetAttribute MapCreate(FgsAssetAttributeCreateDto dto) => new() { AssetTypeId = dto.AssetTypeId, AttributeCode = dto.AttributeCode.Trim().ToUpperInvariant(), AttributeName = dto.AttributeName.Trim(), InputType = dto.InputType.Trim().ToUpperInvariant(), DefaultOptionId = dto.DefaultOptionId, DefaultValueText = Trim(dto.DefaultValueText), DefaultValueInteger = dto.DefaultValueInteger, DefaultValueDecimal = dto.DefaultValueDecimal, DefaultValueDate = dto.DefaultValueDate, DefaultValueBoolean = dto.DefaultValueBoolean, IsRequired = dto.IsRequired, IsSearchable = dto.IsSearchable, DisplayOrder = dto.DisplayOrder };
    private static void ApplyUpdate(FgsAssetAttribute e, FgsAssetAttributeUpdateDto dto) { e.AssetTypeId = dto.AssetTypeId; e.AttributeCode = dto.AttributeCode.Trim().ToUpperInvariant(); e.AttributeName = dto.AttributeName.Trim(); e.InputType = dto.InputType.Trim().ToUpperInvariant(); e.DefaultOptionId = dto.DefaultOptionId; e.DefaultValueText = Trim(dto.DefaultValueText); e.DefaultValueInteger = dto.DefaultValueInteger; e.DefaultValueDecimal = dto.DefaultValueDecimal; e.DefaultValueDate = dto.DefaultValueDate; e.DefaultValueBoolean = dto.DefaultValueBoolean; e.IsRequired = dto.IsRequired; e.IsSearchable = dto.IsSearchable; e.DisplayOrder = dto.DisplayOrder; }
    private static void ApplyPatch(FgsAssetAttribute e, FgsAssetAttributePatchDto dto) { if (dto.AssetTypeId.HasValue) e.AssetTypeId = dto.AssetTypeId.Value; if (dto.AttributeCode is not null) e.AttributeCode = dto.AttributeCode.Trim().ToUpperInvariant(); if (dto.AttributeName is not null) e.AttributeName = dto.AttributeName.Trim(); if (dto.InputType is not null) e.InputType = dto.InputType.Trim().ToUpperInvariant(); if (dto.DefaultOptionId.HasValue) e.DefaultOptionId = dto.DefaultOptionId; if (dto.DefaultValueText is not null) e.DefaultValueText = Trim(dto.DefaultValueText); if (dto.DefaultValueInteger.HasValue) e.DefaultValueInteger = dto.DefaultValueInteger; if (dto.DefaultValueDecimal.HasValue) e.DefaultValueDecimal = dto.DefaultValueDecimal; if (dto.DefaultValueDate.HasValue) e.DefaultValueDate = dto.DefaultValueDate; if (dto.DefaultValueBoolean.HasValue) e.DefaultValueBoolean = dto.DefaultValueBoolean; if (dto.IsRequired.HasValue) e.IsRequired = dto.IsRequired.Value; if (dto.IsSearchable.HasValue) e.IsSearchable = dto.IsSearchable.Value; if (dto.DisplayOrder.HasValue) e.DisplayOrder = dto.DisplayOrder.Value; if (dto.IsActive.HasValue) e.IsActive = dto.IsActive.Value; }
    private static string? Trim(string? v) => string.IsNullOrWhiteSpace(v) ? null : v.Trim();
    private static FgsAssetAttributeDetailDto Map(FgsAssetAttribute e) => new(e.Id, e.AssetTypeId, e.AttributeCode, e.AttributeName, e.InputType, e.DefaultOptionId, e.DefaultValueText, e.DefaultValueInteger, e.DefaultValueDecimal, e.DefaultValueDate, e.DefaultValueBoolean, e.IsRequired, e.IsSearchable, e.DisplayOrder, e.IsActive);
}
""")

write(f"Fgs.Asset.Tests/{f}/FgsAssetAttributeValidatorTests.cs", """using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Features.AssetAttributes.Commands.CreateFgsAssetAttribute;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Asset.Application.Features.AssetAttributes.Validators;
using Moq;
namespace Fgs.Asset.Tests.AssetAttributes;
public sealed class FgsAssetAttributeValidatorTests
{
  [Fact] public async Task CreateValidator_RejectsInvalidInputType() { var repo = new Mock<IFgsAssetAttributeReadRepository>(); repo.Setup(r => r.ExistsAssetTypeIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true); var v = new CreateFgsAssetAttributeCommandValidator(repo.Object); var r = await v.ValidateAsync(new CreateFgsAssetAttributeCommand(new FgsAssetAttributeCreateDto(1, "CODE", "Name", "BAD", null, null, null, null, null, null, false, true, 0))); r.IsValid.Should().BeFalse(); }
}
""")
write(f"Fgs.Asset.Tests/{f}/FgsAssetAttributeCommandHandlerTests.cs", """using Fgs.Asset.Application.Features.AssetAttributes.Commands.CreateFgsAssetAttribute;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Asset.Infrastructure.AssetAttributes;
using Fgs.Asset.Infrastructure.Common;
using Fgs.Asset.Infrastructure.Common.Time;
using Fgs.Asset.Infrastructure.Database;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Persistence.Implementations;
using Fgs.Security.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
namespace Fgs.Asset.Tests.AssetAttributes;
public sealed class FgsAssetAttributeCommandHandlerTests
{
  [Fact] public async Task CreateHandler_PersistsRecord() { var opts = new DbContextOptionsBuilder<FgsAssetDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options; await using var ctx = new FgsAssetDbContext(opts); await ctx.Database.EnsureCreatedAsync(); var uc = new Mock<IFgsUserContext>(); uc.SetupGet(x => x.TenantId).Returns(10L); uc.SetupGet(x => x.CompanyId).Returns(20L); uc.SetupGet(x => x.UserId).Returns(Guid.Parse("11111111-1111-1111-1111-111111111111")); var ta = new TestTenant { Current = new TenantContext { TenantId = 10, CompanyId = 20 } }; var ws = new FgsAssetAttributeWriteService(ctx, new EfUnitOfWork<FgsAssetDbContext>(ctx), new AssetEntityAuditHelper(uc.Object, ta, new DateTimeProvider())); var h = new CreateFgsAssetAttributeCommandHandler(ws, new Mock<ICacheService>().Object, ta, NullLogger<CreateFgsAssetAttributeCommandHandler>.Instance); var res = await h.Handle(new CreateFgsAssetAttributeCommand(new FgsAssetAttributeCreateDto(1, "CODE", "Name", "TEXT", null, null, null, null, null, null, false, true, 0)), CancellationToken.None); res.Success.Should().BeTrue(); }
  private sealed class TestTenant : ITenantContextAccessor { public ITenantContext? Current { get; set; } }
}
""")
write(f"Fgs.Asset.Tests/{f}/FgsAssetAttributeQueryHandlerTests.cs", """using Fgs.Asset.Application.Abstractions.AssetAttributes;
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.AssetAttributes.Dtos;
using Fgs.Asset.Application.Features.AssetAttributes.Queries.ListAssetAttributes;
using Fgs.Foundation.Paging;
using Moq;
namespace Fgs.Asset.Tests.AssetAttributes;
public sealed class FgsAssetAttributeQueryHandlerTests
{
  [Fact] public async Task List_ReturnsPagedResult() { var repo = new Mock<IFgsAssetAttributeReadRepository>(); repo.Setup(r => r.ListAsync(It.IsAny<AssetListQuery>(), It.IsAny<FgsAssetAttributeListFilters>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PagedResult<FgsAssetAttributeSummaryDto>([], 1, 25, 0)); var h = new ListAssetAttributesQueryHandler(repo.Object); var res = await h.Handle(new ListAssetAttributesQuery(new AssetListQuery(), new FgsAssetAttributeListFilters()), CancellationToken.None); res.Success.Should().BeTrue(); }
}
""")
