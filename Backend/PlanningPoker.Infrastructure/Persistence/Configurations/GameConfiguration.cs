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
            .HasMaxLength(Validation.Game.Name.maxLength)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(
                Validation.Game.Description.maxLength
            );

        builder
            .Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(Validation.Game.Code.maxLength);

        builder
            .HasOne(x => x.Owner)
            .WithMany(x => x.Games)
            .HasForeignKey(x => x.OwnerId);
    }
}
