using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.GloLookups.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.GloLookups.Queries.LookupGloAppointmentAssignmentEventTypes;

public sealed record LookupGloAppointmentAssignmentEventTypesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<GloAppointmentAssignmentEventTypeLookupDto>>>;
