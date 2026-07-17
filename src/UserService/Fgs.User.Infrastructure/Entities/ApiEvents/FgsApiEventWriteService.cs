using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.ApiEvents;
using Fgs.User.Application.Features.ApiEvents.Dtos;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Infrastructure.Entities.ApiEvents;

public sealed class FgsApiEventWriteService(
    FgsUserDbContext context,
    IUnitOfWork unitOfWork) : IFgsApiEventWriteService
{
    public async Task<FgsApiEventDetailDto> CreateAsync(
        FgsApiEventCreateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = new FgsApiEvent
        {
            EventCode = NormalizeEventCode(dto.EventCode),
            EventCategory = dto.EventCategory.Trim(),
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            EventVersion = dto.EventVersion,
            DisplayOrder = dto.DisplayOrder,
            IsActive = true,
            CreatedOn = DateTimeOffset.UtcNow
        };

        await context.FgsApiEvents.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);

        return MapToDetail(entity);
    }

    public async Task<FgsApiEventDetailDto> UpdateAsync(
        long id,
        FgsApiEventUpdateDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"API event '{id}' was not found.");

        entity.EventCode = NormalizeEventCode(dto.EventCode);
        entity.EventCategory = dto.EventCategory.Trim();
        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description?.Trim();
        entity.EventVersion = dto.EventVersion;
        entity.DisplayOrder = dto.DisplayOrder;

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    public async Task<FgsApiEventDetailDto> PatchAsync(
        long id,
        FgsApiEventPatchDto dto,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindEntityAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"API event '{id}' was not found.");

        if (dto.EventCode is not null)
        {
            entity.EventCode = NormalizeEventCode(dto.EventCode);
        }

        if (dto.EventCategory is not null)
        {
            entity.EventCategory = dto.EventCategory.Trim();
        }

        if (dto.Name is not null)
        {
            entity.Name = dto.Name.Trim();
        }

        if (dto.Description is not null)
        {
            entity.Description = dto.Description.Trim();
        }

        if (dto.EventVersion.HasValue)
        {
            entity.EventVersion = dto.EventVersion.Value;
        }

        if (dto.DisplayOrder.HasValue)
        {
            entity.DisplayOrder = dto.DisplayOrder.Value;
        }

        if (dto.IsActive.HasValue)
        {
            entity.IsActive = dto.IsActive.Value;
        }

        await SaveChangesAsync(cancellationToken);
        return MapToDetail(entity);
    }

    private async Task<FgsApiEvent?> FindEntityAsync(long id, CancellationToken cancellationToken) =>
        await context.FgsApiEvents.FirstOrDefaultAsync(apiEvent => apiEvent.Id == id, cancellationToken);

    private async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            throw new InvalidOperationException(
                "An API event with the same event code already exists.",
                ex);
        }
    }

    private static bool IsUniqueViolation(DbUpdateException exception) =>
        exception.InnerException?.Message.Contains("duplicate", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("unique", StringComparison.OrdinalIgnoreCase) == true
        || exception.InnerException?.Message.Contains("23505", StringComparison.Ordinal) == true;

    private static string NormalizeEventCode(string eventCode) =>
        eventCode.Trim().ToUpperInvariant();

    private static FgsApiEventDetailDto MapToDetail(FgsApiEvent entity) =>
        new(
            entity.Id,
            entity.EventCode,
            entity.EventCategory,
            entity.Name,
            entity.Description,
            entity.EventVersion,
            entity.DisplayOrder,
            entity.IsActive);
}
