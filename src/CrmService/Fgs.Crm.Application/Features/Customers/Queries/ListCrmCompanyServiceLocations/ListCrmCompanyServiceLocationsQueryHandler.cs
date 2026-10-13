using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Crm.Application.Abstractions.Customers;
using Fgs.Crm.Application.Features.Customers.Dtos;
using MediatR;

namespace Fgs.Crm.Application.Features.Customers.Queries.ListCrmCompanyServiceLocations;

public sealed class ListCrmCompanyServiceLocationsQueryHandler(
    ICrmCustomerReadRepository readRepository,
    ISetupClient setupClient)
    : IRequestHandler<ListCrmCompanyServiceLocationsQuery, ApiResponse<CrmCompanyServiceLocationListResultDto>>
{
    public async Task<ApiResponse<CrmCompanyServiceLocationListResultDto>> Handle(
        ListCrmCompanyServiceLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await readRepository.ListCompanyServiceLocationsAsync(
            request.Query,
            request.Filters,
            request.IncludeSummary,
            cancellationToken);

        if (!result.Items.Any(item => item.Break1Id.HasValue || item.Break2Id.HasValue))
        {
            return ApiResponse<CrmCompanyServiceLocationListResultDto>.Ok(result);
        }

        ApiResponse<IReadOnlyList<GlBreakLookupItemDto>> lookup;
        try
        {
            lookup = await setupClient.LookupGlBreaksAsync(activeOnly: false, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            return ApiResponse<CrmCompanyServiceLocationListResultDto>.Fail(
                [string.IsNullOrWhiteSpace(ex.Message) ? "GL break lookup failed." : ex.Message],
                ApiStatusCodes.InternalServerError);
        }

        if (!lookup.Success)
        {
            return ApiResponse<CrmCompanyServiceLocationListResultDto>.Fail(
                lookup.Errors is { Count: > 0 }
                    ? lookup.Errors
                    : ["GL break lookup failed."],
                lookup.StatusCode > 0
                    ? lookup.StatusCode
                    : ApiStatusCodes.InternalServerError);
        }

        var names = new Dictionary<long, string>();
        if (lookup.Data is not null)
        {
            foreach (var item in lookup.Data)
            {
                names[item.Id] = item.Name;
            }
        }

        var items = result.Items
            .Select(item => item with
            {
                Break1Name = ResolveName(item.Break1Id, names),
                Break2Name = ResolveName(item.Break2Id, names)
            })
            .ToList();

        return ApiResponse<CrmCompanyServiceLocationListResultDto>.Ok(result with { Items = items });
    }

    private static string? ResolveName(long? breakId, IReadOnlyDictionary<long, string> names) =>
        breakId is long id && names.TryGetValue(id, out var name) ? name : null;
}
