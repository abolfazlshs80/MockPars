using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MockPars.Domain.Models;

namespace MockPars.Infrastructure.Configuration
{



    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(e => e.Id);
            builder.HasMany(a => a.Databases)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

        }
    }
}
