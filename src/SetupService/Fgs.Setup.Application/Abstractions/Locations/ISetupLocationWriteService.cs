using Fgs.Setup.Application.Common.Locations;

namespace Fgs.Setup.Application.Abstractions.Locations;

public interface ISetupLocationWriteService
{
    /// <summary>
    /// Creates or updates a location row. When <paramref name="address"/> is null, soft-deletes any existing location and returns null.
    /// </summary>
    Task<Guid?> UpsertAsync(
        string masterEntityTypeCode,
        long entityNumber,
        Guid? existingLocationId,
        LocationWriteDto? address,
        CancellationToken cancellationToken = default);

    Task SoftDeleteAsync(Guid? locationId, CancellationToken cancellationToken = default);
}
