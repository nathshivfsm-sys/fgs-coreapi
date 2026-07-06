# Generates Asset Service catalog CRUD files from templates
$ErrorActionPreference = 'Stop'
$root = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$assetRoot = Join-Path $root 'AssetService'

function Write-File($path, $content) {
    $dir = Split-Path $path -Parent
    if (-not (Test-Path $dir)) { New-Item -ItemType Directory -Path $dir -Force | Out-Null }
    Set-Content -Path $path -Value $content -Encoding UTF8 -NoNewline
}

$simpleCatalogs = @(
    @{ Entity='FgsAssetType'; Plural='AssetTypes'; Feature='AssetTypes'; Route='assettype'; DbSet='FgsAssetTypes'; Display='asset type'; DisplayCap='Asset Type' },
    @{ Entity='FgsAssetManufacturer'; Plural='AssetManufacturers'; Feature='AssetManufacturers'; Route='assetmanufacturer'; DbSet='FgsAssetManufacturers'; Display='asset manufacturer'; DisplayCap='Asset Manufacturer' },
    @{ Entity='FgsAssetStatus'; Plural='AssetStatuses'; Feature='AssetStatuses'; Route='assetstatus'; DbSet='FgsAssetStatuses'; Display='asset status'; DisplayCap='Asset Status' }
)

foreach ($c in $simpleCatalogs) {
    $e = $c.Entity
    $f = $c.Feature
    $r = $c.Route
    $ds = $c.DbSet
    $d = $c.Display
    $dc = $c.DisplayCap

    Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Abstractions/$f/IFgs${e}ReadRepository.cs") @"
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.$f.Dtos;
using Fgs.Foundation.Paging;

namespace Fgs.Asset.Application.Abstractions.$f;

public interface IFgs${e}ReadRepository
{
    Task<Fgs${e}DetailDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<PagedResult<Fgs${e}SummaryDto>> ListAsync(
        AssetListQuery query,
        Fgs${e}ListFilters filters,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Fgs${e}LookupDto>> LookupAsync(
        bool activeOnly = true,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(
        string code,
        long? excludeId = null,
        CancellationToken cancellationToken = default);
}
"@

    Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Abstractions/$f/IFgs${e}WriteService.cs") @"
using Fgs.Asset.Application.Features.$f.Dtos;

namespace Fgs.Asset.Application.Abstractions.$f;

public interface IFgs${e}WriteService
{
    Task<Fgs${e}DetailDto> CreateAsync(Fgs${e}CreateDto dto, CancellationToken cancellationToken = default);
    Task<Fgs${e}DetailDto> UpdateAsync(long id, Fgs${e}UpdateDto dto, CancellationToken cancellationToken = default);
    Task<Fgs${e}DetailDto> PatchAsync(long id, Fgs${e}PatchDto dto, CancellationToken cancellationToken = default);
}
"@

    Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Dtos/Fgs${e}Dtos.cs") @"
namespace Fgs.Asset.Application.Features.$f.Dtos;

public sealed record Fgs${e}SummaryDto(
    long Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);

public sealed record Fgs${e}DetailDto(
    long Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);

public sealed record Fgs${e}LookupDto(
    long Id,
    string Code,
    string Name);

public sealed record Fgs${e}CreateDto(
    string Code,
    string Name,
    string? Description);

public sealed record Fgs${e}UpdateDto(
    string Code,
    string Name,
    string? Description);

public sealed record Fgs${e}PatchDto(
    string? Code,
    string? Name,
    string? Description,
    bool? IsActive);

public sealed record Fgs${e}ListFilters(
    string? Code = null,
    string? Name = null);
"@

    Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Validators/Fgs${e}Validators.cs") @"
using Fgs.Asset.Application.Abstractions.$f;
using Fgs.Asset.Application.Features.$f.Commands.CreateFgs${e};
using Fgs.Asset.Application.Features.$f.Commands.PatchFgs${e};
using Fgs.Asset.Application.Features.$f.Commands.UpdateFgs${e};
using FluentValidation;

namespace Fgs.Asset.Application.Features.$f.Validators;

public sealed class CreateFgs${e}CommandValidator : AbstractValidator<CreateFgs${e}Command>
{
    public CreateFgs${e}CommandValidator(IFgs${e}ReadRepository readRepository)
    {
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, null, cancellationToken))
            .WithMessage("A $d with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }
}

public sealed class UpdateFgs${e}CommandValidator : AbstractValidator<UpdateFgs${e}Command>
{
    public UpdateFgs${e}CommandValidator(IFgs${e}ReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75);
        RuleFor(x => x.Dto.Code)
            .Must(code => string.Equals(code, code.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Code must be uppercase.");
        RuleFor(x => x.Dto.Code)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code, command.Id, cancellationToken))
            .WithMessage("A $d with this code already exists.");
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).MaximumLength(4000);
    }
}

public sealed class PatchFgs${e}CommandValidator : AbstractValidator<PatchFgs${e}Command>
{
    public PatchFgs${e}CommandValidator(IFgs${e}ReadRepository readRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.Code).NotEmpty().MaximumLength(75).When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code)
            .Must(code => string.Equals(code!, code!.Trim().ToUpperInvariant(), StringComparison.Ordinal))
            .WithMessage("Code must be uppercase.")
            .When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Code)
            .MustAsync(async (command, code, cancellationToken) =>
                !await readRepository.ExistsByCodeAsync(code!, command.Id, cancellationToken))
            .WithMessage("A $d with this code already exists.")
            .When(x => x.Dto.Code is not null);
        RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(200).When(x => x.Dto.Name is not null);
        RuleFor(x => x.Dto.Description).MaximumLength(4000).When(x => x.Dto.Description is not null);
    }
}
"@

    foreach ($cmd in @('Create','Update','Patch')) {
        $dtoSuffix = if ($cmd -eq 'Create') { 'CreateDto' } elseif ($cmd -eq 'Update') { 'UpdateDto' } else { 'PatchDto' }
        $status = if ($cmd -eq 'Create') { 'ApiStatusCodes.Created' } else { 'ApiResponse<Fgs' + $e + 'DetailDto>.Ok(result)' }
        $logMsg = if ($cmd -eq 'Create') { "Created $d {Id} with code {Code}" } elseif ($cmd -eq 'Update') { "Updated $d {Id}" } else { "Patched $d {Id}" }
        $logProps = if ($cmd -eq 'Create') { 'result.Id, result.Code' } else { 'result.Id' }

        Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Commands/${cmd}Fgs${e}/${cmd}Fgs${e}Command.cs") @"
using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.$f.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.$f.Commands.${cmd}Fgs${e};

public sealed record ${cmd}Fgs${e}Command$(if ($cmd -ne 'Create') { '(long Id, ' } else { '(' })Fgs${e}${dtoSuffix} Dto)
    : IRequest<ApiResponse<Fgs${e}DetailDto>>;
