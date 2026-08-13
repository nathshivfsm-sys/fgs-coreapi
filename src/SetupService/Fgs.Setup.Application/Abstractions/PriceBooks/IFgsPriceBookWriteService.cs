using Fgs.Setup.Application.Features.PriceBooks.Dtos;

namespace Fgs.Setup.Application.Abstractions.PriceBooks;

public interface IFgsPriceBookWriteService
{
    Task<FgsPriceBookDetailDto> CreateAsync(
        FgsPriceBookCreateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsPriceBookDetailDto> UpdateAsync(
        long id,
        FgsPriceBookUpdateDto dto,
        CancellationToken cancellationToken = default);

    Task<FgsPriceBookDetailDto> PatchAsync(
        long id,
        FgsPriceBookPatchDto dto,
        CancellationToken cancellationToken = default);
}
