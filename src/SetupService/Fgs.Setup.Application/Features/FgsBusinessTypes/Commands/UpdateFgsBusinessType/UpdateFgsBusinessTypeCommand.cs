using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.UpdateFgsBusinessType;

public sealed record UpdateFgsBusinessTypeCommand(long Id, FgsBusinessTypeUpdateDto Dto)
    : IRequest<ApiResponse<FgsBusinessTypeDetailDto>>;
