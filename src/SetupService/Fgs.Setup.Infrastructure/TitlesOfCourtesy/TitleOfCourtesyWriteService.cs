using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.TitlesOfCourtesy;
using Fgs.Setup.Application.Features.TitlesOfCourtesy.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.TitlesOfCourtesy;

public sealed class TitleOfCourtesyWriteService : ITitleOfCourtesyWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public TitleOfCourtesyWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<TitleOfCourtesyDetailDto> CreateAsync(
        TitleOfCourtesyCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupTitleOfCourtesy
        {
            Code = NormalizeCode(dto.Code),
            DisplayName = dto.DisplayName.Trim(),
            SortOrder = dto.SortOrder
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupTitlesOfCourtesy.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<TitleOfCourtesyDetailDto> UpdateAsync(
        long id,
        TitleOfCourtesyUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Title of courtesy '{id}' was not found.");

        entity.Code = NormalizeCode(dto.Code);
        entity.DisplayName = dto.DisplayName.Trim();
        entity.SortOrder = dto.SortOrder;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<TitleOfCourtesyDetailDto> PatchAsync(
        long id,
        TitleOfCourtesyPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Title of courtesy '{id}' was not found.");

        if (dto.Code is not null)
        {
            entity.Code = NormalizeCode(dto.Code);
        }

        if (dto.DisplayName is not null)
        {
            entity.DisplayName = dto.DisplayName.Trim();
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

    public async Task<TitleOfCourtesyDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Title of courtesy '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupTitleOfCourtesy?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupTitlesOfCourtesy.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A title of courtesy with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static TitleOfCourtesyDetailDto MapToDetail(FgsSetupTitleOfCourtesy entity) =>
        new(
            entity.Id,
            entity.TenantId,
            entity.CompanyId,
            entity.Code,
            entity.DisplayName,
            entity.SortOrder,
            entity.IsActive,
            entity.CreatedOn,
            entity.CreatedBy,
            entity.UpdatedOn,
            entity.UpdatedBy);
}
