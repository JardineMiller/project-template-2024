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
            .Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(Validation.User.DisplayName.MaxLength);

        builder
            .Property(x => x.Bio)
            .HasMaxLength(Validation.User.Bio.MaxLength);

        builder
            .Property(x => x.AvatarFileName)
            .HasMaxLength(Validation.Common.FileName.MaxLength);
    }
}
