using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.GLBreaks;
using Fgs.Setup.Application.Abstractions.Locations;
using Fgs.Setup.Application.Common.Locations;
using Fgs.Setup.Application.Features.GLBreaks.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Entities.GLBreaks;

public sealed class GLBreakWriteService : IGLBreakWriteService
{
    private const string MasterEntityTypeCode = "COMPANY";

    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;
    private readonly ISetupLocationWriteService _locationWriteService;

    public GLBreakWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper,
        ISetupLocationWriteService locationWriteService)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
        _locationWriteService = locationWriteService;
    }

    public async Task<GLBreakDetailDto> CreateAsync(
        GLBreakCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupGLBreak
        {
            Code = dto.Code.Trim(),
            Name = dto.Name.Trim(),
            BreakLabel = TrimOrNull(dto.BreakLabel),
            BreakLevel = dto.BreakLevel,
            LogoFileId = dto.LogoFileId
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupGLBreaks.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        entity.AddressId = await _locationWriteService.UpsertAsync(
            MasterEntityTypeCode,
            entity.CompanyId,
            null,
            dto.Address,
            cancellationToken);

        SyncTrades(entity, dto.TradeCodes ?? []);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<GLBreakDetailDto> UpdateAsync(
        long id,
        GLBreakUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityWithTradesAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"GL break '{id}' was not found.");

        entity.Code = dto.Code.Trim();
        entity.Name = dto.Name.Trim();
        entity.BreakLabel = TrimOrNull(dto.BreakLabel);
        entity.BreakLevel = dto.BreakLevel;
        entity.LogoFileId = dto.LogoFileId;

        entity.AddressId = await _locationWriteService.UpsertAsync(
            MasterEntityTypeCode,
            entity.CompanyId,
            entity.AddressId,
            dto.Address,
            cancellationToken);

        SyncTrades(entity, dto.TradeCodes ?? []);
        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<GLBreakDetailDto> PatchAsync(
        long id,
        GLBreakPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityWithTradesAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"GL break '{id}' was not found.");

        if (dto.Code is not null)
        {
            entity.Code = dto.Code.Trim();
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.BreakLabel is not null)
        {
            entity.BreakLabel = string.IsNullOrWhiteSpace(dto.BreakLabel) ? null : dto.BreakLabel.Trim();
        }

        if (dto.BreakLevel.HasValue)
        {
            entity.BreakLevel = dto.BreakLevel.Value;
        }

        if (dto.LogoFileId.HasValue)
        {
            entity.LogoFileId = dto.LogoFileId;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        if (dto.Address is not null)
        {
            entity.AddressId = await _locationWriteService.UpsertAsync(
                MasterEntityTypeCode,
                entity.CompanyId,
                entity.AddressId,
                dto.Address,
                cancellationToken);
        }

        if (dto.TradeCodes is not null)
        {
            SyncTrades(entity, dto.TradeCodes ?? []);
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    public async Task<GLBreakDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityWithTradesAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"GL break '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await _locationWriteService.SoftDeleteAsync(entity.AddressId, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }

        return await MapToDetailAsync(entity.Id, cancellationToken);
    }

    private void SyncTrades(FgsSetupGLBreak entity, IReadOnlyList<string> tradeCodes)
    {
        var desired = tradeCodes
            .Select(NormalizeTradeCode)
            .Distinct(StringComparer.Ordinal)
            .ToHashSet(StringComparer.Ordinal);

        var toRemove = entity.Trades.Where(t => !desired.Contains(t.TradeCode)).ToList();
        foreach (var trade in toRemove)
        {
            entity.Trades.Remove(trade);
            _context.FgsSetupGLBreakTrades.Remove(trade);
        }

        var existingCodes = entity.Trades.Select(t => t.TradeCode).ToHashSet(StringComparer.Ordinal);
        foreach (var code in desired.Where(c => !existingCodes.Contains(c)))
        {
            var trade = new FgsSetupGLBreakTrade { TradeCode = code };
            _auditHelper.StampForCreate(trade, entity.Id);
            entity.Trades.Add(trade);
        }
    }

    private async Task<FgsSetupGLBreak?> FindEntityWithTradesAsync(
        long id,
        CancellationToken cancellationToken) =>
        await _context.FgsSetupGLBreaks
            .Include(e => e.Trades)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task<GLBreakDetailDto> MapToDetailAsync(long id, CancellationToken cancellationToken)
    {
        var entity = await _context.FgsSetupGLBreaks
            .AsNoTracking()
            .Include(e => e.Trades)
            .FirstAsync(e => e.Id == id, cancellationToken);

        GLBreakAddressDetailDto? address = null;
        if (entity.AddressId is Guid addressId)
        {
            var location = await _context.FgsLocations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == addressId && l.IsActive, cancellationToken);

            if (location is not null)
            {
                address = MapLocation(location);
            }
        }

        var trades = entity.Trades
            .OrderBy(t => t.TradeCode)
            .Select(t => new GLBreakTradeDto(t.Id, t.TradeCode))
            .ToList();

        return new GLBreakDetailDto(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.BreakLabel,
            entity.BreakLevel,
            entity.LogoFileId,
            address,
            trades,
            entity.IsActive);
    }

    private static GLBreakAddressDetailDto MapLocation(FgsLocation location) =>
        new(
            location.Id,
            location.AddressLine1,
            location.AddressLine2,
            location.AddressLine3,
            location.AddressLine4,
            location.City,
            location.State,
            location.Country,
            location.PostalCode,
            location.FormattedAddress,
            location.Latitude,
            location.Longitude);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A GL break or trade mapping with the same unique key already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeTradeCode(string tradeCode) => tradeCode.Trim().ToUpperInvariant();

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
