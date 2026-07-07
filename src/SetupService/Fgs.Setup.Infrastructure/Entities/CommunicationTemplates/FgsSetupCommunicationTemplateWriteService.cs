using Fgs.Persistence.Abstractions;
using Fgs.Setup.Application.Abstractions.CommunicationTemplates;
using Fgs.Setup.Application.Features.CommunicationTemplates.Dtos;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Database;
using Fgs.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Fgs.Setup.Infrastructure.Entities.CommunicationTemplates;

public sealed class FgsSetupCommunicationTemplateWriteService : IFgsSetupCommunicationTemplateWriteService
{
    private readonly FgsSetupDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SetupEntityAuditHelper _auditHelper;
    private readonly ITenantContextAccessor _tenantContextAccessor;

    public FgsSetupCommunicationTemplateWriteService(
        FgsSetupDbContext context,
        IUnitOfWork unitOfWork,
        SetupEntityAuditHelper auditHelper,
        ITenantContextAccessor tenantContextAccessor)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _auditHelper = auditHelper;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task<FgsSetupCommunicationTemplateDetailDto> CreateAsync(
        FgsSetupCommunicationTemplateCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsSetupCommunicationTemplate
        {
            CommunicationChannel = dto.CommunicationChannel.Trim(),
            TemplateType = dto.TemplateType.Trim(),
            Code = dto.Code.Trim(),
            Name = dto.Name.Trim(),
            Subject = string.IsNullOrWhiteSpace(dto.Subject) ? null : dto.Subject.Trim(),
            Body = dto.Body.Trim(),
            IsMobileVisible = dto.IsMobileVisible
        };

        long? tenantId = null;
        long? companyId = null;
        if (_tenantContextAccessor.Current is ITenantContext context)
        {
            tenantId = context.TenantId;
            companyId = context.CompanyId;
        }

        _auditHelper.StampForCreate(entity, tenantId, companyId);
        await _context.FgsSetupCommunicationTemplates.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupCommunicationTemplateDetailDto> UpdateAsync(
        long id,
        FgsSetupCommunicationTemplateUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Communication Template '{id}' was not found.");

        entity.CommunicationChannel = dto.CommunicationChannel.Trim();
        entity.TemplateType = dto.TemplateType.Trim();
        entity.Code = dto.Code.Trim();
        entity.Name = dto.Name.Trim();
        entity.Subject = string.IsNullOrWhiteSpace(dto.Subject) ? null : dto.Subject.Trim();
        entity.Body = dto.Body.Trim();
        entity.IsMobileVisible = dto.IsMobileVisible;

        _auditHelper.StampForUpdate(entity);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsSetupCommunicationTemplateDetailDto> PatchAsync(
        long id,
        FgsSetupCommunicationTemplatePatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Communication Template '{id}' was not found.");

        if (dto.CommunicationChannel is not null)
        {
            entity.CommunicationChannel = dto.CommunicationChannel.Trim(); ;
        }
        if (dto.TemplateType is not null)
        {
            entity.TemplateType = dto.TemplateType.Trim(); ;
        }
        if (dto.Code is not null)
        {
            entity.Code = dto.Code.Trim(); ;
        }
        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim(); ;
        }
        if (dto.Subject is not null)
        {
            entity.Subject = string.IsNullOrWhiteSpace(dto.Subject) ? null : dto.Subject.Trim(); ;
        }
        if (dto.Body is not null)
        {
            entity.Body = dto.Body.Trim(); ;
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

    public async Task<FgsSetupCommunicationTemplateDetailDto> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Communication Template '{id}' was not found.");

        if (entity.IsActive)
        {
            entity.IsActive = false;
            _auditHelper.StampForUpdate(entity);
            await SaveChangesAsync(cancellationToken);
        }

        return MapToDetail(entity);
    }

    private async Task<FgsSetupCommunicationTemplate?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await _context.FgsSetupCommunicationTemplates.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("A communication template with the same type and name already exists.", ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeCode(string code) => code.Trim().ToUpperInvariant();

    private static FgsSetupCommunicationTemplateDetailDto MapToDetail(FgsSetupCommunicationTemplate entity) =>
        new(
            entity.Id,
            entity.CommunicationChannel,
            entity.TemplateType,
            entity.Code,
            entity.Name,
            entity.Subject,
            entity.Body,
            entity.IsMobileVisible,
            entity.IsActive);
}
