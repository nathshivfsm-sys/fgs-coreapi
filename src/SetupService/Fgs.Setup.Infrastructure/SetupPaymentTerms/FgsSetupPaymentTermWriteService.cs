using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.SetupPaymentTerms;
using Fgs.Setup.Application.Features.SetupPaymentTerms.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.SetupPaymentTerms;

public sealed class FgsSetupPaymentTermWriteService : IFgsSetupPaymentTermWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;

    public FgsSetupPaymentTermWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
    }

    public async Task<FgsSetupPaymentTermDetailDto> CreateAsync(
        FgsSetupPaymentTermCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupPaymentTerm
        {
            Name = dto.Name.Trim(),
            DueDateMethod = dto.DueDateMethod.Trim(),
            NumberOfDays = dto.NumberOfDays ?? 1,
            IsAccountsReceivable = dto.IsAccountsReceivable,
            IsAccountsPayable = dto.IsAccountsPayable,
            IsMobileVisible = dto.IsMobileVisible
        };

        _auditHelper.StampForCreate(entity);
        await _context.FgsSetupPaymentTerms.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPaymentTermDetailDto> UpdateAsync(
        long id,
        FgsSetupPaymentTermUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Payment Term '{id}' was not found.");

        entity.Name = dto.Name.Trim();
        entity.DueDateMethod = dto.DueDateMethod.Trim();
        entity.NumberOfDays = dto.NumberOfDays ?? entity.NumberOfDays;
        entity.IsAccountsReceivable = dto.IsAccountsReceivable;
        entity.IsAccountsPayable = dto.IsAccountsPayable;
        entity.IsMobileVisible = dto.IsMobileVisible;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPaymentTermDetailDto> PatchAsync(
        long id,
        FgsSetupPaymentTermPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Payment Term '{id}' was not found.");

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim(); ;
        }
        if (dto.DueDateMethod is not null)
        {
            entity.DueDateMethod = dto.DueDateMethod.Trim(); ;
        }
        if (dto.NumberOfDays.HasValue)
        {
            entity.NumberOfDays = dto.NumberOfDays.Value;
        }
        if (dto.IsAccountsReceivable.HasValue)
        {
            entity.IsAccountsReceivable = dto.IsAccountsReceivable.Value;
        }
        if (dto.IsAccountsPayable.HasValue)
        {
            entity.IsAccountsPayable = dto.IsAccountsPayable.Value;
        }
        if (dto.IsMobileVisible.HasValue)
        {
            entity.IsMobileVisible = dto.IsMobileVisible.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupPaymentTermDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Payment Term '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupPaymentTerm?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupPaymentTerms.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A payment term with the same code already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupPaymentTermDetailDto MapToDetail(FgsSetupPaymentTerm entity) =>
        new(
            entity.Id,
            entity.Name,
            entity.DueDateMethod,
            entity.NumberOfDays,
            entity.IsAccountsReceivable,
            entity.IsAccountsPayable,
            entity.IsMobileVisible,
            entity.IsActive);
}
