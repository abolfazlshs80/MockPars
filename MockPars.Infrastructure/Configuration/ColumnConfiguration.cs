using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MockPars.Domain.Enums;
using MockPars.Domain.Models;

namespace MockPars.Infrastructure.Configuration;

public class ColumnConfiguration : IEntityTypeConfiguration<Columns>
{
    public void Configure(EntityTypeBuilder<Columns> builder)
    {

        builder.HasKey(e => e.Id);
        builder.Property(a=>a.ColumnName).IsRequired();
        builder.Property(a=>a.ColumnType).IsRequired();
        builder.Property(a=>a.TableId).IsRequired();
        builder.Property(a => a.FakeDataTypes).IsRequired()
            .HasConversion(new EnumToNumberConverter<FakeDataTypes, int>()); // تبدیل Enum به int

    }
}