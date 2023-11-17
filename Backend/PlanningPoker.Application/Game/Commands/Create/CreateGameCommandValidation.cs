using FluentValidation;
using PlanningPoker.Application.Common.Validators;
using PlanningPoker.Domain.Common.Validation;

namespace PlanningPoker.Application.Game.Commands.Create;

public class CreateGameCommandValidation
    : AbstractValidator<CreateGameCommand>
{
    public CreateGameCommandValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(Validation.Game.Name.MaxLength)
            .MinimumLength(Validation.Game.Name.MinLength);

        RuleFor(x => x.Description)
            .MaximumLength(
                Validation.Game.Description.MaxLength
            )
            .MinimumLength(
                Validation.Game.Description.MinLength
            );

        RuleFor(x => x.OwnerId).IsGuid();
    }
}
