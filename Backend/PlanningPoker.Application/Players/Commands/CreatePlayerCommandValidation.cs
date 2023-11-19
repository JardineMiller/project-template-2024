using FluentValidation;
using PlanningPoker.Domain.Common.Validation;

namespace PlanningPoker.Application.Players.Commands;

public class CreatePlayerCommandValidation
    : AbstractValidator<CreatePlayerCommand>
{
    public CreatePlayerCommandValidation()
    {
        RuleFor(x => x.DisplayName)
            .MinimumLength(Validation.Player.DisplayName.MinLength);

        RuleFor(x => x.DisplayName)
            .MaximumLength(Validation.Player.DisplayName.MaxLength);
    }
}
