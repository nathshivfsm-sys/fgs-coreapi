using Fgs.Contracts.Api;
using Fgs.User.Application.Features.Companies.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Commands.PatchCompany;

public sealed record PatchCompanyCommand(long TenantId, long CompanyId, CompanyPatchDto Dto)
    : IRequest<ApiResponse<CompanyDetailDto>>;
