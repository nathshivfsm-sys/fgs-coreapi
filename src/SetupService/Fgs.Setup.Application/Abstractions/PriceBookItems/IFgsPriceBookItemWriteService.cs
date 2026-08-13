using Fgs.Setup.Application.Features.PriceBookItems.Dtos;

namespace Fgs.Setup.Application.Abstractions.PriceBookItems;

public interface IFgsPriceBookItemWriteService
{
    Task<FgsPriceBookItemDetailDto> CreateAsync(
        FgsPriceBookItemCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsPriceBookItemDetailDto> UpdateAsync(
        long id,
        FgsPriceBookItemUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsPriceBookItemDetailDto> PatchAsync(
        long id,
        FgsPriceBookItemPatchDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsPriceBookItemDetailDto> DeleteAsync(
        long id,
        CancellationToken cancellationToken = default);
}
