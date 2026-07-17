using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.TechTrades;
using Fgs.Setup.Application.Features.TechTrades.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Persistence.TechTrades;

public sealed class TechTradeWriteService : ITechTradeWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public TechTradeWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<TechTradeDetailDto> CreateAsync(
        TechTradeCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupTechTrade
        {
            TradeCode = NormalizeTradeCode(dto.TradeCode),
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            SortOrder = dto.SortOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupTechTrades.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<TechTradeDetailDto> UpdateAsync(
        long id,
        TechTradeUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tech trade '{id}' was not found.");

        entity.TradeCode = NormalizeTradeCode(dto.TradeCode);
        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim();
        entity.SortOrder = dto.SortOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<TechTradeDetailDto> PatchAsync(
        long id,
        TechTradePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tech trade '{id}' was not found.");

        if (dto.TradeCode is not null)
        {
            entity.TradeCode = NormalizeTradeCode(dto.TradeCode);
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.Description is not null)
        {
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        }

        if (dto.SortOrder.HasValue)
        {
            entity.SortOrder = dto.SortOrder;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<TechTradeDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Tech trade '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupTechTrade?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupTechTrades.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A tech trade with the same trade code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeTradeCode(string tradeCode) => tradeCode.Trim().ToUpperInvariant();

    private static TechTradeDetailDto MapToDetail(FgsSetupTechTrade entity) =>
        new(
            entity.Id,
            entity.TradeCode,
            entity.Name,
            entity.Description,
            entity.SortOrder,
            entity.IsActive);
}
