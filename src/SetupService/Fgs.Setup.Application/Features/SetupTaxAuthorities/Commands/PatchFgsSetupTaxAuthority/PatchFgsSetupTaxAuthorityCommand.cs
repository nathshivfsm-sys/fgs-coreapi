using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.SetupTaxAuthorities.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.SetupTaxAuthorities.Commands.PatchFgsSetupTaxAuthority;

public sealed record PatchFgsSetupTaxAuthorityCommand(long Id, FgsSetupTaxAuthorityPatchDto Dto)
    : IRequest<ApiResponse<FgsSetupTaxAuthorityDetailDto>>;
