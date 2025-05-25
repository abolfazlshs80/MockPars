using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MockPars.Domain.Enums;
using MockPars.Domain.Models;

namespace MockPars.Infrastructure.Configuration;

public class RecordDataConfiguration : IEntityTypeConfiguration<RecordData>
{
    public void Configure(EntityTypeBuilder<RecordData> builder)
    {

        builder.HasKey(e => e.Id);
        builder.Property(a => a.Value).IsRequired();
        builder.HasOne(_ => _.Columns)
            .WithMany(_ => _.RecordData)
            .HasForeignKey(_ => _.ColumnsId);

    }
}