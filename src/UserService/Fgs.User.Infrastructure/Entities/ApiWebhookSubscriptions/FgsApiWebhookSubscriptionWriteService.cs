using Fgs.MultiTenancy;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.ApiWebhookSubscriptions;
using Fgs.User.Application.Features.ApiWebhookSubscriptions.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Common;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.ApiWebhookSubscriptions;

public sealed class FgsApiWebhookSubscriptionWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork,
    ITenantContextAccessor tenantContextAccessor,
    IFgsUserContext userContext) : IFgsApiWebhookSubscriptionWriteService
{
    public async Task<FgsApiWebhookSubscriptionDetailDto> CreateAsync(
        FgsApiWebhookSubscriptionCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);

        var webhookExists = await context.FgsApiWebhooks.AnyAsync(
            w => w.Id == dto.FgsApiWebhookId && w.TenantId == tenantId && w.CompanyId == companyId,
            cancellationToken);
        if (!webhookExists)
        {
            throw new KeyNotFoundException($"API webhook '{dto.FgsApiWebhookId}' was not found.");
        }

        var eventExists = await context.FgsApiEvents.AnyAsync(
            e => e.Id == dto.FgsApiEventId,
            cancellationToken);
        if (!eventExists)
        {
            throw new KeyNotFoundException($"API event '{dto.FgsApiEventId}' was not found.");
        }

        var entity = new FgsApiWebhookSubscription
        {
            TenantId = tenantId,
            CompanyId = companyId,
            FgsApiWebhookId = dto.FgsApiWebhookId,
            FgsApiEventId = dto.FgsApiEventId,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = ResolveActor()
        };

        await context.FgsApiWebhookSubscriptions.AddAsync(entity, cancellationToken);
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException("This event is already subscribed on the webhook.", ex);
        }

        return new FgsApiWebhookSubscriptionDetailDto(
            entity.Id,
            entity.FgsApiWebhookId,
            entity.FgsApiEventId,
            entity.CreatedOn,
            entity.CreatedBy);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var (tenantId, companyId) = IdentityTenantScopeResolver.ResolveRequired(tenantContextAccessor);
        var entity = await context.FgsApiWebhookSubscriptions.FirstOrDefaultAsync(
            x => x.Id == id && x.TenantId == tenantId && x.CompanyId == companyId,
            cancellationToken)
            ?? throw new KeyNotFoundException($"API webhook subscription '{id}' was not found.");

        context.FgsApiWebhookSubscriptions.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private string ResolveActor() =>
        userContext.Email
        ?? userContext.DisplayName
        ?? userContext.UserId?.ToString()
        ?? "system";

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;
}
