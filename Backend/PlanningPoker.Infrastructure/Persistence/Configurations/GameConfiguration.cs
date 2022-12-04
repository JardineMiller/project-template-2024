using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningPoker.Domain.Common.Validation;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Infrastructure.Persistence.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(ValidationConstants.Game.Name.MaxLength)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(
                ValidationConstants.Game.Description.MaxLength
            );

        builder
            .Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(ValidationConstants.Game.Code.MaxLength);

        builder
            .HasOne(x => x.Owner)
            .WithMany(x => x.Games)
            .HasForeignKey(x => x.OwnerId);
    }
}
