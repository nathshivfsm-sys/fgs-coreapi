using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Queries.GetFgsNonWorkingDateById;

public sealed record GetFgsNonWorkingDateByIdQuery(long Id)
    : IRequest<ApiResponse<FgsNonWorkingDateDetailDto>>;
