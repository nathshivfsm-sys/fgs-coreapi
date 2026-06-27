using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.FgsBusinessTypes.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.FgsBusinessTypes.Commands.PatchFgsBusinessType;

public sealed record PatchFgsBusinessTypeCommand(long Id, FgsBusinessTypePatchDto Dto)
    : IRequest<ApiResponse<FgsBusinessTypeDetailDto>>;
