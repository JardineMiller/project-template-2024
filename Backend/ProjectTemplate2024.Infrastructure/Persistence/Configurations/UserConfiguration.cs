using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTemplate2024.Domain.Common.Validation;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(Validation.User.FirstName.MaxLength);

        builder
            .Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(Validation.User.LastName.MaxLength);
    }
}