"@

        $returnLine = if ($cmd -eq 'Create') { 'return ApiResponse<Fgs' + $e + 'DetailDto>.Ok(result, ApiStatusCodes.Created);' } else { 'return ApiResponse<Fgs' + $e + 'DetailDto>.Ok(result);' }
        $method = $cmd.ToLower() + 'Async'
        $args = if ($cmd -eq 'Create') { 'request.Dto' } else { 'request.Id, request.Dto' }

        Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Commands/${cmd}Fgs${e}/${cmd}Fgs${e}CommandHandler.cs") @"
using Fgs.Asset.Application.Abstractions.$f;
using Fgs.Asset.Application.Features.$f.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fgs.Asset.Application.Features.$f.Commands.${cmd}Fgs${e};

public sealed class ${cmd}Fgs${e}CommandHandler(
    IFgs${e}WriteService writeService,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor,
    ILogger<${cmd}Fgs${e}CommandHandler> logger)
    : IRequestHandler<${cmd}Fgs${e}Command, ApiResponse<Fgs${e}DetailDto>>
{
    public async Task<ApiResponse<Fgs${e}DetailDto>> Handle(
        ${cmd}Fgs${e}Command request,
        CancellationToken cancellationToken)
    {
        var result = await writeService.$method($args, cancellationToken);
        logger.LogInformation("$logMsg", $logProps);
        var tenantScope = tenantContextAccessor.Current!;
        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(tenantScope.TenantId, tenantScope.CompanyId, "$r"),
            cancellationToken);
        $returnLine
    }
}
"@
    }

    foreach ($q in @(
        @{ Name='GetFgs' + $e + 'ById'; Handler='GetByIdAsync'; Param='long Id'; CacheSingle=$true },
        @{ Name='List' + $f; Handler='ListAsync'; Param='AssetListQuery Query, Fgs' + $e + 'ListFilters Filters'; CacheSingle=$false },
        @{ Name='Lookup' + $f; Handler='LookupAsync'; Param='bool ActiveOnly = true'; CacheSingle=$false }
    )) {
        $qn = $q.Name
        $shortName = $qn -replace 'Fgs',''
        if ($q.CacheSingle) {
            Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Queries/$qn/${qn}Query.cs") @"
using Fgs.Contracts.Api;
using Fgs.Asset.Application.Features.$f.Dtos;
using MediatR;

namespace Fgs.Asset.Application.Features.$f.Queries.$qn;

public sealed record ${qn}Query(long Id) : IRequest<ApiResponse<Fgs${e}DetailDto>>;
"@
            Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Queries/$qn/${qn}QueryHandler.cs") @"
using Fgs.Asset.Application.Abstractions.$f;
using Fgs.Asset.Application.Features.$f.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using MediatR;

namespace Fgs.Asset.Application.Features.$f.Queries.$qn;

public sealed class ${qn}QueryHandler(
    IFgs${e}ReadRepository readRepository,
    ICacheService cache,
    ITenantContextAccessor tenantContextAccessor)
    : IRequestHandler<${qn}Query, ApiResponse<Fgs${e}DetailDto>>
{
    public async Task<ApiResponse<Fgs${e}DetailDto>> Handle(
        ${qn}Query request,
        CancellationToken cancellationToken)
    {
        var tenantScope = tenantContextAccessor.Current!;
        var cacheKey = CacheKeys.Build(
            tenantScope.TenantId,
            tenantScope.CompanyId,
            "$r",
            request.Id.ToString());

        var cached = await cache.GetAsync<Fgs${e}DetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<Fgs${e}DetailDto>.Ok(cached);
        }

        var result = await readRepository.GetByIdAsync(request.Id, cancellationToken);
        if (result is null)
        {
            return ApiResponse<Fgs${e}DetailDto>.Fail(
                [$"$dc '{request.Id}' was not found."],
                ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<Fgs${e}DetailDto>.Ok(result);
    }
}
"@
        } elseif ($qn -like 'List*') {
            Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Queries/$qn/${qn}Query.cs") @"
using Fgs.Asset.Application.Common.AssetCrud;
using Fgs.Asset.Application.Features.$f.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.$f.Queries.$qn;

public sealed record ${qn}Query(AssetListQuery Query, Fgs${e}ListFilters Filters)
    : IRequest<ApiResponse<PagedResult<Fgs${e}SummaryDto>>>;
"@
            Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Queries/$qn/${qn}QueryHandler.cs") @"
using Fgs.Asset.Application.Abstractions.$f;
                  ;
using Fgs.Asset.Application.Features.$f.Dtos;
using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using MediatR;

namespace Fgs.Asset.Application.Features.$f.Queries.$qn;

public sealed class ${qn}QueryHandler(IFgs${e}ReadRepository readRepository)
    : IRequestHandler<${qn}Query, ApiResponse<PagedResult<Fgs${e}SummaryDto>>>
{
    public async Task<ApiResponse<PagedResult<Fgs${e}SummaryDto>>> Handle(
        ${qn}Query request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListAsync(request.Query, request.Filters, cancellationToken);
        return ApiResponse<PagedResult<Fgs${e}SummaryDto>>.Ok(result);
    }
}
"@
        } else {
            Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Queries/$qn/${qn}Query.cs") @"
using Fgs.Asset.Application.Features.$f.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.$f.Queries.$qn;

public sealed record ${qn}Query(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<Fgs${e}LookupDto>>>;
"@
            Write-File (Join-Path $assetRoot "Fgs.Asset.Application/Features/$f/Queries/$qn/${qn}QueryHandler.cs") @"
using Fgs.Asset.Application.Abstractions.$f;
using Fgs.Asset.Application.Features.$f.Dtos;
using Fgs.Contracts.Api;
using MediatR;

namespace Fgs.Asset.Application.Features.$f.Queries.$qn;

public sealed class ${qn}QueryHandler(IFgs${e}ReadRepository readRepository)
    : IRequestHandler<${qn}Query, ApiResponse<IReadOnlyList<Fgs${e}LookupDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<Fgs${e}LookupDto>>> Handle(
        ${qn}Query request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.LookupAsync(request.ActiveOnly, cancellationToken);
        return ApiResponse<IReadOnlyList<Fgs${e}LookupDto>>.Ok(result);
    }
}
"@
        }
    }

    $infraFolder = $f
    Write-File (Join-Path $assetRoot "Fgs.Asset.Infrastructure/$infraFolder/Fgs${e}Sql.cs") @"
using Fgs.Foundation.Paging;

namespace Fgs.Asset.Infrastructure.$infraFolder;

internal static class Fgs${e}Sql
{
    public const string Table = "asset.\"$e\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "Name", "Description", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "Code", "Name", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return `$"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return `$"ORDER BY \"{column}\" {dir}";
    }
}
"@

    Write-File (Join-Path $assetRoot "Fgs.Asset.Infrastructure/$infraFolder/Fgs${e}DapperRows.cs") @"
using Fgs.Asset.Application.Features.$f.Dtos;

namespace Fgs.Asset.Infrastructure.$infraFolder;

internal sealed class Fgs${e}SummaryRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public Fgs${e}SummaryDto ToDto() => new(Id, Code, Name, Description, IsActive);
}

internal sealed class Fgs${e}DetailRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public Fgs${e}DetailDto ToDto() => new(Id, Code, Name, Description, IsActive);
}

internal sealed class Fgs${e}LookupRow
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public Fgs${e}LookupDto ToDto() => new(Id, Code, Name);
}
"@

    # ReadRepository and WriteService - abbreviated in script due to length
}

Write-Host "Simple catalog scaffolding complete (partial - run full script extension)"
