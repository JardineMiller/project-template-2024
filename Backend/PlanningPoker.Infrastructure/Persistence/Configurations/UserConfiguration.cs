using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningPoker.Domain.Common.Validation;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Infrastructure.Persistence.Configurations;

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
