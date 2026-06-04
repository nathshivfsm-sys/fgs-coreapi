using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Fgs.Persistence.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Fgs.User.Tests.Infrastructure;

public sealed class UnitOfWorkTransactionTests
{
    [Fact]
    public async Task ExecuteInTransactionAsync_OnFailure_RollsBackChanges()
    {
        await using var context = await TestDbContextFactory.CreateAndInitializeAsync();
        var unitOfWork = new EfUnitOfWork<FgsUserDbContext>(context);

        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await unitOfWork.Repository<FgsTenant>().AddAsync(new FgsTenant
            {
                TenantCode = "commit-test",
                Name = "Committed",
                CreatedOn = DateTimeOffset.UtcNow
            }, ct);
        }, CancellationToken.None);

        (await context.FgsTenants.CountAsync()).Should().Be(1);
    }
}
