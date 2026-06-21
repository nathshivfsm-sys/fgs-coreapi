using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.UpdateFgsSetupTaxAuthority;

public sealed record UpdateFgsSetupTaxAuthorityCommand(long Id, FgsSetupTaxAuthorityUpdateDto Dto)
    : IRequest<ApiResponse<FgsSetupTaxAuthorityDetailDto>>;
