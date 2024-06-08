using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTemplate2024.Domain.Common.Validation;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Infrastructure.Persistence.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(Validation.Game.Name.MaxLength)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(
                Validation.Game.Description.MaxLength
            );

        builder
            .Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(Validation.Game.Code.MaxLength);

        builder
            .HasOne(x => x.Owner)
            .WithMany(x => x.Games)
            .HasForeignKey(x => x.OwnerId);
    }
}
