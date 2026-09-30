using Fgs.Contracts.Api;
using Fgs.Foundation.Paging;
using Fgs.User.Application.Common.IdentityCrud;
using Fgs.User.Application.Features.ServiceSetups.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.ServiceSetups.Queries.ListFgsTenantServiceSetups;

public sealed record ListFgsTenantServiceSetupsQuery(IdentityListQuery Query)
    : IRequest<ApiResponse<PagedResult<FgsTenantServiceSetupSummaryDto>>>;
