using Fgs.Setup.Application.Features.GloLookups.Dtos;

namespace Fgs.Setup.Application.Abstractions.GloLookups;

public interface IGloAppointmentAssignmentEventTypeReadRepository
{
    Task<IReadOnlyList<GloAppointmentAssignmentEventTypeLookupDto>> LookupAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
}
