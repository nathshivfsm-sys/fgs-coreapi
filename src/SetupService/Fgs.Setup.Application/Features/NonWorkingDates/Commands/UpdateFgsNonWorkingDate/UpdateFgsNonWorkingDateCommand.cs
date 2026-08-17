using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.NonWorkingDates.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.NonWorkingDates.Commands.UpdateFgsNonWorkingDate;

public sealed record UpdateFgsNonWorkingDateCommand(long Id, FgsNonWorkingDateUpdateDto Dto)
    : IRequest<ApiResponse<FgsNonWorkingDateDetailDto>>;
