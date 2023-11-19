using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanningPoker.Domain.Common.Validation;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Infrastructure.Persistence.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<Player>(x => x.UserId);

        builder
            .Property(x => x.DisplayName)
            .HasMaxLength(Validation.Player.DisplayName.MaxLength)
            .IsRequired();

        builder.Ignore(x => x.IsAnon);

        builder
            .HasMany(x => x.Games)
            .WithMany(x => x.Players)
            .UsingEntity(x => x.ToTable("GamePlayers"));
    }
}
