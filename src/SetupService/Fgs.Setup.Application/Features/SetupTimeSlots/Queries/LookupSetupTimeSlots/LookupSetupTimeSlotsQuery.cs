using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTimeSlots.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTimeSlots.Queries.LookupSetupTimeSlots;

public sealed record LookupSetupTimeSlotsQuery(
    bool ActiveOnly = true,
    bool? IsMobileVisible = null,
    bool? IsCustomerPortalVisible = null)
    : IRequest<ApiResponse<IReadOnlyList<FgsSetupTimeSlotLookupDto>>>;
