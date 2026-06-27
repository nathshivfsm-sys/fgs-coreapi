using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.DeleteFgsBusinessType;

public sealed record DeleteFgsBusinessTypeCommand(long Id)
    : IRequest<ApiResponse<FgsBusinessTypeDetailDto>>;
