using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Queries.LookupNonWorkingDates;

public sealed record LookupNonWorkingDatesQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsNonWorkingDateLookupDto>>>;
