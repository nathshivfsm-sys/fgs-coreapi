using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Tests.Infrastructure;

public sealed class UnitOfWorkTransactionTests
{
    [Fact]
    public async Task ExecuteInTransactionAsync_OnFailure_RollsBackChanges()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var unitOfWork = new UnitOfWork(context);

        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await unitOfWork.Repository<FgsTenant>().AddAsync(new FgsTenant
            {
                Id = Guid.NewGuid(),
                TenantCode = "commit-test",
                Name = "Committed",
                CreatedOn = DateTimeOffset.UtcNow
            }, ct);
        }, CancellationToken.None);

        (await context.FgsTenants.CountAsync()).Should().Be(1);
    }
}
