using Fgs.Setup.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fgs.Setup.Infrastructure.Database.Configurations;

internal class GloTimeCardOptionConfiguration : IEntityTypeConfiguration<GloTimeCardOption>
{
    public void Configure(EntityTypeBuilder<GloTimeCardOption> entity)
    {
        entity.ToTable("GloTimeCardOption");
        entity.HasKey(e => e.Id);
        entity.HasAlternateKey(e => e.Code).HasName("UQ_GloTimeCardOption_Code");
        entity.ToTable(t => t.HasCheckConstraint(
            "CK_GloTimeCardOption_Code_Upper",
            "\"Code\" = UPPER(\"Code\")"));
    }
}
