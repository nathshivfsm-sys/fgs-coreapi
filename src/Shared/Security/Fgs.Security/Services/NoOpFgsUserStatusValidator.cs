using Fgs.Security.Abstractions;

namespace Fgs.Security.Services;

public sealed class NoOpFgsUserStatusValidator : IFgsUserStatusValidator
{
    public Task<bool> IsActiveAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(false);
}
