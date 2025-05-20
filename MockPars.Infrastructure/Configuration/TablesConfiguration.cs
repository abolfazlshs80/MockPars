using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockPars.Domain.Models;

namespace MockPars.Infrastructure.Configuration;

public class TablesConfiguration : IEntityTypeConfiguration<Tables>
{
    public void Configure(EntityTypeBuilder<Tables> builder)
    {

        builder.HasKey(e => e.Id);
        builder.Property(a => a.TableName).IsRequired();
        builder.Property(a => a.Slug).IsRequired();

        builder.HasMany(a => a.Columns)
            .WithOne(a => a.Tables)
            .HasForeignKey(a => a.TablesId);
    }
}