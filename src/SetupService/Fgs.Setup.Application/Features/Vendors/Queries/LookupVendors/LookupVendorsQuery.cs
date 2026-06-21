using Fgs.Contracts.Api;
using Fgs.Setup.Application.Features.Vendors.Dtos;
using MediatR;

namespace Fgs.Setup.Application.Features.Vendors.Queries.LookupVendors;

public sealed record LookupVendorsQuery(bool ActiveOnly = true)
    : IRequest<ApiResponse<IReadOnlyList<FgsVendorLookupDto>>>;
