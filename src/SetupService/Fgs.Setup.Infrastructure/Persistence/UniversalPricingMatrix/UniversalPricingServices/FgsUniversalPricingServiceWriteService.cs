using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.UniversalPricingServices;
using Fgs.Setup.Application.Features.UniversalPricingServices.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.UniversalPricingMatrix.UniversalPricingServices;

public sealed class FgsUniversalPricingServiceWriteService : IFgsUniversalPricingServiceWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsUniversalPricingServiceWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsUniversalPricingServiceDetailDto> CreateAsync(
        FgsUniversalPricingServiceCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsUniversalPricingService
        {
            UniversalPricingServiceCode = NormalizeCode(dto.UniversalPricingServiceCode),
            DisplayOrder = dto.DisplayOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsUniversalPricingServices.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalPricingServiceDetailDto> UpdateAsync(
        long id,
        FgsUniversalPricingServiceUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Pricing Service '{id}' was not found.");

        entity.UniversalPricingServiceCode = NormalizeCode(dto.UniversalPricingServiceCode);
        entity.DisplayOrder = dto.DisplayOrder;
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalPricingServiceDetailDto> PatchAsync(
        long id,
        FgsUniversalPricingServicePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Pricing Service '{id}' was not found.");

        if (dto.UniversalPricingServiceCode is not null)
        {
            entity.UniversalPricingServiceCode = NormalizeCode(dto.UniversalPricingServiceCode);
        }

        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsUniversalPricingServiceDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Universal Pricing Service '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsUniversalPricingService?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsUniversalPricingServices.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A universal pricing service with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsUniversalPricingServiceDetailDto MapToDetail(FgsUniversalPricingService entity) =>
        new(entity.Id, entity.UniversalPricingServiceCode, entity.DisplayOrder, entity.IsActive);
}
