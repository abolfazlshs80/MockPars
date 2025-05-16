using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockPars.Domain.Models;

namespace MockPars.Infrastructure.Configuration;

public class DatabaseConfiguration : IEntityTypeConfiguration<Databases>
{
    public void Configure(EntityTypeBuilder<Databases> builder)
    {

        builder.HasKey(e => e.Id);

        builder.Property(e => e.DatabaseName).IsRequired();
        builder.Property(e => e.Slug).IsRequired();
        builder.Property(e => e.UserId).IsRequired();

        builder.HasMany(a => a.Tables)
            .WithOne(a => a.Databases)
            .HasForeignKey(a => a.DatabaseId);

    }
}